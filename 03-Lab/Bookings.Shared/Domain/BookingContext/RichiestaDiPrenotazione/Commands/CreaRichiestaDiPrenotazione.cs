using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookings.Shared.Domain.BookingContext.BookableItem;
using Bookings.Shared.Messaging;

namespace Bookings.Shared.Domain.BookingContext.RichiestaDiPrenotazione.Commands
{
    public class CreaRichiestaDiPrenotazione : Command
    {
        public BookableItemId Itemid { get; set; }
        public string Causale { get; set; }
        public string Utente { get; set; }
        public DateTime Da { get; set; }
        public DateTime A { get; set; }

        public RichiestaDiPrenotazioneId RichiestaDiPrenotazioneId { get; set; }

        protected CreaRichiestaDiPrenotazione()
        {
        }

        public CreaRichiestaDiPrenotazione(RichiestaDiPrenotazioneId richiestaDiPrenotazioneId, BookableItemId itemid, string motivazione, string utente, DateTime da, DateTime a)
        {
            RichiestaDiPrenotazioneId = richiestaDiPrenotazioneId;
            Itemid = itemid;
            Causale = motivazione;
            Utente = utente;
            Da = da;
            A = a;
        }
    }
}
