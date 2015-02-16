using System.Configuration;
using System.Linq;
using Bookings.Shared;
using Bookings.Shared.Support;
using Castle.MicroKernel;
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

namespace Bookings.ProcessManager.Support
{
    public class EventstoreInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component
                    .For<IDispatchCommits>()
                    .ImplementedBy<CommitsDispatcher>(),
                
                Component
                    .For<ISagaRepository>()
                    .ImplementedBy<SagaEventStoreRepository>()
                    .LifestyleTransient(),

                Component.For<IStoreEvents>()
                         .UsingFactoryMethod(k =>
                                             Wireup.Init()
                                                   .UsingMongoPersistence("pm", new DocumentObjectSerializer())
                                                   .InitializeStorageEngine()
                                                    .UsingSynchronousDispatchScheduler(k.Resolve<IDispatchCommits>())
//                                                   .UsingAsynchronousDispatchScheduler(k.Resolve<IDispatchCommits>())
                                                   .Build()
                    )
                );
            MongoDBRegistration.Register();
        }
    }
}
