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
using CommonDomain;
using CommonDomain.Core;
using CommonDomain.Persistence;
using CommonDomain.Persistence.EventStore;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using NEventStore;
using NEventStore.Dispatcher;
using NEventStore.Serialization;
using NEventStore.Persistence.MongoDB;

namespace Bookings.Service.Support
{
    public class EventstoreInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var readModel = new MongoUrl(ConfigurationManager.ConnectionStrings["readmodel"].ConnectionString);
            MongoPersistenceOptions options = new MongoPersistenceOptions();
            options.ServerSideOptimisticLoop = true;
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
                    .For<IDispatchCommits>()
                    .ImplementedBy<CommitsDispatcher>(),

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
                            .UsingMongoPersistence("events", new DocumentObjectSerializer(), options)
                            .InitializeStorageEngine()
                            .UsingSynchronousDispatchScheduler(k.Resolve<IDispatchCommits>())
                            .Build()
                    )
                );

            MongoDBRegistration.Register();
        }
    }
}
