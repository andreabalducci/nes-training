using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookings.Domain.BookingContenxt.Events;
using CommonDomain.Core;

namespace Bookings.Domain.BookingContenxt
{
    public class Resource : AggregateBase
    {
        public bool Available { get; private set; }
        public string Description { get; protected set; }
        public bool Dismissed { get; protected set; }
        public bool Lent { get; protected set; }

        public Resource()
        {
        }

        public Resource(Guid id, string description)
        {
            RaiseEvent(new ResourceCreated(id, description));
        }

        public void MakeAvailable()
        {
            if (Dismissed)
                throw new Exception("La risorsa risulta dismessa. Non è possibile prenotarla");
            
            RaiseEvent(new ResourceHasBeenSetAvailable(this.Id));
        }

        public void Lend()
        {
            if (Lent)
                throw new Exception("La risorsa non può essere presa in quanto risulta già presa");
            
            RaiseEvent(new ResourceLent(Id));
        }

        public void Return()
        {
            if (!Lent)
                throw new Exception("La risorsa non può essere restituita in quanto risulta non ancora presa");
            
            RaiseEvent(new ResourceReturned(Id));
        }


        public void MakeUnavailable()
        {
            if (Dismissed)
                throw new Exception("La risorsa risulta dismessa. Non è possibile prenotarla");
            
            if (Available)
                RaiseEvent(new ResourceHasBeenSetUnavailable(Id));
        }

        public void Dismiss()
        {
            RaiseEvent(new ResourceDismissed(Id));
        }

        public void Apply(ResourceHasBeenSetUnavailable evt)
        {
            Available = false;
        }

        public void Apply(ResourceDismissed evt)
        {
            Dismissed = true;
        }

        public void Apply(ResourceLent evt)
        {
            Lent = true;
        }

        public void Apply(ResourceReturned evt)
        {
            Lent = false;
        }

        public void Apply(ResourceCreated evt)
        {
            this.Id = evt.Id;
            this.Description = evt.Description;
        }

        public void Apply(ResourceHasBeenSetAvailable evt)
        {
            Available = true;
        }
    }
}