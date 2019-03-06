using Gtk;

namespace Granite.Widgets
{
    public class HeaderLabel : Label
    {

        /**
         * Create a new HeaderLabel
         */
        public HeaderLabel(string label): this()
        {
            Text = label;
        }

        private HeaderLabel()
        {
            Halign = Align.Start;
            Xalign = 0;
            StyleContext.AddClass(Granite.StyleClass.H4Label);
        }
    }
}