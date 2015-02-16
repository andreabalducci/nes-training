using System;
using Bookings.Shared.Domain.BookingContext.BookableItem;

namespace Bookings.Domain.BookingContext.BookableItem
{
    public class Prenotazione
    {
        public Prenotazione(PrenotazioneId prenotazioneId, DateTime da, DateTime a)
        {
            PrenotazioneId = prenotazioneId;
            Da = da;
            A = a;
        }

        protected Prenotazione()
        {
        }

        public DateTime Da { get; set; }
        public DateTime A { get; set; }
        public PrenotazioneId PrenotazioneId { get; set; }    
    }
}