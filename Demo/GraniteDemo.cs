using Demo.Views;
using GLib;
using Gtk;
using Application = Gtk.Application;

namespace Demo
{
    public class GraniteDemo : Application
    {
        private GraniteDemo() : base("br.thiagoabreu.demo", ApplicationFlags.None)
        {
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            var window = new Window(WindowType.Toplevel);

            var toastView = new ToastView();
            var welcome = new WelcomeView();

            var mainStack = new Stack();
            mainStack.AddTitled(welcome, "welcome", "Welcome");
            mainStack.AddTitled(toastView, "toasts", "Toast");

            var stackSidebar = new StackSidebar {Stack = mainStack};

            var paned = new Paned(Orientation.Horizontal);
            paned.Add1(stackSidebar);
            paned.Add2(mainStack);

            var headerBar = new HeaderBar {ShowCloseButton = true};
            headerBar.StyleContext.AddClass("default-decoration");

            window.Add(paned);
            window.SetDefaultSize(900, 600);
            window.SetSizeRequest(750, 500);
            window.Titlebar = headerBar;
            window.Title = "Granite Demo";
            window.ShowAll();

            AddWindow(window);
        }

        public static int Main(string[] args) => ((GLib.Application) new GraniteDemo()).Run();
    }
}