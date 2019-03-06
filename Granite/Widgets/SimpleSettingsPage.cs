using Gtk;
using Pango;

namespace Granite.Widgets
{
    public abstract class SimpleSettingsPage : SettingsPage
    {
        private Image _headerIcon;
        private Label _descriptionLabel;
        private Label _titleLabel;
        private string _description;

        /**
         * A {@link Gtk.ButtonbBox} used as the action area for #this
         */
        public ButtonBox ActionArea { get; }

        /**
         * A {@link Gtk.Grid} used as the content area for #this
         */
        public Grid ContentArea { get; }

        /**
         * A {@link Gtk.Switch} that appears in the header area when #this.activatable is #true. #status_switch will be #null when #this.activatable is #false
         */
        public Switch StatusSwitch { get; }

        /**
         * Creates a {@link Gtk.Switch} #status_switch in the header of #this
         */
        public bool Activatable { get; }

        /**
         * Creates a {@link Gtk.Label} with a page description in the header of #this
         */
        public string Description
        {
            get => _description;
            set
            {
                if (_descriptionLabel != null) 
                    _descriptionLabel.Text = value;

                _description = value;
            }
        }

        /**
         * An icon name associated with #this
         * Deprecated: Use #SettingsPage.icon_name instead.
         */
        public new string IconName
        {
            get => _iconName;
            set
            {
                if (_headerIcon != null) 
                    _headerIcon.IconName = value;
                _iconName = value;
            }
        }

        /**
         * A title associated with #this
         * Deprecated: Use #SettingsPage.title instead.
         */
        public new string Title
        {
            get => _title;
            set
            {
                if (_titleLabel != null)
                    _titleLabel.Text = value;

                _title = value;
            }
        }

        protected SimpleSettingsPage(string title, string description, string iconName, bool activatable = false)
        {
            Title = title;
            IconName = iconName;
            Description = description;
            Activatable = activatable;
            
            _headerIcon = Image.NewFromIconName(_iconName, IconSize.Dialog);
            _headerIcon.PixelSize = 48;
            _headerIcon.Valign = Align.Start;

            _titleLabel = new Label(_title) {Ellipsize = EllipsizeMode.End, Xalign = 0};
            _titleLabel.StyleContext.AddClass("h2");

            var headerArea = new Grid {ColumnSpacing = 12, RowSpacing = 3};

            headerArea.Attach(_titleLabel, 1, 0, 1, 1);

            if (description != null)
            {
                _descriptionLabel = new Label(description) {Xalign = 0, Wrap = true};

                headerArea.Attach(_headerIcon, 0, 0, 1, 2);
                headerArea.Attach(_descriptionLabel, 1, 1, 1, 1);
            }
            else
            {
                headerArea.Attach(_headerIcon, 0, 0, 1, 1);
            }

            if (activatable)
            {
                StatusSwitch = new Switch {Hexpand = true, Halign = Align.End, Valign = Align.Center};
                headerArea.Attach(StatusSwitch, 2, 0, 1, 1);
            }

            ContentArea = new Grid {ColumnSpacing = 12, RowSpacing = 12, Vexpand = true};

            ActionArea = new ButtonBox(Orientation.Horizontal) {Layout = (ButtonBoxStyle.End), Spacing = 6};

            var grid = new Grid {Margin = 12, Orientation = Orientation.Vertical, RowSpacing = 24};
            grid.Add(headerArea);
            grid.Add(ContentArea);
            grid.Add(ActionArea);

            Add(grid);

            SetActionAreaVisibility();

            ActionArea.Added += (o, e) => SetActionAreaVisibility();
            ActionArea.Removed += (o, e) => SetActionAreaVisibility();

//            notify["icon-name"].connect(() =>
//            {
//                if (header_icon != null)
//                {
//                    header_icon.icon_name = icon_name;
//                }
//            });
//
//            notify["title"].connect(() =>
//            {
//                if (title_label != null)
//                {
//                    title_label.label = title;
//                }
//            });
        }

        private void SetActionAreaVisibility()
        {
            if (ActionArea.Children != null)
            {
                ActionArea.NoShowAll = false;
                ActionArea.Show();
            }
            else
            {
                ActionArea.NoShowAll = true;
                ActionArea.Hide();
            }
        }
    }
}