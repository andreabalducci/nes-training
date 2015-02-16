using System;
using NUnit.Framework;

namespace Bookings.Tests.IntegrationTests.Sample
{
	[TestFixture]
	public class CommandHandlerTest : AbstractIntegrationTest
	{
		[Test]
		public void send_command_and_wait_projection()
		{
			// dirty hack to avoid ioc container for this test
			CreateItemCommandHandler.Repository = Repository;

		    var aggregateId = new ItemId(new Guid("F44F6000-1900-4B46-98F4-C59F752A5810"));
            SendCommand(new CreateItemCommand(aggregateId, "Articolo 1"));
			FlushExecutionQueue();
			Assert.AreEqual(1, ItemProjection.Counter);
		}
	}
}
