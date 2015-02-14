using System;
using Castle.Core.Logging;
using NEventStore;

namespace Bookings.Service.QueryModel
{
    public class CommitsDispatcher : IObserver<ICommit>
    {
        private readonly CheckpointTracker _tracker;

        public CommitsDispatcher(CheckpointTracker tracker, ILogger logger)
        {
            _tracker = tracker;
            Logger = logger;
        }

        private ILogger Logger { get; set; }
        
        public void OnNext(ICommit value)
        {
            Logger.DebugFormat("OnNext {0}", value.CheckpointToken);
            _tracker.SaveCheckpoint(value.CheckpointToken);
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
}