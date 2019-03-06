using Gtk;

namespace Granite.Widgets
{
    public abstract class SettingsPage : ScrolledWindow
    {
        protected string _iconName;
        protected string _title;

        /**
     * Used to display a status icon overlayed on the display_widget in a Granite.SettingsSidebar
     */
        public enum SettingsStatusType {
            ERROR,
            OFFLINE,
            SUCCESS,
            WARNING,
            NONE
        }
        
        /**
     * Selects a colored icon to be displayed in a Granite.SettingsSidebar
     */
        public SettingsStatusType StatusType { get; set; } = SettingsStatusType.NONE;

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
        public string Status { get; protected set; }

        /**
         * An icon name to be displayed in a Granite.SettingsSidebar
         */
        public string IconName {
            get => _iconName;
            protected set => _iconName = value;
        }

        /**
         * A title to be displayed in a Granite.SettingsSidebar
         */
        public string Title {
            get => _title;
            protected set => _title = value;
        }
    }
}