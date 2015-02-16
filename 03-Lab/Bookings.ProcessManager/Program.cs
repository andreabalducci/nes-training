using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookings.ProcessManager.Support;
using Topshelf;

namespace Bookings.ProcessManager
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
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

                x.SetDescription("Bookings process manager service");
                x.SetDisplayName("Bookings process manager service");
                x.SetServiceName("BookingsPM");
            });
        }
    }
}
