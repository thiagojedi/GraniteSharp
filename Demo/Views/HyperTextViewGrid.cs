using Granite.Widgets;
using Gtk;

namespace Demo.Views
{
    public class HyperTextViewGrid : Grid
    {
        public HyperTextViewGrid()
        {
            var headerLabel = new HeaderLabel("Hold Ctrl and click to follow the link");
            var hypertextTextView = new HyperTextView();
            hypertextTextView.Buffer.Text =
                "elementary OS - https://elementary.io/\nThe fast, open and privacy-respecting replacement for Windows and macOS.";

            var hypertextScrolledWindow = new ScrolledWindow()
            {
                HeightRequest = 300,
                WidthRequest = 600
            };
            hypertextScrolledWindow.Add(hypertextTextView);

            Margin = 12;
            Orientation = Orientation.Vertical;
            RowSpacing = 3;
            Halign = Align.Center;
            Valign = Align.Center;
            Vexpand = true;

            Add(headerLabel);
            Add(hypertextScrolledWindow);
        }
    }
}