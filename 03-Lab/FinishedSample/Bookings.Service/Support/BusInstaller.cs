using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Rebus;
using Rebus.Castle.Windsor;
using Rebus.Configuration;
using Rebus.MongoDb;
using Rebus.OldLog4Net;
using Rebus.Transports.Msmq;

namespace Bookings.Service.Support
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

            Configure.With(new WindsorContainerAdapter(container))
                .Logging(l => l.OldLog4Net())
                .Transport(t => t.UseMsmqAndGetInputQueueNameFromAppConfig())
                .MessageOwnership(d => d.FromRebusConfigurationSection())
                .Timeouts(t => t.StoreInMongoDb(cstring, "service-timeouts"))
                .Subscriptions(s => s.StoreInMongoDb(cstring, "service-subscriptions"))
                .CreateBus().Start();
        }
    }
}
