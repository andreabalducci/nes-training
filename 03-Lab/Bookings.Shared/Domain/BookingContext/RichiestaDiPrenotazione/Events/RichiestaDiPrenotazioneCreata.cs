using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookings.Shared.Domain.BookingContext.BookableItem;

namespace Bookings.Shared.Domain.BookingContext.RichiestaDiPrenotazione.Events
{
    public class RichiestaDiPrenotazioneCreata : DomainEvent
    {
        public RichiestaDiPrenotazioneId Id { get; set; }
        public BookableItemId BookableItemId { get; set; }
        public string Utente { get; set; }
        public DateTime Da { get; set; }
        public DateTime A { get; set; }
        public string Causale { get; set; }

        protected RichiestaDiPrenotazioneCreata()
        {
            
        }

        public RichiestaDiPrenotazioneCreata(RichiestaDiPrenotazioneId id, BookableItemId bookableItemId, string utente, DateTime da, DateTime dateTime, string causale)
        {
            Id = id;
            BookableItemId = bookableItemId;
            Utente = utente;
            Da = da;
            A = dateTime;
            Causale = causale;
        }
    }
}
