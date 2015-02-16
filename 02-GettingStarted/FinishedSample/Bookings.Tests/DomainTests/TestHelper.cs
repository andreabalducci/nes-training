using System.Linq;
using CommonDomain;
using Machine.Specifications;

namespace Bookings.Tests.DomainTests
{
	public static class TestHelper
	{
		public static bool HasRaised<TEvent>(this IAggregate aggregate)
		{
			return aggregate.GetUncommittedEvents().OfType<TEvent>().Any();
		}

        public static TEvent Last<TEvent>(this IAggregate aggregate)
        {
            return aggregate.GetUncommittedEvents().OfType<TEvent>().Last();
        }
        
        public static void ShouldHadRaised<TEvent>(this IAggregate aggregate)
	    {
            aggregate.HasRaised<TEvent>().ShouldBeTrue();
	    }
	}
}