using System;
using System.Collections.Generic;
using Gdk;
using Gtk;
using WrapMode = Pango.WrapMode;

namespace Granite.Widgets
{
    public class Welcome : EventBox
    {
        private readonly Label _subtitleLabel;

        private readonly Label _titleLabel;

        /**
         * List of buttons for action items
         */
        protected new readonly List<Button> Children = new List<Button>();

        /**
         * Grid for action items
         */
        protected readonly Grid Options;

        /**
         * Makes new Welcome Page
         *
         * @param title_text main title for new Welcome Page
         * @param subtitle_text subtitle text for new Welcome Page
         */
        public Welcome()
        {
//        get_style_context ().add_class (Gtk.STYLE_CLASS_VIEW);
//        get_style_context ().add_class (Granite.STYLE_CLASS_WELCOME);

            _titleLabel = new Label(null)
            {
                Justify = Justification.Center,
                Hexpand = true
            };
            //        title_label.get_style_context ().add_class (Granite.STYLE_CLASS_H1_LABEL);

            _subtitleLabel = new Label(null)
            {
                Justify = Justification.Center, Hexpand = true, Wrap = true, LineWrapMode = WrapMode.Word
            };

//        var subtitle_label_context = subtitle_label.get_style_context ();
//        subtitle_label_context.add_class (Gtk.STYLE_CLASS_DIM_LABEL);
//        subtitle_label_context.add_class (Granite.STYLE_CLASS_H2_LABEL);

            Options = new Grid
            {
                Orientation = Orientation.Vertical,
                RowSpacing = 12,
                Halign = Align.Center,
                MarginTop = 24
            };

            var content = new Grid
            {
                Expand = true,
                Margin = 12,
                Orientation = Orientation.Vertical,
                Valign = Align.Center
            };
            content.Add(_titleLabel);
            content.Add(_subtitleLabel);
            content.Add(Options);

            Add(content);
        }

        /**
         * This is the title of the welcome widget. It should use Title Case.
         */
        public string Title
        {
            get => _titleLabel.Text;
            set => _titleLabel.Text = value;
        }

        /**
         * This is the subtitle of the welcome widget. It should use sentence case.
         */
        public string Subtitle
        {
            get => _subtitleLabel.Text;
            set => _subtitleLabel.Text = value;
        }

        public event EventHandler<int> Activated;

        /**
         * Sets action item of given index's visibility
         *
         * @param index index of action item to be changed
         * @param val value determining whether the action item is visible
         */
        public void SetItemVisible(int index, bool val)
        {
            if (index >= Children.Count || Children[index] == null) return;

            Children[index].NoShowAll = !val;
            Children[index].Visible = val;
        }

        /**
         * Removes action item of given index
         *
         * @param index index of action item to remove
         */
        public void RemoveItem(int index)
        {
            if (index >= Children.Count || Children[index] == null) return;

            var item = Children[index];
            item.Destroy();
            Children.Remove(item);
        }

        /**
         * Sets action item of given index sensitivity
         *
         * @param index index of action item to be changed
         * @param val value deteriming whether the action item is senstitive
         */
        public void SetItemSensitivity(int index, bool val)
        {
            if (index < Children.Count && Children[index] != null)
                Children[index].Sensitive = val;
        }

        /**
         * Appends new action item to welcome page with a {@link Gtk.Image.from_icon_name}
         *
         * @param icon_name named icon to be set as icon for action item
         * @param option_text text to be set as the title for action item. It should use Title Case.
         * @param description_text text to be set as description for action item. It should use sentence case.
         * @return index of new item
         */
        public int Append(string iconName, string optionText, string descriptionText)
        {
            var image = new Image(iconName, IconSize.Dialog);
            image.UseFallback = true;
            return Append(image, optionText, descriptionText);
        }

        /**
         * Appends new action item to welcome page with a {link Gdk.Pixbuf} icon
         *
         * @param pixbuf pixbuf to be set as icon for action item
         * @param option_text text to be set as the header for action item
         * @param description_text text to be set as description for action item
         * @return index of new item
         */
        public int Append(Pixbuf pixbuf, string optionText, string descriptionText)
        {
            var image = new Image(pixbuf);
            return Append(image, optionText, descriptionText);
        }

        /**
         * Appends new action item to welcome page with a {@link Gtk.Image} icon
         *
         * @param image image to be set as icon for action item
         * @param option_text text to be set as the header for action item
         * @param description_text text to be set as description for action item
         * @return index of new item
         */
        public int Append(Image image, string optionText, string descriptionText)
        {
            // Option label
            var button = new WelcomeButton
            {
                Title = optionText,
                Description = descriptionText,
                Icon = image
            };
            Children.Add(button);
            Options.Add(button);

            button.Clicked += (o, e) =>
            {
                var index = Children.IndexOf(button);
                Activated?.Invoke(this, index); // send signal
            };

            return Children.IndexOf(button);
        }

        /**
         * Returns a welcome button by index
         *
         * @param index index of action item to be returned
         * @return welcome button at //index//, or //null// if //index// is invalid.
         * @since 0.3
         */
        public WelcomeButton get_button_from_index(int index)
        {
            if (index >= 0 && index < Children.Count)
                return Children[index] as WelcomeButton;

            return null;
        }
    }
}