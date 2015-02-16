using System;
using Castle.Core.Logging;
using Castle.Facilities.Logging;
using Castle.Windsor;

namespace Bookings.ProcessManager.Support
{
    public class Boostrapper : IDisposable
    {
        private IWindsorContainer _container;
        private ILogger Logger { get; set; }
        
        public Boostrapper()
        {
            _container = new WindsorContainer();
            _container.AddFacility<LoggingFacility>(facility => facility.UseLog4Net());

            _container.Install(new EventstoreInstaller());
            _container.Install(new BusInstaller());

            Logger = _container.Resolve<ILogger>();

            Logger.DebugFormat("Started");
        }

        public void Dispose()
        {
            Logger.DebugFormat("Stopping");
            _container.Dispose();
        }

        public void Start()
        {
            Console.WriteLine("Started");
        }

        public void Stop()
        {
            Console.WriteLine("Stopped");
        }
    }
}
