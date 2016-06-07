using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Bookings.Service.QueryModel;
using Bookings.Shared;
using Bookings.Shared.Messaging;
using Bookings.Shared.Support;
using Castle.Facilities.Startable;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using NEventStore.Domain;
using NEventStore.Domain.Core;
using NEventStore.Domain.Persistence;
using NEventStore.Domain.Persistence.EventStore;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using NEventStore;

using NEventStore.Serialization;

namespace Bookings.Service.Support
{
    public class EventstoreInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var readModel = new MongoUrl(ConfigurationManager.ConnectionStrings["readmodel"].ConnectionString);

            container.Register(
                Classes
                    .FromAssemblyInThisApplication()
                    .BasedOn(typeof(IEventHandler<>))
                    .WithServiceAllInterfaces()
                    .LifestyleTransient(),
                
                Component
                    .For<NotifyReadModelUpdates>(),

                Component
                    .For<MongoDatabase>()
                    .UsingFactoryMethod( k => new MongoClient(readModel).GetServer().GetDatabase(readModel.DatabaseName)),


                Component
                    .For<IRepository>()
                    .ImplementedBy<EventStoreRepository>()
                    .LifeStyle.Transient,

                Component
                    .For<IConstructAggregates>()
                    .ImplementedBy<AggregateFactory>(),

                Component
                    .For<IDetectConflicts>()
                    .ImplementedBy<ConflictDetector>()
                    .LifestyleTransient(),

                Component
                    .For<ProjectionEngine>()
                    .Start(),

                Component.For<IStoreEvents>().UsingFactoryMethod(k =>
                        Wireup.Init()
                            .UsingMongoPersistence("events", new DocumentObjectSerializer())
                            .InitializeStorageEngine()
            
                            .Build()
                    )
                );

            MongoDBRegistration.Register();
        }
    }
}
