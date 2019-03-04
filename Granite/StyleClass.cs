namespace Granite
{
    public static class StyleClass
    {
        /**
         * Style class to give accent color to a {@link Gtk.Label} or symbolic icon
         */
        public const string Accent = "accent";

        public const string Avatar = "avatar";

        /**
         * Style class for shaping a {@link Gtk.Button}
         */
        public const string BackButton = "back-button";

        /**
         * Style class for numbered badges as in a {@link Granite.Widgets.SourceList}
         */
        public const string Badge = "badge";

        /**
         * Style class for adding a small shadow to a container such as for image thumbnails
         *
         * Can be combined with the style class ".collapsed" to further reduce the size of the shadow
         */
        public const string Card = "card";

        public const string CategoryExpander = "category-expander";

        /**
         * Style class for checkered backgrounds to represent transparency in images
         */
        public const string Checkerboard = "checkerboard";

        /**
         * Style class for large primary text as seen in {@link Granite.Widgets.Welcome}
         */
        public const string H1Label = "h1";

        /**
         * Style class for large seondary text as seen in {@link Granite.Widgets.Welcome}
         */
        public const string H2Label = "h2";

        /**
         * Style class for small primary text
         */
        public const string H3Label = "h3";

        /**
         * Style class for a {@link Granite.HeaderLabel}
         */
        public const string H4Label = "h4";

        /**
         * Style class for a {@link Gtk.Switch} used to change between two modes rather than active and inactive states
         */
        public const string ModeSwitch = "mode-switch";

        /**
         * Style class for a {@link Granite.Widgets.OverlayBar}
         */
        public const string OverlayBar = "overlay-bar";

        /**
         * Style class for primary label text in a {@link Granite.MessageDialog}
         */
        public const string PrimaryLabel = "primary";

        /**
         * Style class for a {@link Granite.SeekBar}
         */
        public const string Seekbar = "seek-bar";

        /**
         * Style class for a {@link Granite.Widgets.SourceList}
         */
        public const string SourceList = "source-list";

        /**
         * Style class for a {@link Granite.Widgets.Granite.Widgets.StorageBar}
         */
        public const string Storagebar = "storage-bar";

        /**
         * Style class for {@link Gtk.Label} or {@link Gtk.TextView} to emulate the appearance of Terminal. This includes
         * text color, background color, selection highlighting, and selecting the system monospace font.
         *
         * When used with {@link Gtk.Label} this style includes internal padding. When used with {@link Gtk.TextView}
         * interal padding will need to be set with {@link Gtk.Container.border_width}
         */
        public const string Terminal = "terminal";

        /**
         * Style class for a {@link Granite.Widgets.Welcome}
         */
        public const string Welcome = "welcome";
    }
}