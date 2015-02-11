using System;
using Bookings.Shared.Domain.BookingContext.BookableItem;

namespace Bookings.Shared.Domain.BookingContext.RichiestaDiPrenotazione.Events
{
    public class RichiestaDiPrenotazioneApprovata: DomainEvent
    {
        public RichiestaDiPrenotazioneApprovata(RichiestaDiPrenotazioneId idPrenotazione,
            BookableItemId bookableItemId, DateTime da , DateTime a)
        {
            Id = idPrenotazione;
            BookableItemId = bookableItemId;
            Da = da;
            A = a;
        }

        public RichiestaDiPrenotazioneId Id { get; set; }
        public BookableItemId BookableItemId { get; set; }
        public DateTime Da { get; set; }
        public DateTime A { get; set; }

        protected RichiestaDiPrenotazioneApprovata()
        {
        }
    }
}