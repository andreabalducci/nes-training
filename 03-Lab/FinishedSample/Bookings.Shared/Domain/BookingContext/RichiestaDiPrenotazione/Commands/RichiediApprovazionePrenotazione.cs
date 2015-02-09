using Bookings.Shared.Messaging;

namespace Bookings.Shared.Domain.BookingContext.RichiestaDiPrenotazione.Commands
{
    public class RichiediApprovazionePrenotazione: Command
    {
        public RichiestaDiPrenotazioneId IdPrenotazione { get; set; }

        public RichiediApprovazionePrenotazione(RichiestaDiPrenotazioneId idPrenotazione)
        {
            IdPrenotazione = idPrenotazione;
        }

        public RichiediApprovazionePrenotazione()
        {
        }
    }
}