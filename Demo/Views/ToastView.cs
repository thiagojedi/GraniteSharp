using Granite.Widgets;
using Gtk;

namespace Demo.Views
{
    public class ToastView: Overlay
    {
        public ToastView()
        {
            var toast = new Toast ("Button was pressed!");
            toast.SetDefaultAction("Do Things");

            var button = new Button {Label = "Press Me"};

            var grid = new Grid
            {
                Orientation = Orientation.Vertical,
                Margin = 24,
                Halign = Align.Center,
                Valign = Align.Center,
                RowSpacing = 6
            };
            grid.Add (button);

            AddOverlay (grid);
            AddOverlay (toast);

            button.Clicked += (sender, args) => toast.SendNotification();

            toast.DefaultAction += (sender, args) => {
                var label = new Label ("Did The Thing");
                toast.Title = "Already did the thing";
                toast.SetDefaultAction(null);
                grid.Add (label);
                grid.ShowAll();
            };
        }
    }
}