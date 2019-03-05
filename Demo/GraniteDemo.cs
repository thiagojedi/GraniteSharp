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

            var toastView = new ToastView();
            var welcome = new WelcomeView();
            var alertView = new AlertViewView();

            var mainStack = new Stack();
            mainStack.AddTitled(welcome, nameof(welcome), "Welcome");
            mainStack.AddTitled(alertView, nameof(alertView), "AlertView");
            mainStack.AddTitled(toastView, nameof(toastView), "Toast");

            var stackSidebar = new StackSidebar {Stack = mainStack};

            var paned = new Paned(Orientation.Horizontal);
            paned.Add1(stackSidebar);
            paned.Add2(mainStack);

            var headerBar = new HeaderBar {ShowCloseButton = true};
            headerBar.StyleContext.AddClass("default-decoration");

            var window = new Window(WindowType.Toplevel) {Title = "Granite Demo", Titlebar = headerBar};
            window.Add(paned);
            window.SetDefaultSize(900, 600);
            window.SetSizeRequest(750, 500);
            window.ShowAll();

            AddWindow(window);
        }

        public static int Main(string[] args) => ((GLib.Application) new GraniteDemo()).Run();
    }
}