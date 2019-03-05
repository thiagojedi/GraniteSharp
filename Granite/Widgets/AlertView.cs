using System;
using Gtk;
using WrapMode = Pango.WrapMode;

namespace Granite.Widgets
{
    public class AlertView : Grid
    {
        public event EventHandler ActionActivated;

        /**
         * The first line of text, should be short and not contain markup.
         */
        public string Title
        {
            get => _titleLabel.Text;
            set => _titleLabel.Text = value;
        }

        /**
         * The second line of text, explaining why this alert is shown.
         * You may need to escape it with #escape_text or #printf_escaped
         */
        public string Description
        {
            get => _descriptionLabel.LabelProp;
            set => _descriptionLabel.LabelProp = value;
        }

        /**
         * The icon name
         */
        public string IconName
        {
            get => _image.IconName ?? "";
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _image.SetFromIconName(value, IconSize.Dialog);
                    _image.NoShowAll = false;
                    _image.Show();
                }
                else
                {
                    _image.NoShowAll = true;
                    _image.Hide();
                }
            }
        }

        private readonly Label _titleLabel;
        private readonly Label _descriptionLabel;
        private readonly Image _image;
        private readonly Button _actionButton;
        private readonly Revealer _actionRevealer;

        /**
         * Makes new AlertView
         *
         * @param title the first line of text
         * @param description the second line of text
         * @param icon_name the icon to be shown
         */
        public AlertView(string title, string description, string iconName)
            : this()
        {
            Title = title;
            Description = description;
            IconName = iconName;
        }

        private AlertView()
        {
            //            StyleContext.AddClass(Gtk.STYLE_CLASS_VIEW);
            _titleLabel = new Label
            {
                Hexpand = true,
                MaxWidthChars = 75,
                Wrap = true,
                LineWrapMode = WrapMode.WordChar,
                Xalign = 0
            };
            _titleLabel.StyleContext.AddClass(StyleClass.H2Label);

            _descriptionLabel = new Label
            {
                Hexpand = true,
                MaxWidthChars = 75,
                Wrap = true,
                Xalign = 0,
                Valign = Align.Start
            };
            _descriptionLabel.UseMarkup = true;

            _actionButton = new Button {MarginTop = 24};

            _actionRevealer = new Revealer
            {
                Halign = Align.End, TransitionType = RevealerTransitionType.SlideUp
            };
            _actionRevealer.Add(_actionButton);

            _image = new Image {MarginTop = 6, Valign = Align.Start};

            var layout = new Grid
            {
                ColumnSpacing = 12,
                RowSpacing = 6,
                Halign = Align.Center,
                Valign = Align.Center,
                Vexpand = true,
                Margin = 24
            };

            layout.Attach(_image, 1, 1, 1, 2);
            layout.Attach(_titleLabel, 2, 1, 1, 1);
            layout.Attach(_descriptionLabel, 2, 2, 1, 1);
            layout.Attach(_actionRevealer, 2, 3, 1, 1);

            Add(layout);

            _actionButton.Clicked += (o, e) => ActionActivated?.Invoke(o, e);
        }

        /**
         * Creates the action button with the given label
         *
         * @param label the text of the action button
         */
        public void ShowAction(string label = null)
        {
            if (label != null)
                _actionButton.Label = label;

            if (_actionButton.Label == null)
                return;

            _actionRevealer.RevealChild = true;
            _actionRevealer.ShowAll();
        }

        /**
         * Hides the action button.
         */
        public void HideAction()
        {
            _actionRevealer.RevealChild = false;
        }
    }
}