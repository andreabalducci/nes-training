using System;
using Bookings.Shared.Domain.BookingContext.RichiestaDiPrenotazione;
using Bookings.Shared.Messaging;

namespace Bookings.Shared.Domain.BookingContext.BookableItem.Commands
{
    public class RiservaBookableItem : Command
    {
        public BookableItemId Itemid { get; set; }
        public DateTime Da { get; set; }
        public DateTime A { get; set; }

        public RichiestaDiPrenotazioneId RichiestaDiPrenotazioneId { get; set; }

        protected RiservaBookableItem()
        {
        }

        public RiservaBookableItem(BookableItemId itemid, DateTime da, DateTime a, RichiestaDiPrenotazioneId richiestaDiPrenotazioneId)
        {
            this.Itemid = itemid;
            Da = da;
            A = a;
            RichiestaDiPrenotazioneId = richiestaDiPrenotazioneId;
        }
    }
}