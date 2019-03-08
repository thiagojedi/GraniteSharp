using Gtk;

namespace Granite.Widgets
{
    public abstract class SettingsPage : ScrolledWindow
    {
        private string _iconName;
        private string _title;
        private SettingsStatusType _statusType;
        private string _status;

        /**
     * Used to display a status icon overlayed on the display_widget in a Granite.SettingsSidebar
     */
        public enum SettingsStatusType
        {
            ERROR,
            OFFLINE,
            SUCCESS,
            WARNING,
            NONE
        }

        /**
     * Selects a colored icon to be displayed in a Granite.SettingsSidebar
     */
        [GLib.Property(nameof(StatusType))]
        public SettingsStatusType StatusType
        {
            get => _statusType;
            set
            {
                _statusType = value;
                Notify(nameof(StatusType));
            }
        }

        /**
         * A widget to display in place of an icon in a Granite.SettingsSidebar
         */
        public Widget DisplayWidget { get; protected set; }

        /**
         * A header to be sorted under in a Granite.SettingsSidebar
         */
        public string Header { get; protected set; }

        /**
         * A status string to be displayed underneath the title in a Granite.SettingsSidebar
         */
        [GLib.Property(nameof(Status))]
        public string Status
        {
            get => _status;
            protected set
            {
                _status = value;
                Notify(nameof(Status));
            }
        }

        /**
         * An icon name to be displayed in a Granite.SettingsSidebar
         */
        [GLib.Property(nameof(IconName))]
        public string IconName
        {
            get => _iconName;
            set
            {
                _iconName = value;
                Notify(nameof(IconName));
            }
        }

        /**
         * A title to be displayed in a Granite.SettingsSidebar
         */
        [GLib.Property(nameof(Title))]
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                Notify(nameof(Title));
            }
        }
    }
}