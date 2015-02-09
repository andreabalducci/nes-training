using System.Linq;
using CommonDomain;

namespace Bookings.Tests
{
	public static class TestHelper
	{
		public static bool RaisedEvent<TEvent>(this IAggregate aggregate)
		{
			return aggregate.GetUncommittedEvents().OfType<TEvent>().Any();
		}

	    public static TEvent LastEventOfType<TEvent>(this IAggregate aggregate)
	    {
            return aggregate.GetUncommittedEvents().OfType<TEvent>().LastOrDefault();
        }
	}
}