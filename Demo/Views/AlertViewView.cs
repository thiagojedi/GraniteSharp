using Gtk;

namespace Demo.Views
{
    public class AlertViewView : Grid
    {
        public AlertViewView()
        {
            var alert = new Granite.Widgets.AlertView("Nothing here",
                "Maybe you can enable <b>something</b> to hide it but <i>otherwise</i> it will stay here",
                "dialog-warning");
            alert.ShowAction("Hide this button");

            alert.ActionActivated += (sender, args) => alert.HideAction();

            Add(alert);
        }
    }
}