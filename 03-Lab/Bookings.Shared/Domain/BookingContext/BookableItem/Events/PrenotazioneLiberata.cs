namespace Bookings.Shared.Domain.BookingContext.BookableItem.Events
{
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