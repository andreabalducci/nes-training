using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookings.Shared.Domain.BookingContext.RichiestaDiPrenotazione.Events;
using MongoDB.Driver;

namespace Bookings.Shared.Projections
{
    public class ListaRichiesteDiPrenotazioneProjection :
        IEventHandler<RichiestaDiPrenotazioneCreata>,
        IEventHandler<RichiestaDiPrenotazioneApprovata>,
        IEventHandler<RichiestaDiPrenotazioneRespinta>
    {
        private MongoCollection<ListaRichiesteDiPrenotazioneReadModel> _collection;
        private MongoCollection<BookableItemInListReadModel> _collectionBookableItems;

        public ListaRichiesteDiPrenotazioneProjection(MongoDatabase db)
        {
            _collection = db.GetCollection<ListaRichiesteDiPrenotazioneReadModel>("richiesteDiPrenotazione");
            _collectionBookableItems = db.GetCollection<BookableItemInListReadModel>("bookingList");
        }

        public void On(RichiestaDiPrenotazioneCreata evt)
        {
            var bi = _collectionBookableItems.FindOneById(evt.BookableItemId.Id);
            if (bi == null)
                throw new Exception("Bookable Item NON ESISTE");

            var rm = new ListaRichiesteDiPrenotazioneReadModel
                {
                    Id = evt.Id.Id,
                    Utente = evt.Utente,
                    BookableItemDescription = bi.Description,
                    Da = evt.Da,
                    A = evt.A,
                    Causale = evt.Causale
                };
            _collection.Save(rm);
        }

        public void On(RichiestaDiPrenotazioneApprovata evt)
        {
            ListaRichiesteDiPrenotazioneReadModel ric = _collection.FindOneById(evt.Id.Id);
            ric.Stato = "Approvata";
            _collection.Save(ric);
        }

        public void On(RichiestaDiPrenotazioneRespinta evt)
        {
            ListaRichiesteDiPrenotazioneReadModel ric = _collection.FindOneById(evt.IdRichiesta.Id);
            ric.Stato = "Respinta";
            _collection.Save(ric);
        }
    }
}
