using System;
using System.Globalization;
using Gdk;
using GLib;
using Gtk;
using Calendar = Gtk.Calendar;
using DateTime = System.DateTime;

namespace Granite.Widgets
{
    public class DatePicker : Entry
    {
        private readonly Popover _popover;
        private readonly Calendar _calendar;
        private DateTime _date;
        private bool _procNextDaySelected;

        public event EventHandler DateChanged;

        public string Format { get; }

        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                Text = _date.ToString(Format);
                _procNextDaySelected = false;
                _calendar.SelectMonth((uint)(value.Month - 1), (uint)value.Year);
                _procNextDaySelected = false;
                _calendar.SelectDay((uint)value.Day);
                DateChanged?.Invoke(this, null);
            }
        }

        public DatePicker() : this(DateTimeFormatInfo.CurrentInfo.LongDatePattern)
        {
        }

        public DatePicker(string format)
        {
            Format = format;
            var dropdown = new EventBox { Margin = 6 };
            _popover = new Popover(this) { dropdown };
            _calendar = new Calendar();
            _date = DateTime.Now;

            CanFocus = false;
            IsEditable = false;
            SecondaryIconGicon = ThemedIcon.NewWithDefaultFallbacks("office-calendar-symbolic");

            dropdown.AddEvents((int)EventMask.FocusChangeMask);
            dropdown.Add(_calendar);

            IconRelease += OnIconRelease;
            _calendar.DaySelected += OnCalendarDaySelected;

            _calendar.NextMonth += (sender, args) => _procNextDaySelected = false;
            _calendar.NextYear += (sender, args) => _procNextDaySelected = false;
            _calendar.PrevMonth += (sender, args) => _procNextDaySelected = false;
            _calendar.PrevYear += (sender, args) => _procNextDaySelected = false;
        }

        private void OnCalendarDaySelected(object sender, EventArgs e)
        {
            if (_procNextDaySelected)
            {
                _date = new DateTime(_calendar.Year, _calendar.Month + 1, _calendar.Day);
                HideDropdown();
            }
            else
            {
                _procNextDaySelected = true;
            }
        }

        private void OnIconRelease(object o, IconReleaseArgs args)
        {
            PositionDropdown(out var rect);
            _popover.PointingTo = rect;
            _popover.Position = PositionType.Bottom;
            _popover.ShowAll();
            _calendar.GrabFocus();
        }

        private void PositionDropdown(out Rectangle rect)
        {
            var size = Allocation;

            rect = new Rectangle
            {
                X = size.Width - Offset,
                Y = size.Height,
            };
        }

        private const int Offset = 15;

        private void HideDropdown()
        {
            _popover.Hide();
        }
    }
}