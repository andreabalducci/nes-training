using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonDomain.Core;

namespace Bookings.Domain.BookingContenxt
{
    public class Risorsa : AggregateBase
    {
        public bool Prenotabile { get; private set; }

        public Risorsa()
        {
        }

        public Risorsa(Guid id, string description)
        {
            RaiseEvent(new RisorsaCreata(id, description));
        }

        public void RendiPrenotabile()
        {
            if (Dismessa)
                throw new Exception("La risorsa risulta dismessa. Non è possibile prenotarla");
            RaiseEvent(new RisorsaResaPrenotabile(this.Id));
        }

        public string Description { get; protected set; }

        public bool Dismessa { get; protected set; }

        public void Apply(RisorsaCreata evt)
        {
            this.Id = evt.Id;
            this.Description = evt.Description;
        }

        public void Apply(RisorsaResaPrenotabile evt)
        {
            Prenotabile = true;
        }

        public void RendiNonPrenotabile()
        {
            if (Dismessa)
                throw new Exception("La risorsa risulta dismessa. Non è possibile prenotarla");
            
            if (Prenotabile)
                RaiseEvent(new RisorsaResaNonPrenotabile(Id));
        }

        public void Apply(RisorsaResaNonPrenotabile evt)
        {
            Prenotabile = false;
        }

        public void Dismetti()
        {
            RaiseEvent(new RisorsaDismessa(Id));
        }

        public void Apply(RisorsaDismessa evt)
        {
            Dismessa = true;
        }

        public void Prendi()
        {
            if (Presa)
                throw new Exception("La risorsa non può essere presa in quanto risulta già presa");
            RaiseEvent(new RisorsaPresa(Id));
        }

        public bool Presa { get; protected set; }

        public void Apply(RisorsaPresa evt)
        {
            Presa = true;
        }

        public void Apply(RisorsaRestituita evt)
        {
            Presa = false;
        }


        public void Restituisci()
        {
            if (!Presa)
                throw new Exception("La risorsa non può essere restituita in quanto risulta non ancora presa");
            RaiseEvent(new RisorsaRestituita(Id));
        }
    }
}
