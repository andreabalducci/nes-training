using System.Configuration;
using System.Linq;
using Bookings.Shared;
using Bookings.Shared.Support;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using NEventStore;

using NEventStore.Serialization;
using NEventStore.Domain.Persistence.EventStore;
using NEventStore.Domain.Persistence;

namespace Bookings.ProcessManager.Support
{
    public class EventstoreInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                
                Component
                    .For<ISagaRepository>()
                    .ImplementedBy<SagaEventStoreRepository>()
                    .LifestyleTransient(),

                Component.For<IStoreEvents>()
                         .UsingFactoryMethod(k =>
                                             Wireup.Init()
                                                   .UsingMongoPersistence("pm", new DocumentObjectSerializer())
                                                   .InitializeStorageEngine()
//                                                   .UsingAsynchronousDispatchScheduler(k.Resolve<IDispatchCommits>())
                                                   .Build()
                    )
                );
            MongoDBRegistration.Register();
        }
    }
}
