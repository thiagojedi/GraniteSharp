using Demo.Views;
using Demo.Views.SettingsView;
using GLib;
using Gtk;
using Application = Gtk.Application;

namespace Demo;

public class GraniteDemo() : Application("br.thiagoabreu.demo", ApplicationFlags.None)
{
    protected override void OnActivated()
    {
        base.OnActivated();

        var toastView = new ToastView();
        var welcome = new WelcomeView();
        var alertView = new AlertViewView();
        var settingsView = new SettingsView();
        var hypertextView = new HyperTextViewGrid();
        var dateTimeView = new DateTimePickerView();
            
            
        var mainStack = new Stack();
        mainStack.AddTitled(welcome, nameof(welcome), "Welcome");
        mainStack.AddTitled(alertView, nameof(alertView), "AlertView");
        mainStack.AddTitled(dateTimeView, nameof(dateTimeView), "Date & Time");
        mainStack.AddTitled(hypertextView, nameof(hypertextView), "HyperTextView");
        mainStack.AddTitled(settingsView, nameof(settingsView), "SettingsSidebar");
        mainStack.AddTitled(toastView, nameof(toastView), "Toast");
            

        var stackSidebar = new StackSidebar {Stack = mainStack};

        var paned = new Paned(Orientation.Horizontal);
        paned.Add1(stackSidebar);
        paned.Add2(mainStack);

        const string title = "Granite Demo";
        var headerBar = new HeaderBar {ShowCloseButton = true, Title = title};
        headerBar.StyleContext.AddClass("default-decoration");

        var window = new Window(WindowType.Toplevel) {Titlebar = headerBar, Title = title};
        window.Add(paned);
        window.SetDefaultSize(900, 600);
        window.SetSizeRequest(750, 500);
        window.ShowAll();

        AddWindow(window);
    }

    public static int Main(string[] args) => ((GLib.Application) new GraniteDemo()).Run();
}