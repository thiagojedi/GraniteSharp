using System;
using GLib;

namespace Demo.Core
{
    internal static class Program
    {
        public static void Main(string[] args) => ((Application) new GraniteDemo()).Run();
    }
}