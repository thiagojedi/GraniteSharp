using System;
using System.Globalization;
using Granite;
using Granite.Widgets;
using Gtk;
using Humanizer;

namespace Demo.Views;

public class DateTimePickerView : Grid
{
    private readonly DatePicker _datePicker;
    private readonly Label _relativeDateTime;

    public DateTimePickerView()
    {
        var pickersLabel = new Label("Picker Widgets") { Xalign = 0 };
        pickersLabel.StyleContext.AddClass(StyleClass.H4Label);

        var dateLabel = new Label("DatePicker:") { Halign = Align.End };

        _datePicker = new DatePicker();

        var timeLabel = new Label("TimePicker:") { Halign = Align.End };

        // TODO create TimePicker

        var formattingLabel = new Label("String formating")
        {
            MarginTop = 6,
            Xalign = 0,
        };
        formattingLabel.StyleContext.AddClass(StyleClass.H4Label);

        var currentTimeLabel = new Label("Localized time:") { Halign = Align.End };

        var now = DateTime.Now;
        var timeFormat = DateTimeFormatInfo.CurrentInfo.ShortTimePattern;
        var currentTime = new Label(now.ToShortTimeString()) { TooltipText = timeFormat, Xalign = 0 };

        var currentDateLabel = new Label("Localized date:") { Halign = Align.End };

        var relativeDateTimeLabel = new Label("Relative datetime:") { Halign = Align.End };

        _relativeDateTime = new Label("") {Xalign = 0};
        SetSelectedDateTime(null, null);
        _datePicker.Changed += SetSelectedDateTime;


        ColumnSpacing = 12;
        RowSpacing = 6;
        Valign = Align.Center;
        Halign = Align.Center;

        Attach(pickersLabel, 0, 0, 1, 1);
        Attach(dateLabel, 0, 1, 1, 1);
        Attach(_datePicker, 1, 1, 1, 1);
        Attach(timeLabel, 0, 2, 1, 1);
        Attach(formattingLabel, 0, 3, 1, 1);
        Attach(currentTimeLabel, 0, 4, 1, 1);
        Attach(currentTime, 1, 4, 1, 1);
        Attach(currentDateLabel, 0, 5, 1, 1);
        Attach(relativeDateTimeLabel, 0, 6, 1, 1);
        Attach(_relativeDateTime, 1, 6, 1, 1);
    }

    private void SetSelectedDateTime(object sender, EventArgs e)
    {
        var selectedDateTime = _datePicker.Date;

        _relativeDateTime.Text = selectedDateTime.Humanize();
    }
}