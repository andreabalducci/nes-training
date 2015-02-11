using System;
using System.Configuration;
using Bookings.Domain.BookingContenxt.Events;
using Bookings.Domain.Messaging;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Bookings.Domain.Query
{
    public class ListaDelleRisorsePrenotabili :
        IMessageHandler<ResourceCreated>,
        IMessageHandler<ResourceHasBeenSetAvailable>,
        IMessageHandler<ResourceHasBeenSetUnavailable>
    {
        private MongoCollection<RisorsaPrenotabileReadModel> _collection;
        private MongoCollection<InfoRisorsaReadModel> _collectionInfoRisorsa;

        public ListaDelleRisorsePrenotabili()
        {
            var url = new MongoUrl(ConfigurationManager.ConnectionStrings["readmodel"].ConnectionString);
            var client = new MongoClient(url);
            var db = client.GetServer().GetDatabase(url.DatabaseName);

            _collection = db.GetCollection<RisorsaPrenotabileReadModel>("RisorsePrenotabili");
            _collectionInfoRisorsa = db.GetCollection<InfoRisorsaReadModel>("InfoRisorsePrenotabili");

            _collection.Drop();
            _collectionInfoRisorsa.Drop();
        }

        public void Handle(ResourceCreated message)
        {
            _collectionInfoRisorsa.Save(new InfoRisorsaReadModel
                {
                    Id = message.Id,
                    Description = message.Description
                });
        }

        public void Handle(ResourceHasBeenSetAvailable message)
        {
            var info = _collectionInfoRisorsa.FindOneById(message.Id);
            if (info == null)
                throw new Exception("ERROREEE!!");

            _collection.Save(new RisorsaPrenotabileReadModel {Id = message.Id, Description = info.Description});
        }

        public void Handle(ResourceHasBeenSetUnavailable message)
        {
            _collection.Remove(Query<RisorsaPrenotabileReadModel>.Where(x=>x.Id == message.Id));
        }
    }
}
