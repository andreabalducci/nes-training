using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Bookings.Service.QueryModel
{
    public class CheckpointTracker
    {
        private class Tracker
        {
            public string Id { get; private set; }
            public string Checkpoint { get; private set; }

            public Tracker(string id, string checkpoint)
            {
                Id = id;
                Checkpoint = checkpoint;
            }
        }

        private readonly MongoCollection<Tracker> _checkpointCollection;
        private const string EngineSlotName = "default";

        public CheckpointTracker(MongoDatabase readModelDb)
        {
            _checkpointCollection = readModelDb.GetCollection<Tracker>("sys.checkpoints");
        }

        public string LoadCheckpoint()
        {
            var tracker = _checkpointCollection.AsQueryable().SingleOrDefault(x => x.Id == EngineSlotName);

            if (tracker != null)
                return tracker.Checkpoint;

            return null;
        }

        public void SaveCheckpoint(string checkpointToken)
        {
            _checkpointCollection.Save(new Tracker(EngineSlotName, checkpointToken));
        }
    }
}