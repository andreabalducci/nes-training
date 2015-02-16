using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Bookings.Domain.Messaging;
using CommonDomain.Core;
using CommonDomain.Persistence;
using CommonDomain.Persistence.EventStore;
using NEventStore;
using NEventStore.Serialization;

namespace Bookings.Domain.Support
{
    public class Bootstrapper : IBookingApplication
    {
        private readonly string _eventStoreConnectionString;
        private IStoreEvents _eventStore;
        private SimpleMessageQueue _simpleMessageQueue;

        private IStoreEvents EventStore
        {
            get { return _eventStore; }
        }

        public Bootstrapper(string eventStoreConnectionString)
        {
            _eventStoreConnectionString = eventStoreConnectionString;
        }

        public void FlushExecutionQueue()
        {
            while (_simpleMessageQueue.HasPendingMessages)
            {
                Thread.Sleep(100);
            }
        }

        public IRepository CreateRepository()
        {
            return new EventStoreRepository(
                _eventStore,
                new AggregateFactory(),
                new ConflictDetector()
            );
        }

        public IBookingApplication Start()
        {
            StartMessageQueue();
            CreateEventStore();

            return this;
        }

        public void Stop()
        {
            _eventStore.Dispose();
            _simpleMessageQueue.Dispose();
        }

        private void StartMessageQueue()
        {
            _simpleMessageQueue = new SimpleMessageQueue(10);
            var assemblies =
                AppDomain.CurrentDomain.GetAssemblies().Where(x => x.GetName().Name.StartsWith("Bookings")).ToArray();

            foreach (var assembly in assemblies)
            {
                _simpleMessageQueue.RegisterAssembly(assembly);
            }
        }

        private void CreateEventStore()
        {
            _eventStore = Wireup.Init()
                                .UsingMongoPersistence(_eventStoreConnectionString, new DocumentObjectSerializer())
                                .InitializeStorageEngine()
                                .UsingSynchronousDispatchScheduler(new CommitsDispatcher(_simpleMessageQueue))
                                .Build();
        }

        public void Accept(IMessage message)
        {
            Bus.Send(message);
        }

        private IBus Bus
        {
            get { return _simpleMessageQueue; }
        }
    }
}
