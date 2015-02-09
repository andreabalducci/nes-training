using System;
using CommonDomain;
using CommonDomain.Persistence;

namespace Bookings.Domain.Support
{
    public class AggregateFactory : IConstructAggregates
    {
        public IAggregate Build(Type type, Guid id, IMemento snapshot)
        {
            return (IAggregate) Activator.CreateInstance(type);
        }
    }
}