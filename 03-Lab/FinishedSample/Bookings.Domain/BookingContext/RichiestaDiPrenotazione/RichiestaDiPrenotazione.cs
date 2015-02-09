using System;
using Bookings.Shared.Domain.BookingContext.BookableItem;
using Bookings.Shared.Domain.BookingContext.RichiestaDiPrenotazione;
using Bookings.Shared.Domain.BookingContext.RichiestaDiPrenotazione.Commands;
using Bookings.Shared.Domain.BookingContext.RichiestaDiPrenotazione.Events;
using CommonDomain.Core;

namespace Bookings.Domain.BookingContext.RichiestaDiPrenotazione
{
    public class RichiestaDiPrenotazione : AggregateBase
    {
        public BookableItemId BookableItemId { get; set; }
        public DateTime Da { get; set; }
        public DateTime A { get; set; }
        private RichiestaDiPrenotazioneId _idRichiesta;

        protected RichiestaDiPrenotazione()
        {

        }

        public RichiestaDiPrenotazione(RichiestaDiPrenotazioneId idRichiesta, BookableItemId bookableItemId, string utente, DateTime da, DateTime a, string causale)
        {
            RaiseEvent(new RichiestaDiPrenotazioneCreata(idRichiesta, bookableItemId, utente, da, a, causale));
        }

        public void Apply(RichiestaDiPrenotazioneCreata e)
        {
            this.Id = e.Id.Id;
            this._idRichiesta = e.Id;
            BookableItemId = e.BookableItemId;
            Da = e.Da;
            A = e.A;
        }

        public void Approva()
        {
            RaiseEvent(new RichiestaDiPrenotazioneApprovata(_idRichiesta, BookableItemId, Da, A));
        }

        public void Apply(RichiestaDiPrenotazioneApprovata evt)
        {
        }

        public void Respingi()
        {
            RaiseEvent(new RichiestaDiPrenotazioneRespinta(_idRichiesta));
        }

        public void Apply(RichiestaDiPrenotazioneRespinta evt)
        {
            
        }
    }
}
