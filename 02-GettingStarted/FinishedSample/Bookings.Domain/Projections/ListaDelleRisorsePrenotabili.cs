using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Bookings.Domain.BookingContenxt;
using Bookings.Domain.Messaging;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Bookings.Domain.Projections
{
    public class RisorsaPrenotabileReadModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
    }

    public class InfoRisorsaReadModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
    }

    public class ListaDelleRisorsePrenotabili :
        IMessageHandler<RisorsaCreata>,
        IMessageHandler<RisorsaResaPrenotabile>,
        IMessageHandler<RisorsaResaNonPrenotabile>
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

        public void Handle(RisorsaCreata message)
        {
            _collectionInfoRisorsa.Save(new InfoRisorsaReadModel
                {
                    Id = message.Id,
                    Description = message.Description
                });
        }

        public void Handle(RisorsaResaPrenotabile message)
        {
            var info = _collectionInfoRisorsa.FindOneById(message.Id);
            if (info == null)
                throw new Exception("ERROREEE!!");

            _collection.Save(new RisorsaPrenotabileReadModel {Id = message.Id, Description = info.Description});
        }

        public void Handle(RisorsaResaNonPrenotabile message)
        {
            _collection.Remove(Query<RisorsaPrenotabileReadModel>.Where(x=>x.Id == message.Id));
        }
    }
}
