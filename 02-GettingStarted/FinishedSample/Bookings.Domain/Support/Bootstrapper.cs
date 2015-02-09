using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Bookings.Domain.Messaging;
using NEventStore;
using NEventStore.Serialization;

namespace Bookings.Domain.Support
{
	public class Bootstrapper
	{
		private readonly string _es_connectionString;
		private IStoreEvents _eventstore;
		private SimpleMessageQueue _simpleMessageQueue;

		public IStoreEvents EventStore
		{
			get { return _eventstore; }
		}

		public Bootstrapper(string esConnectionString)
		{
			_es_connectionString = esConnectionString;
		}

		public bool HasPendingMessages()
		{
			return _simpleMessageQueue.HasPendingMessages();
		}

		public void Start()
		{
			StartMessageQueue();
			StartEventStore();
		}

		public void Stop()
		{
			_eventstore.Dispose();
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

		private void StartEventStore()
		{
			_eventstore = Wireup.Init()
								.UsingMongoPersistence(_es_connectionString, new DocumentObjectSerializer())
								.InitializeStorageEngine()
								.UsingSynchronousDispatchScheduler(new CommitsDispatcher(_simpleMessageQueue))
								.Build();
		}

		public IBus Bus {
			get { return _simpleMessageQueue; }
		}
	}
}
