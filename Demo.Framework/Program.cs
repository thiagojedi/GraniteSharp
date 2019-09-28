using Application = GLib.Application;

namespace Demo.Framework
{
    internal static class Program
    {
        private static int Main(string[] args) => (new GraniteDemo() as Application).Run();
    }
}
