using System;
using CommonDomain;
using CommonDomain.Persistence;
using Fasterflect;

namespace Bookings.Service.Support
{
    public class AggregateFactory : IConstructAggregates
    {
        public IAggregate Build(Type type, Guid id, IMemento snapshot)
        {
            var ctor = type.Constructor(Flags.Default, new Type[] { });

            if (ctor == null)
                throw new MissingDefaultCtorException(type);

            var aggregate = (IAggregate)ctor.CreateInstance();

            /*
            if (snapshot != null && aggregate is ISnapshotable)
            {
                ((ISnapshotable)aggregate).Restore(snapshot);
            }
            */

            return aggregate;
        }
    }
}