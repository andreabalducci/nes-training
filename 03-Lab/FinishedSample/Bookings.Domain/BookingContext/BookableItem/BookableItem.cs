using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookings.Shared.Domain.BookingContext.BookableItem;
using Bookings.Shared.Domain.BookingContext.BookableItem.Events;
using Bookings.Shared.Domain.BookingContext.RichiestaDiPrenotazione;
using CommonDomain.Core;

namespace Bookings.Domain.BookingContext.BookableItem
{
    public class BookableItem : AggregateBase
    {
        private BookableItemId _id;

        protected BookableItem()
        {
            Prenotazioni = new List<Prenotazione>();
        }

        public BookableItem(BookableItemId id, string description)
            : this()
        {
            _id = id;
            RaiseEvent(new BookableItemCreated(id, description));
        }

        public void Delete()
        {
            RaiseEvent(new BookableItemDeleted(_id));
        }

        public void Apply(BookableItemCreated evt)
        {
            this.Id = evt.Id.Id;
        }

        public void Apply(BookableItemDeleted evt)
        {
            this.Deleted = true;
        }

        protected bool Deleted { get; set; }

        protected List<Prenotazione> Prenotazioni { get; set; }

        public void Riserva(RichiestaDiPrenotazioneId  richiestaDiPrenotazioneId, DateTime da, DateTime a)
        {
            //se trovo una prenotazione con periodo sovrapposto a quello richiesto failed!
            // se poi è scazzato pace! :-)
            if (Prenotazioni.Any(x => da <= x.A && a >= x.Da))
            {
                RaiseEvent(new RiservaRifiutata(richiestaDiPrenotazioneId));
            }
            else
            {
                RaiseEvent(new RiservaAccettata(_id, da, a));         
            }
        }

        public void Apply(RiservaAccettata evt)
        {
            Prenotazioni.Add(new Prenotazione(new PrenotazioneId(Guid.NewGuid(), _id),  evt.Da, evt.A));
        }

        public void Apply(RiservaRifiutata evt)
        {
            // ma serve?
        }

        public void Libera(PrenotazioneId prenotazioneId)
        {
            throw new NotImplementedException("Libera cosa???");
            //RaiseEvent(new PrenotazioneLiberata(prenotazioneId));
        }

        public void Apply(PrenotazioneLiberata evt)
        {
            Prenotazioni.RemoveAll(x => x.PrenotazioneId.Id == evt.PrenotazioneId.Id);
        }
    }

    public class Prenotazione
    {
        public Prenotazione(PrenotazioneId prenotazioneId, DateTime da, DateTime a)
        {
            PrenotazioneId = prenotazioneId;
            Da = da;
            A = a;
        }

        protected Prenotazione()
        {
        }

        public DateTime Da { get; set; }
        public DateTime A { get; set; }
        public PrenotazioneId PrenotazioneId { get; set; }    
    }

  
}
