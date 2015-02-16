using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Castle.Core;
using Castle.Core.Logging;
using MongoDB.Driver;
using NEventStore;
using NEventStore.Client;

namespace Bookings.Service.QueryModel
{
    public class ProjectionEngine : IStartable
    {
        private IStoreEvents _eventStore;
        public ILogger Logger { get; set; }
        private IObserveCommits _observer;
        private IDisposable _subscription;
        private CheckpointTracker _tracker;
        public ProjectionEngine(IStoreEvents eventStore, MongoDatabase readModelDb)
        {
            Logger = NullLogger.Instance;
            _eventStore = eventStore;
            _tracker = new CheckpointTracker(readModelDb);
        }

        public void Start()
        {
            var start = _tracker.LoadCheckpoint();

            var client = new PollingClient(_eventStore.Advanced, 1000);
            var dispatcher = new CommitsDispatcher(_tracker, Logger);
            _observer = client.ObserveFrom(start);
            _subscription = _observer.Subscribe(dispatcher);

            _observer.Start();

            Logger.InfoFormat("Projection engine started from {0}", start);
        }

        public void Stop()
        {
            _subscription.Dispose();
            _observer.Dispose();
            Logger.Info("Projection engine stopped");
        }
    }
}
