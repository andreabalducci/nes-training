using System;
using Bookings.Shared.Domain.BookingContext.RichiestaDiPrenotazione;
using Bookings.Shared.Messaging;

namespace Bookings.Shared.Domain.BookingContext.BookableItem.Commands
{
    public class CreateBookableItem : Command
    {
        public BookableItemId Itemid { get; set; }
        public string Description { get; set; }

        protected CreateBookableItem()
        {
        }

        public CreateBookableItem(BookableItemId itemid, string description)
        {
            this.Itemid = itemid;
            this.Description = description;
        }
    }

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
