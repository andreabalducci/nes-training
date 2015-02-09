using System;
using Bookings.Shared.Domain.BookingContext.RichiestaDiPrenotazione;

namespace Bookings.Shared.Domain.BookingContext.BookableItem.Events
{
    public class BookableItemDeleted : DomainEvent
    {
        public BookableItemId Id { get; set; }

        public BookableItemDeleted(BookableItemId id)
        {
            Id = id;
        }
    }

    public class RiservaAccettata : DomainEvent
    {
        protected RiservaAccettata()
        {
        }

        public BookableItemId IdRisorsa { get; set; }
        public DateTime Da { get; set; }
        public DateTime A { get; set; }

        public RiservaAccettata(BookableItemId id, DateTime da, DateTime a)
        {
            IdRisorsa = id;
            Da = da;
            A = a;
        }
    }

    public class RiservaRifiutata : DomainEvent
    {
        public RichiestaDiPrenotazioneId RichiestaDiPrenotazioneId { get; set; }

        protected RiservaRifiutata()
        {
        }

        public RiservaRifiutata(RichiestaDiPrenotazioneId richiestaDiPrenotazioneId)
        {
            RichiestaDiPrenotazioneId = richiestaDiPrenotazioneId;
        }
    }

    public class PrenotazioneLiberata : DomainEvent
    {
        public PrenotazioneLiberata(PrenotazioneId prenotazioneId)
        {
            PrenotazioneId = prenotazioneId;
        }

        protected PrenotazioneLiberata()
        {
        }

        public PrenotazioneId PrenotazioneId { get; set; }
    }
}