using Gtk;

namespace Granite.Widgets
{
    public class SettingsSidebar : ScrolledWindow
    {
        private ListBox _listBox;

        public Stack Stack { get; }

        public SettingsSidebar(Stack stack) : this()
        {
            Stack = stack;
            OnSidebarChanged();
            Stack.Added += (o, args) => OnSidebarChanged();
            Stack.Removed += (o, args) => OnSidebarChanged();
        }

        private SettingsSidebar()
        {
            HscrollbarPolicy = PolicyType.Never;
            WidthRequest = 200;
            _listBox = new ListBox {ActivateOnSingleClick = true, SelectionMode = SelectionMode.Single};

            Add(_listBox);

            _listBox.RowSelected += (o, args) =>
            {
                Stack.VisibleChild = ((SettingsSidebarRow) args.Row).Page;
            };

            _listBox.HeaderFunc = (row, before) =>
            {
                var header = ((SettingsSidebarRow) row).Header;
                if (header != null)
                {
                    row.Header = new HeaderLabel(header);
                }
            };
        }

        private void OnSidebarChanged()
        {
            foreach (var listBoxChild in _listBox.Children)
                listBoxChild.Destroy();

            foreach (var child in Stack.Children)
            {
                if (!(child is SettingsPage page)) continue;
                var row = new SettingsSidebarRow(page);
                _listBox.Add(row);
            }

            _listBox.ShowAll();
        }
    }
}