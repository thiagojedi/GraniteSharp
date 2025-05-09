using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Gdk;
using GLib;
using Gtk;
using Pango;
using Key = Gdk.Key;

namespace Granite.Widgets
{
    public class HyperTextView : TextView
    {
        private const int ForceFullBufferRescanChangeStartOffset = -1;
        
        private readonly Regex _uriRegex;
        private readonly Dictionary<string, TextTag> _uriTextTags;
        private uint _bufferChangedDebounceTimeoutId;

        private int _bufferCursorPositionWhenChangedStarted;
        private bool _isControlKeyPressed;

        public HyperTextView()
        {
            var httpCharset = "[\\w\\/\\-\\+\\.:@\\?&%=#]";
            var emailCharset = "[\\w\\-\\.]";
            var emailTldCharset = "[\\w\\-]";

            var httpMatchStr = $@"https?:\/\/{httpCharset}+\.{httpCharset}+";
            var emailMatchStr = $"(mailto:)?{emailCharset}+@{emailCharset}+\\.{emailTldCharset}+";

            var uriRegexStr = $"(?:({httpMatchStr})|({emailMatchStr}))";

            _uriTextTags = new Dictionary<string, TextTag>();

            try
            {
                _uriRegex = new Regex(uriRegexStr);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

            BufferConnect(Buffer);
            Buffer.AddNotification("connect", (o, args) => { BufferConnect(Buffer); });

        }

        private void OnFocusOutEvent(object o, FocusOutEventArgs args)
        {
            _isControlKeyPressed = false;
        }

        private void OnMotionNotifyEvent(object o, MotionNotifyEventArgs args)
        {
            var @event = args.Event;
            var uri = GetUriAtLocation((int)@event.X, (int)@event.Y);
            if (uri != null && !HasTooltip)
            {
                HasTooltip = true;
                TooltipMarkup = string.Join("\n", "Follow Link", "Control + Click");
            }
            else if (uri == null && HasTooltip)
            {
                HasTooltip = false;
            }
        }

        private void OnButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
        {
            if (!_isControlKeyPressed) return;

            var textIter = Buffer.GetIterAtMark(Buffer.InsertMark);

            var tags = textIter.Tags;
            foreach (var tag in tags)
                if (tag.Data["uri"] != null)
                {
                    var uri = tag.Data["uri"].ToString();

                    try
                    {
                        AppInfoAdapter.LaunchDefaultForUri(uri, null);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }

                    break;
                }
        }

        protected override void OnShown()
        {
            base.OnShown();

            /*
             * Binding key_press/key_release signals to all toplevel
             * windows possible enables us to detect when the Control
             * key is pressed even when HyperTextView is not focused.
             */
            var topLevelWindows = Gtk.Window.ListToplevels();

            if (topLevelWindows.Length != 0)
            {
                foreach (var window in topLevelWindows)
                {
                    window.KeyPressEvent += OnKeyPressEvent;
                    window.KeyReleaseEvent += OnKeyReleasedEvent;
                }
            }
            else
            {
                Debug.WriteLine("Could not bind to top level", "warning");
                KeyPressEvent += OnKeyPressEvent;
                KeyReleaseEvent += OnKeyReleasedEvent;
            }
            
            ButtonReleaseEvent += OnButtonReleaseEvent;
            MotionNotifyEvent += OnMotionNotifyEvent;
            FocusOutEvent += OnFocusOutEvent;
        }

        private void OnKeyReleasedEvent(object o, KeyReleaseEventArgs args)
        {
            var @event = args.Event;

            if (@event.KeyValue == (uint)Key.Control_L || @event.KeyValue == (uint)Key.Control_R)
            {
                var window = GetWindow(TextWindowType.Text);
                if (_isControlKeyPressed && window != null) window.Cursor = new Cursor(Display, "text");

                _isControlKeyPressed = false;
            }
        }

        private void OnKeyPressEvent(object o, KeyPressEventArgs args)
        {
            var @event = args.Event;
            if (@event.KeyValue == (uint)Key.Control_L || @event.KeyValue == (uint)Key.Control_R)
            {
                var window = GetWindow(TextWindowType.Text);
                if (window != null)
                {
                    var pointerDevice = window.Display.DefaultSeat.Pointer;
                    if (pointerDevice != null)
                    {
                        window.GetDevicePosition(pointerDevice, out var pointerX, out var pointerY, out var mask);

                        var uriHoveringOver = GetUriAtLocation(pointerX, pointerY);
                        if (uriHoveringOver != null) window.Cursor = new Cursor(Display, "pointer");
                    }
                }

                _isControlKeyPressed = true;
            }
        }

        private string GetUriAtLocation(int locationX, int locationY)
        {
            var window = GetWindow(TextWindowType.Widget);
            if (window == null) return null;

            WindowToBufferCoords(TextWindowType.Text, locationX, locationY, out var x, out var y);

            if (!GetIterAtLocation(out var iter, x, y)) return null;

            return iter.Tags
                .FirstOrDefault(tag => tag.Data.ContainsKey("uri"))?.Data["uri"]
                .ToString();
        }


        private void BufferConnect(TextBuffer buffer)
        {
            buffer.AddNotification("cursor-position", OnBufferCursorPositionChanged);
            buffer.PasteDone += OnPastDone;
            buffer.Changed += OnBufferChanged;
        }

        private void OnBufferCursorPositionChanged(object o, NotifyArgs args)
        {
            if (_bufferCursorPositionWhenChangedStarted == 0)
                _bufferCursorPositionWhenChangedStarted = Buffer.CursorPosition;
        }

        private void OnPastDone(object o, PasteDoneArgs args)
        {
            // force rescan of whole buffer:
            _bufferCursorPositionWhenChangedStarted = ForceFullBufferRescanChangeStartOffset;
        }


        private void OnBufferChanged(object sender, EventArgs e)
        {
            if (_bufferChangedDebounceTimeoutId != 0)
            {
                Source.Remove(_bufferChangedDebounceTimeoutId);
                _bufferChangedDebounceTimeoutId = 0;
            }

            _bufferChangedDebounceTimeoutId = Timeout.Add(300, () =>
            {
                _bufferChangedDebounceTimeoutId = 0;

                var changeStartOffset = _bufferCursorPositionWhenChangedStarted;
                var changeEndOffset = Buffer.CursorPosition;

                _bufferCursorPositionWhenChangedStarted = 0;

                if (changeStartOffset == ForceFullBufferRescanChangeStartOffset ||
                    changeStartOffset == changeEndOffset)
                {
                    changeStartOffset = 0;
                    changeEndOffset = Buffer.Text.Length;
                }

                UpdateTagsInBufferForRange(Math.Min(changeStartOffset, changeEndOffset),
                    Math.Max(changeStartOffset, changeEndOffset));

                return false;
            });
        }

        private void UpdateTagsInBufferForRange(int bufferStartOffset, int bufferEndOffset)
        {
            if (bufferStartOffset == bufferEndOffset) return;

            var bufferStartIter = Buffer.GetIterAtOffset(bufferStartOffset);
            bufferStartIter.BackwardLine();
            bufferStartOffset = bufferStartIter.Offset;

            var bufferEndIter = Buffer.GetIterAtOffset(bufferEndOffset);
            bufferEndIter.ForwardLine();
            bufferEndOffset = bufferEndIter.Offset;

            // Delete all tags in buffer for range [start_iter.offset,end_iter.offset]
            foreach (var tagKey in _uriTextTags)
            {
                var m = Regex.Match(tagKey.Key, @"\[(\d+),(\d+)\]");

                var tagStart = int.Parse(m.Groups[1].Value);
                var tagEnd = int.Parse(m.Groups[2].Value);

                if ((tagStart > bufferStartOffset && tagStart < bufferEndOffset) ||
                    (tagEnd > bufferStartOffset && tagEnd < bufferEndOffset))
                    Buffer.TagTable.Remove(tagKey.Value);
            }


            var bufferSubstring = Buffer.Text.Substring(bufferStartOffset, bufferEndOffset - bufferStartOffset);

            if (string.IsNullOrWhiteSpace(bufferSubstring)) return;

            var matches = _uriRegex.Matches(bufferSubstring);
            foreach (Match match in matches)
            {
                var bufferMatchStartOffset = bufferStartOffset + match.Index;
                var bufferMatchEndOffset = bufferMatchStartOffset + match.Length;
                var bufferMatchStartIter = Buffer.GetIterAtOffset(bufferMatchStartOffset);
                var bufferMatchEndIter = Buffer.GetIterAtOffset(bufferMatchEndOffset);

                var matchText = match.Value;

                var tag = new TextTag("underline")
                {
                    Underline = Underline.Single
                };
                if (matchText.Contains("://") && matchText.Contains("@") && !matchText.StartsWith("mailto:"))
                    matchText = "mailto:" + matchText;

                tag.Data.Add("uri", matchText);
                Buffer.TagTable.Add(tag);
                Buffer.ApplyTag(tag, bufferMatchStartIter, bufferMatchEndIter);

                var newKey = $"[{bufferMatchStartOffset:D},{bufferMatchEndOffset:D}]";
                _uriTextTags[newKey] = tag;
            }
        }
    }
}