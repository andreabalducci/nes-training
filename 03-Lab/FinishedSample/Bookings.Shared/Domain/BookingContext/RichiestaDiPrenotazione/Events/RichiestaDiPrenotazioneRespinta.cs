namespace Bookings.Shared.Domain.BookingContext.RichiestaDiPrenotazione.Events
{
    public class RichiestaDiPrenotazioneRespinta: DomainEvent
    {
        protected RichiestaDiPrenotazioneRespinta()
        {
        }

        public RichiestaDiPrenotazioneId IdRichiesta { get; set; }

        public RichiestaDiPrenotazioneRespinta(RichiestaDiPrenotazioneId idRichiesta)
        {
            IdRichiesta = idRichiesta;
        }
    }
}