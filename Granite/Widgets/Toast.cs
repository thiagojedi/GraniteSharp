using System;
using Gdk;
using Gtk;

namespace Granite.Widgets
{
        public class Toast : Revealer {

        /**
         * Emitted when the Toast is closed by activating the close button
         */
        public event ClosedHandler Closed;

        /**
         * Emitted when the default action button is activated
         */
        public event EventHandler DefaultAction;

        private readonly Label _notificationLabel;
        private readonly Button _defaultActionButton;
        private string _title;
        private uint _timeoutId;

        /**
         * The notification text label to be displayed inside of #this
         */
        public string Title {
            get => _title;
            set {
                if (_notificationLabel != null) {
                    _notificationLabel.Text = value;
                }
                _title = value;
            }
        }

        /**
         * Creates a new Toast with #title as its title
         */
        public Toast(string title) : this()
        {
            Title = title;
        }

        public Toast()
        {
            Margin = 3;
            Halign = Align.Center;
            Valign = Align.Start;

            _defaultActionButton = new Button {Visible = false, NoShowAll = true};
            _defaultActionButton.Clicked += (o,e) =>
            {
                RevealChild = false;
                if (_timeoutId != 0)
                {
                    RemoveTickCallback(_timeoutId);
                    _timeoutId = 0;
                }

                DefaultAction?.Invoke(this, null);
            };

            var closeButton = new Button("window-close-symbolic", IconSize.Menu);
            closeButton.StyleContext.AddClass("close-button");
            closeButton.Clicked += (o,e) => {
                RevealChild = false;
                if (_timeoutId != 0) {
                    RemoveTickCallback(_timeoutId);
                    _timeoutId = 0;
                }
                Closed?.Invoke(this,null);
            };

            _notificationLabel = new Label ();

            var notificationBox = new Grid {ColumnSpacing = 12};
            notificationBox.Add (closeButton);
            notificationBox.Add (_notificationLabel);
            notificationBox.Add (_defaultActionButton);

            var notificationFrame = new Frame (null);
            notificationFrame.StyleContext.AddClass ("app-notification");
            notificationFrame.Add (notificationBox);

            Add (notificationFrame);
        }

        /**
         * Sets the default action button label of #this to #label and hides the
         * button if #label is #null.
         */
        public void SetDefaultAction (string label) {
            if (string.IsNullOrEmpty(label)) {
                _defaultActionButton.NoShowAll = true;
                _defaultActionButton.Visible = false;
            } else {
                _defaultActionButton.NoShowAll = false;
                _defaultActionButton.Visible = true;
            }
            _defaultActionButton.Label = label;
        }

        /**
         * Sends the Toast on behalf of #this
         */
        public void SendNotification () {
            if (ChildRevealed) return;
            RevealChild = true;

            var duration = (uint) (_defaultActionButton.Visible ? 3500 : 2000);

            _timeoutId = GLib.Timeout.Add (duration, () => {
                RevealChild = false;
                _timeoutId = 0;
                return false;
            });
        }
    }
}