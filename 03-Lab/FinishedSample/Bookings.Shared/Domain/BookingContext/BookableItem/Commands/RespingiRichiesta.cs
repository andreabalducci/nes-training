using Bookings.Shared.Domain.BookingContext.RichiestaDiPrenotazione;
using Bookings.Shared.Messaging;

namespace Bookings.Shared.Domain.BookingContext.BookableItem.Commands
{
    public class RespingiRichiesta : Command
    {
        protected RespingiRichiesta()
        {
        }

        public RichiestaDiPrenotazioneId RichiestaDiPrenotazioneId { get; set; }

        public RespingiRichiesta(RichiestaDiPrenotazioneId richiestaDiPrenotazioneId)
        {
            RichiestaDiPrenotazioneId = richiestaDiPrenotazioneId;
        }
    }
}