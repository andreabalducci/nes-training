using System;
using System.IO;
using Newtonsoft.Json;

namespace SimpleEventStore.Eventstore
{
    public class Repository
    {
        private readonly string _storage;

        private readonly Action<object> _dispatcher = (evt) =>{};

        public Repository(string evenstStoreFolder = null, Action<object> eventsDispatcher = null)
        {
            _storage = evenstStoreFolder ??  Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "evenstore");
            if (!Directory.Exists(_storage))
                Directory.CreateDirectory(_storage);

            if(eventsDispatcher != null)
                _dispatcher = eventsDispatcher;
        }

        public TAggregate GetById<TAggregate>(string id) where TAggregate : AggregateBase, new()
        {
            var json = File.ReadAllText(MakeAggregateStreamFileName(id));
            var stream = (EventStream) JsonConvert.DeserializeObject(json, new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All
                });
            return AggregateBase.Load<TAggregate>(stream);
        }

        public void Save(AggregateBase aggregate)
        {
            var stream = new EventStream();
            aggregate.Save(stream);
            var json = JsonConvert.SerializeObject(stream, new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All,
                    Formatting = Formatting.Indented
                });

            File.WriteAllText(MakeAggregateStreamFileName(aggregate.Id), json);

            Dispatch(stream);
        }

        public string MakeAggregateStreamFileName(string aggregateId)
        {
            return Path.Combine(_storage, aggregateId + ".json");
        }

        private void Dispatch(EventStream stream)
        {
            for (int i = stream.LastDispatchedEventIdx; i < stream.Version; i++)
            {
                _dispatcher(stream.Events[i]);
            }
        }
    }
}