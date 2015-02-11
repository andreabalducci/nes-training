using System;

namespace Bookings.Shared.Projections
{
    public class ListaRichiesteDiPrenotazioneReadModel
    {
        public Guid Id { get; set; }
        public string Utente { get; set; }
        public string Causale { get; set; }
        public DateTime Da { get; set; }
        public DateTime A { get; set; }
        public string BookableItemDescription { get; set; }
        public string Stato { get; set; }
    }
}