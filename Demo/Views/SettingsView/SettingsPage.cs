using System;

namespace Demo.Views.SettingsView
{
    public class SettingsPage : Granite.Widgets.SettingsPage
    {
        public SettingsPage()
        {
            var userName = Environment.UserName;

            Header = "Manual";
            DisplayWidget = null;
            Status = userName;
            Title = "Avatar Test Page";

            var titleLabel = new Gtk.Label("Title:") {Xalign = 1};

            var titleEntry = new Gtk.Entry {Hexpand = true, PlaceholderText = "This page's title"};

            var contentArea = new Gtk.Grid {ColumnSpacing = 12, RowSpacing = 12, Margin = 12};
            contentArea.Attach(titleLabel, 0, 1, 1, 1);
            contentArea.Attach(titleEntry, 1, 1, 1, 1);

            Add(contentArea);

            titleEntry.Changed += (sender, args) => { Title = titleEntry.Text; };
        }
    }
}