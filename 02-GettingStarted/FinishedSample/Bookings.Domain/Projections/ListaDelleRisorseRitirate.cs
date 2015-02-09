using System;
using System.Configuration;
using System.Linq;
using System.Text;
using Bookings.Domain.BookingContenxt;
using Bookings.Domain.Messaging;
using MongoDB.Driver;

namespace Bookings.Domain.Projections
{
    public class ListaDelleRisorseRitirate:
        IMessageHandler<RisorsaPresa>,
        IMessageHandler<RisorsaRestituita>,
        IMessageHandler<RisorsaCreata>
    {
        private MongoCollection<InfoRisorsaReadModel2> _collection;

        public ListaDelleRisorseRitirate()
        {
            var url = new MongoUrl(ConfigurationManager.ConnectionStrings["readmodel"].ConnectionString);
            var client = new MongoClient(url);
            var db = client.GetServer().GetDatabase(url.DatabaseName);

            _collection = db.GetCollection<InfoRisorsaReadModel2>("RisorseRitirate");

            _collection.Drop();
        }

        public void Handle(RisorsaPresa message)
        {
            var item = _collection.FindOneById(message.Id);
            if (item == null)
                throw new Exception("Nessuno saprà mai che mi hanno rubato una risorsa");
            item.Ritirata = true;
            item.Eventi.Add(message.Id);
            _collection.Save(item);
        }

        public void Handle(RisorsaRestituita message)
        {
            var item = _collection.FindOneById(message.Id);
            if (item == null)
                throw new Exception("Nessuno saprà mai che mi hanno rubato una risorsa");
            item.Ritirata = false;
            _collection.Save(item);
        }

        public void Handle(RisorsaCreata message)
        {
            _collection.Save(new InfoRisorsaReadModel2()
                {
                    Id = message.Id,
                    Description = message.Description
                });
        }
    }
}
