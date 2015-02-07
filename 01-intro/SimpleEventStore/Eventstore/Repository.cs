using System;
using System.IO;
using Newtonsoft.Json;

namespace SimpleEventStore.Eventstore
{
    public class Repository
    {
        private string _storage;

        private Action<object> Dispatcher = (evt) =>{};

        public Repository(string evenstStoreFolder = null, Action<object> eventsDispatcher = null)
        {
            _storage = evenstStoreFolder ??  Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "evenstore");
            if (!Directory.Exists(_storage))
                Directory.CreateDirectory(_storage);

            if(eventsDispatcher != null)
                Dispatcher = eventsDispatcher;
        }

        public TAggregate GetById<TAggregate>(Guid id) where TAggregate : AggregateBase, new()
        {
            var json = File.ReadAllText(GetFileNameOfAggregateStream(id));
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

            File.WriteAllText(GetFileNameOfAggregateStream(aggregate.Id), json);

            Dispatch(stream);
        }

        public string GetFileNameOfAggregateStream(Guid aggregateId)
        {
            return Path.Combine(_storage, aggregateId.ToString() + ".json");
        }

        private void Dispatch(EventStream stream)
        {
            for (int i = stream.LastDispatchedEventIdx; i < stream.Version; i++)
            {
                Dispatcher(stream.Events[i]);
            }
        }
    }
}