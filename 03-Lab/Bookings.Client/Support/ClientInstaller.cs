using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Bookings.Client.Client;
using Bookings.Shared.Messaging;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using MongoDB.Driver;
using Rebus;

namespace Bookings.Client.Support
{
    public class ClientInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component
                    .For<MongoDatabase>()
                    .UsingFactoryMethod(k =>
                        {
                            var url = new MongoUrl(ConfigurationManager.ConnectionStrings["readmodel"].ConnectionString);
                            return new MongoClient().GetServer().GetDatabase(url.DatabaseName);
                        }),

                Component
                    .For<IReadModel>()
                    .ImplementedBy<ReadModel>(),

                Component
                    .For<IClient, IHandleMessages<ReadModelUpdatedMessage>>()
                    .ImplementedBy<BookingsClient>()
            );
        }
    }
}
