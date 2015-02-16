using System;

namespace Bookings.Shared
{
    public abstract class DomainEvent : IDomainEvent
    {
        public Guid EventId { get; set; }

        protected DomainEvent()
        {
            EventId = Guid.NewGuid();
        }
    }
}