using Gtk;

namespace Demo.Views.SettingsView
{
    public class SimpleSettingsPage : Granite.Widgets.SimpleSettingsPage
    {
        public SimpleSettingsPage() : base(
            "First Test Page",
            "This is a demo of Granite's SimpleSettingsPage",
            "preferences-system",
            true)
        {
            Header = "Simple Pages";

            var iconLabel = new Label("Icon Name:") { Xalign = 1 };

            var iconEntry = new Entry { Hexpand = true, PlaceholderText = "This page's icon name", Text = IconName };

            var titleLabel = new Label("Title:") { Xalign = 1 };

            var titleEntry = new Entry { Hexpand = true, PlaceholderText = "This page's title" };

            var descriptionLabel = new Label("Description:") { Xalign = 1 };

            var descriptionEntry = new Entry { Hexpand = true, PlaceholderText = "This page's description" };

            ContentArea.Attach(iconLabel, 0, 0, 1, 1);
            ContentArea.Attach(iconEntry, 1, 0, 1, 1);
            ContentArea.Attach(titleLabel, 0, 1, 1, 1);
            ContentArea.Attach(titleEntry, 1, 1, 1, 1);
            ContentArea.Attach(descriptionLabel, 0, 2, 1, 1);
            ContentArea.Attach(descriptionEntry, 1, 2, 1, 1);

            var button = new Button("Test Button");

            UpdateStatus();

            descriptionEntry.Changed += (sender, args) => { Description = descriptionEntry.Text; };

            iconEntry.Changed += (sender, args) => { IconName = iconEntry.Text; };

            StatusSwitch.AddNotification("active", (o, args) => UpdateStatus());

            titleEntry.Changed += (sender, args) => { Title = titleEntry.Text; };

            ActionArea.Add(button);
        }

        private void UpdateStatus()
        {
            if (StatusSwitch.Active)
            {
                StatusType = SettingsStatusType.SUCCESS;
                Status = "Enabled";
            }
            else
            {
                StatusType = SettingsStatusType.OFFLINE;
                Status = "Disabled";
            }
        }
    }
}