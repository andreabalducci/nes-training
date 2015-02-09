using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rebus;

namespace Bookings.Shared.Messaging
{
    public class NotifyReadModelUpdates
    {
        private IBus _bus;

        public NotifyReadModelUpdates(IBus bus)
        {
            _bus = bus;
        }

        public void Created<T>(T model) where T : class
        {
            _bus.Publish(ReadModelUpdatedMessage.Created(model));
        }

        public void Updated<T>(T model) where T : class
        {
            _bus.Publish(ReadModelUpdatedMessage.Updated(model));
        }

        public void Deleted<T>(T model) where T : class
        {
            _bus.Publish(ReadModelUpdatedMessage.Deleted(model));
        }
    }
}
