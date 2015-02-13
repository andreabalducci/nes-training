using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core;
using Castle.Core.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using NEventStore;
using NEventStore.Client;

namespace Bookings.Service.QueryModel
{
    public class ProjectionTracker
    {
        public string Id { get; private set; }
        public string Checkpoint { get; private set; }

        public ProjectionTracker(string id, string checkpoint)
        {
            Id = id;
            Checkpoint = checkpoint;
        }
    }

    public class CommitsDispatcher : IObserver<ICommit>
    {
        public CommitsDispatcher(ILogger logger)
        {
            Logger = logger;
        }

        public ILogger Logger { get; private set; }
        public void OnNext(ICommit value)
        {
            Logger.DebugFormat("OnNext {0}", value.CheckpointToken);
        }

        public void OnError(Exception error)
        {
            Logger.ErrorFormat(error, "Error {0}", error.Message);
        }

        public void OnCompleted()
        {
            Logger.Debug("Completed");
        }
    }

    public class ProjectionEngine : IStartable
    {
        private IStoreEvents _eventStore;
        public ILogger Logger { get; set; }
        private MongoCollection<ProjectionTracker> _checkpointCollection;
        private string EngineSlotName = "default";
        private IObserveCommits _observer;
        private IDisposable _subscription;

        public ProjectionEngine(IStoreEvents eventStore, MongoDatabase readModelDb)
        {
            Logger = NullLogger.Instance;
            _eventStore = eventStore;

            _checkpointCollection = readModelDb.GetCollection<ProjectionTracker>("system.checkpoints");
        }

        public void Start()
        {
            var start = LoadCheckpoint();
            Logger.InfoFormat("Projection engine started from {0}", start);

            var client = new PollingClient(_eventStore.Advanced, 1000);
            var dispatcher = new CommitsDispatcher(Logger);
            _observer = client.ObserveFrom(start);
            _subscription = _observer.Subscribe(dispatcher);

            _observer.Start();
        }

        public void Stop()
        {
            _subscription.Dispose();
            _observer.Dispose();
            Logger.Info("Projection engine stopped");
        }

        private string LoadCheckpoint()
        {
            var tracker = _checkpointCollection.AsQueryable().SingleOrDefault(x => x.Id == EngineSlotName);

            if (tracker != null)
                return tracker.Checkpoint;

            return null;
        }

        private void SaveCheckpoint(string checkpointToken)
        {
            _checkpointCollection.Save(new ProjectionTracker(EngineSlotName, checkpointToken));
        }
    }
}
