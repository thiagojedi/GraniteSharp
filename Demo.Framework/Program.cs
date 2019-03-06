using Demo.Views;
using Demo.Views.SettingsView;
using GLib;
using Gtk;
using Application = GLib.Application;

namespace Demo.Framework
{
    internal static class Program
    {
        private static int Main(string[] args) => ((Application) new GraniteDemo()).Run();
    }
}
