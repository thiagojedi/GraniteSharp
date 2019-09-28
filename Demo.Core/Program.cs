using System;
using GLib;

namespace Demo.Core
{
    internal static class Program
    {
        public static void Main(string[] args) => (new GraniteDemo() as Application).Run();
    }
}