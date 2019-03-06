using Gtk;

namespace Demo.Views.SettingsView
{
    public class SettingsView : Paned
    {
        public SettingsView() : base(Orientation.Horizontal)
        {
            var settingsPage = new SimpleSettingsPage();

            var settingsPageTwo = new SettingsPage();

            var stack = new Stack();
            stack.AddNamed(settingsPage, nameof(settingsPage));
            stack.AddNamed(settingsPageTwo, nameof(settingsPageTwo));

            var settingsSidebar = new Granite.Widgets.SettingsSidebar(stack);

            Add(settingsSidebar);
            Add(stack);
        }
    }
}