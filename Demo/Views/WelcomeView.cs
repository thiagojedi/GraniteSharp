using System;
using GLib;
using Granite.Widgets;
using Gtk;

namespace Demo.Views
{
    public class WelcomeView : Grid
    {
        public WelcomeView()
        {
            var welcome = new Welcome
            {
                Title = "Granite Demo",
                Subtitle = "This is a demo of the Granite library."
            };

            welcome.Append(
                "text-x-vala",
                "Visit Valadoc",
                "The canonical source for Vala API references."
            );
            welcome.Append(
                "text-x-vala",
                "Get Granite Source",
                "Granite's source code is hosted on GitHub."
            );

            Add(welcome);

            welcome.Activated += (sender, i) =>
            {
                switch (i)
                {
                    case 0:
                        try
                        {
                            AppInfoAdapter.LaunchDefaultForUri("https://valadoc.org/granite/Granite.html", null);
                        }
                        catch (Exception e)
                        {
                            Console.Error.WriteLine(e.Message);
                        }

                        break;
                    case 1:
                        try
                        {
                            AppInfoAdapter.LaunchDefaultForUri("https://github.com/elementary/granite", null);
                        }
                        catch (Exception e)
                        {
                            Console.Error.WriteLine(e.Message);
                        }

                        break;
                    default:
                        throw new Exception("Should not call this");
                }
            };
        }
    }
}