using Gtk;
using WrapMode = Pango.WrapMode;

namespace Granite.Widgets
{
    public class WelcomeButton : Button
    {
        private readonly Label _buttonTitle;
        private readonly Label _buttonDescription;
        private Image _icon;
        private readonly Grid _buttonGrid;

        /**
         * Title property of the Welcome Button
         */
        public string Title
        {
            get => _buttonTitle.Text;
            set => _buttonTitle.Text = value;
        }

        /**
         * Description property of the Welcome Button
         */
        public string Description
        {
            get => _buttonDescription.Text;
            set => _buttonDescription.Text = value;
        }

        /**
         * Image of the Welcome Button
         */
        public Image Icon
        {
            get => _icon;
            set
            {
                _icon?.Destroy();

                _icon = value;
                if (_icon == null) return;

                _icon.PixelSize = 48;
                _icon.Halign = Align.Center;
                _icon.Valign = Align.Center;;
                _buttonGrid.Attach(_icon, 0, 0, 1, 2);
            }
        }

        public WelcomeButton()
        {
            // Title label
            _buttonTitle = new Label {Halign = Align.Start, Valign = Align.End};
            _buttonTitle.StyleContext.AddClass(StyleClass.H3Label);

            // Description label
            _buttonDescription = new Label
            {
                Halign = Align.Start, Valign = Align.Start, LineWrap = true, LineWrapMode = WrapMode.Word
            };
            _buttonDescription.StyleContext.AddClass("dim-label");

            // Button contents wrapper
            _buttonGrid = new Grid {ColumnSpacing = 12};

            _buttonGrid.Attach(_buttonTitle, 1, 0, 1, 1);
            _buttonGrid.Attach(_buttonDescription, 1, 1, 1, 1);
            Add(_buttonGrid);
            
            StyleContext.AddClass("flat");
        }
    }
}