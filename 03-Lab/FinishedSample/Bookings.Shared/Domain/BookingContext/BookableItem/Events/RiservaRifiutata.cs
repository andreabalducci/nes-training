using Bookings.Shared.Domain.BookingContext.RichiestaDiPrenotazione;

namespace Bookings.Shared.Domain.BookingContext.BookableItem.Events
{
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
}