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
        BookableItemsListProjection _projection;
        CommitSequencer _sequencer;
        PollingClient2 _client;
        public ProjectionEngine(IStoreEvents eventStore, MongoDatabase readModelDb, NotifyReadModelUpdates updates)
        {
            Logger = NullLogger.Instance;
            _eventStore = eventStore;
            _projection = new BookableItemsListProjection(readModelDb, updates);
            _tracker = new CheckpointTracker(readModelDb);
        }

        private HandlingResult Handle(ICommit commit)
        {
            foreach (var @event in commit.Events)
            {
                ((dynamic) _projection).On((dynamic) @event.Body);
            }
            return HandlingResult.MoveToNext;
        }

        public void Start()
        {
            var start = _tracker.LoadCheckpoint();
            _sequencer = new CommitSequencer(Handle, 0, 5000);
            _client = new PollingClient2(_eventStore.Advanced, _sequencer.Handle, 1000);
            var dispatcher = new CommitsDispatcher(_tracker, Logger);

            _client.StartFrom(null);

            Logger.InfoFormat("Projection engine started from {0}", start);
        }

        public void Stop()
        {
            Logger.Info("Projection engine stopped");
        }
    }
}
