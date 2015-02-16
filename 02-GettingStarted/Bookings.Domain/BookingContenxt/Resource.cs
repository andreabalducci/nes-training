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

        public Resource()
        {
        }

        public Resource(Guid id, string description)
        {
            RaiseEvent(new ResourceCreated(id, description));
        }

        public void Dismiss()
        {
            RaiseEvent(new ResourceDismissed(Id));
        }

        public void Apply(ResourceDismissed evt)
        {
            Dismissed = true;
        }

        public void Apply(ResourceCreated evt)
        {
            this.Id = evt.Id;
            this.Description = evt.Description;
        }
    }
}