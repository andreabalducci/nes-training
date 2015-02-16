using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookings.Shared.Projections;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Bookings.Client.Client
{
    public class ReadModel : IReadModel
    {
        private readonly MongoDatabase _readModelDb;

        public ReadModel(MongoDatabase readModelDb)
        {
            _readModelDb = readModelDb;
        }

        public IList<BookableItemInListReadModel> ListItems()
        {
            var collection = _readModelDb.GetCollection<BookableItemInListReadModel>("bookingList");
            var list = collection.AsQueryable().OrderBy(x => x.Id).ToArray();

            return list;
        }

        public IList<ListaRichiesteDiPrenotazioneReadModel> ListRichiesteDiPrenotazione()
        {
            var collection = _readModelDb.GetCollection<ListaRichiesteDiPrenotazioneReadModel>("richiesteDiPrenotazione");
            var list = collection.AsQueryable().OrderBy(x => x.Da).ToArray();

            return list;
        }
    }
}
