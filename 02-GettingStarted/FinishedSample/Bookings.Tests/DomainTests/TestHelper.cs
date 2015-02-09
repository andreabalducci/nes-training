using System.Linq;
using CommonDomain;

namespace Bookings.Tests.DomainTests
{
	public static class TestHelper
	{
		public static bool RaisedEvent<TEvent>(this IAggregate aggregate)
		{
			return aggregate.GetUncommittedEvents().OfType<TEvent>().Any();
		}
	}
}