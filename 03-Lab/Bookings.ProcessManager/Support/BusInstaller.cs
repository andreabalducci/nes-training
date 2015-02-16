using System.Configuration;
using Bookings.Shared.Domain.BookingContext.BookableItem.Events;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Rebus;
using Rebus.Castle.Windsor;
using Rebus.Configuration;
using Rebus.MongoDb;
using Rebus.OldLog4Net;
using Rebus.Transports.Msmq;

namespace Bookings.ProcessManager.Support
{
    public class BusInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromThisAssembly()
                       .BasedOn(typeof (IHandleMessages<>))
                       .WithServiceAllInterfaces()
                       .LifestyleTransient()
                );

            var cstring = ConfigurationManager.ConnectionStrings["bus"].ConnectionString;

            var bus = Configure.With(new WindsorContainerAdapter(container))
                .Logging(l => l.OldLog4Net())
                .Transport(t => t.UseMsmqAndGetInputQueueNameFromAppConfig())
                .MessageOwnership(d => d.FromRebusConfigurationSection())
                .Timeouts(t => t.StoreInMongoDb(cstring, "pm-timeouts"))
                .Subscriptions(s => s.StoreInMongoDb(cstring, "pm-subscriptions"))
                .CreateBus().Start();

            bus.Subscribe<BookableItemCreated>();
        }
    }
}
