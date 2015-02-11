using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bookings.Shared
{
    public interface IEventHandler<T> where T : IDomainEvent
    {
        void On(T evt);
    }
}
