using System;
using Gtk;
using Pango;

namespace Granite.Widgets
{
    public class SettingsSidebarRow : ListBoxRow
    {
        public SettingsPage.SettingsStatusType StatusType
        {
            set
            {
                switch (value)
                {
                    case SettingsPage.SettingsStatusType.ERROR:
                        _statusIcon.IconName = "user-busy";
                        break;
                    case SettingsPage.SettingsStatusType.OFFLINE:
                        _statusIcon.IconName = "user-offline";
                        break;
                    case SettingsPage.SettingsStatusType.SUCCESS:
                        _statusIcon.IconName = "user-available";
                        break;
                    case SettingsPage.SettingsStatusType.WARNING:
                        _statusIcon.IconName = "user-away";
                        break;
                    case SettingsPage.SettingsStatusType.NONE:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
        }

        public Widget DisplayWidget { get; }

        public new string Header { get; set; }

        public SettingsPage Page { get; }

        public string IconName
        {
            get => _iconName;
            set
            {
                _iconName = value;
                if (!(DisplayWidget is Image image)) return;
                image.IconName = value;
                image.PixelSize = 32;
            }
        }

        public string Status
        {
            set
            {
                _statusLabel.LabelProp = $"<span font_size='small'>{value}</span>";
                _statusLabel.NoShowAll = false;
                _statusLabel.Show();
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                _titleLabel.Text = value;
            }
        }

        private Image _statusIcon;
        private Label _statusLabel;
        private Label _titleLabel;
        private string _iconName;
        private string _title;

        public SettingsSidebarRow(SettingsPage page)
        {
            Page = page;
            _titleLabel = new Label(Page.Title) {Ellipsize = EllipsizeMode.End, Xalign = 0};
            _titleLabel.StyleContext.AddClass("h3");

            _statusIcon = new Image {Halign = Align.End, Valign = Align.End};

            _statusLabel = new Label(null)
            {
                NoShowAll = true, UseMarkup = true, Ellipsize = EllipsizeMode.End, Xalign = 0
            };

            if (Page.IconName != null)
            {
                DisplayWidget = new Image();
                IconName = Page.IconName;
            }
            else
            {
                DisplayWidget = Page.DisplayWidget;
            }

            var overlay = new Overlay {WidthRequest = 38};
            overlay.Add(DisplayWidget);
            overlay.AddOverlay(_statusIcon);

            var grid = new Grid {Margin = 6, ColumnSpacing = 6};
            grid.Attach(overlay, 0, 0, 1, 2);
            grid.Attach(_titleLabel, 1, 0, 1, 1);
            grid.Attach(_statusLabel, 1, 1, 1, 1);

            Add(grid);

            Header = Page.Header;
            Page.AddNotification(nameof(StatusType), (o, e) => StatusType = Page.StatusType);
            Page.AddNotification(nameof(Status), (o, e) => Status = Page.Status);
            Page.AddNotification(nameof(Title), (o, e) => Title = Page.Title);
            Page.AddNotification(nameof(IconName), (o, e) => IconName = Page.IconName);

            if (Page.Status != null)
            {
                Status = Page.Status;
            }

            if (Page.StatusType != SettingsPage.SettingsStatusType.NONE)
            {
                StatusType = Page.StatusType;
            }
        }
    }
}