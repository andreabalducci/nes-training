using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bookings.Shared
{
    public interface IEventHandler<T> where T : IDomainEvent
    {
        void On(T evt);
    }

    public interface IDomainEvent
    {
    }

    public abstract class DomainEvent : IDomainEvent
    {
        public Guid EventId { get; set; }

        protected DomainEvent()
        {
            EventId = Guid.NewGuid();
        }
    }
}
