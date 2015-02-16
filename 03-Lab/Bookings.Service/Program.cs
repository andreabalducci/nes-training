using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookings.Service.Support;
using Topshelf;

namespace Bookings.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.Clear();
            }

            HostFactory.Run(x =>
            {
                x.UseOldLog4Net("log4net.config");
                x.Service<Boostrapper>(s =>
                {
                    s.ConstructUsing(name => new Boostrapper());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();
                x.DependsOnMsmq();

                x.SetDescription("Bookings service");
                x.SetDisplayName("Bookings service");
                x.SetServiceName("Bookings");
            });
        }
    }
}
