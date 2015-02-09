using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookings.Client.Client;
using Castle.Core.Logging;
using Castle.Facilities.Logging;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace Bookings.Client.Support
{
    public class Boostrapper : IDisposable
    {
        private IWindsorContainer _container;
        private ILogger Logger { get; set; }
        
        public Boostrapper()
        {
            _container = new WindsorContainer();
            _container.AddFacility<LoggingFacility>(facility => facility.UseLog4Net());

            _container.Install(new ClientInstaller());
            _container.Install(new BusInstaller());

            Logger = _container.Resolve<ILogger>();

            Logger.DebugFormat("Started");
        }

        public void Dispose()
        {
            Logger.DebugFormat("Stopping");
            _container.Dispose();
        }

        public void Run()
        {
            _container.Resolve<IClient>().Run();
        }
    }
}
