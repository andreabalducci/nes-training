using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Castle.Core;
using Castle.Core.Logging;
using MongoDB.Driver;
using NEventStore;
using NEventStore.Client;
using static NEventStore.Client.PollingClient2;
using Bookings.Shared.Projections;
using Bookings.Shared.Messaging;

namespace Bookings.Service.QueryModel
{
    public class ProjectionEngine : IStartable
    {
        private IStoreEvents _eventStore;
        public ILogger Logger { get; set; }

        private IDisposable _subscription;
        private CheckpointTracker _tracker;
        BookableItemsListProjection projection;
        public ProjectionEngine(IStoreEvents eventStore, MongoDatabase readModelDb, NotifyReadModelUpdates updates)
        {
            Logger = NullLogger.Instance;
            _eventStore = eventStore;
            projection = new BookableItemsListProjection(readModelDb, updates);
            _tracker = new CheckpointTracker(readModelDb);
        }

        private HandlingResult Handle(ICommit commit)
        {
            foreach (var @event in commit.Events)
            {
                ((dynamic) projection).On((dynamic) @event.Body);
            }
            return HandlingResult.MoveToNext;
        }

        public void Start()
        {
            var start = _tracker.LoadCheckpoint();

            var client = new PollingClient2(_eventStore.Advanced, Handle, 1000);
            var dispatcher = new CommitsDispatcher(_tracker, Logger);

            client.StartFrom(null);

            Logger.InfoFormat("Projection engine started from {0}", start);
        }

        public void Stop()
        {
            _subscription.Dispose();
            Logger.Info("Projection engine stopped");
        }
    }
}
