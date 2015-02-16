using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookings.Shared;
using NEventStore;

namespace Bookings.Tests.Advanced
{
    public class ReplayEvents
    {
        private readonly IStoreEvents _eventStore;
      
        public ReplayEvents(IStoreEvents eventStore)
        {
            _eventStore = eventStore;
        }

        public void ReplayAll(Action<IDomainEvent> dispatcher)
        {
            var commits = _eventStore.Advanced.GetFromTo(
                "default",
                DateTime.MinValue, 
                DateTime.MaxValue
            );

            foreach (var commit in commits)
            {
                foreach (var evt in commit.Events)
                {
                    dispatcher((IDomainEvent) evt.Body);
                }
            }
        }
    }
}
