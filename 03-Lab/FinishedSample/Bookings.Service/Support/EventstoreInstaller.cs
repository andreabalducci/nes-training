using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Bookings.Shared;
using Bookings.Shared.Messaging;
using Bookings.Shared.Support;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CommonDomain;
using CommonDomain.Core;
using CommonDomain.Persistence;
using CommonDomain.Persistence.EventStore;
using Fasterflect;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using NEventStore;
using NEventStore.Dispatcher;
using NEventStore.Serialization;
using Rebus;

namespace Bookings.Service.Support
{
    public class CommitsDispatcher : IDispatchCommits
    {
        private IKernel _kernel;
        private IBus _bus;
        public CommitsDispatcher(IKernel kernel, IBus bus)
        {
            _kernel = kernel;
            _bus = bus;
        }

        public void Dispose()
        {
        }

        public void Dispatch(ICommit commit)
        {
            // in process
            foreach (var eventMessage in commit.Events)
            {
                var message  =eventMessage.Body;
                var handlerType = typeof (IEventHandler<>).MakeGenericType(new[] {message.GetType()});
                var handlers = _kernel.ResolveAll(handlerType); // singleton handlers
                foreach (var handler in handlers)
                {
                    ((dynamic) handler).On((dynamic) message);
                }
            }

            // publish on bus
            foreach (var eventMessage in commit.Events)
            {
                var message = eventMessage.Body;
                _bus.Publish(message);
            }
        }
    }

    public class MissingDefaultCtorException : Exception
    {
        public MissingDefaultCtorException(Type t)
            : base(string.Format("Type {0} has no default constructor. Add protected {1}(){{}}", t.FullName, t.Name))
        {
        }
    }
    public class AggregateFactory : IConstructAggregates
    {
        public IAggregate Build(Type type, Guid id, IMemento snapshot)
        {
            var ctor = type.Constructor(Flags.Default, new Type[] { });

            if (ctor == null)
                throw new MissingDefaultCtorException(type);

            var aggregate = (IAggregate)ctor.CreateInstance();

            /*
            if (snapshot != null && aggregate is ISnapshotable)
            {
                ((ISnapshotable)aggregate).Restore(snapshot);
            }
            */

            return aggregate;
        }
    }

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

                Component.For<IStoreEvents>()
                         .UsingFactoryMethod(k =>
                                             Wireup.Init()
                                                   .UsingMongoPersistence("events", new DocumentObjectSerializer())
                                                   .InitializeStorageEngine()
                                                   .UsingAsynchronousDispatchScheduler(k.Resolve<IDispatchCommits>())
                                                   .Build()
                    )
                );

            MongoDBRegistration.Register();
        }
    }
}
