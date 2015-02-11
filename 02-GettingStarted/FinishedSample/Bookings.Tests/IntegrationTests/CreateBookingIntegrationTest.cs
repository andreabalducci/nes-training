using System;
using System.Diagnostics;
using Bookings.Domain.BookingContenxt;
using Bookings.Domain.BookingContenxt.Events;
using Bookings.Domain.Messaging;
using NUnit.Framework;

namespace Bookings.Tests.IntegrationTests
{
    [TestFixture]
    public class CreateBookingIntegrationTest : AbstractIntegrationTest
    {
		public class BookableItemCreatedEventHandler : IMessageHandler<ResourceCreated>
		{
			public static int Counter;
			public void Handle(ResourceCreated message)
			{
				Counter++;
				Debug.WriteLine("item has been created!");
			}
		}

	    public class BookableItemCreatedEventHandler2 : IMessageHandler<ResourceCreated>
	    {
		    public void Handle(ResourceCreated message)
		    {
			    Debug.WriteLine("Second handler");
		    }
	    }

	    [Test]
        public void save_and_load_new_bookableItem()
	    {
	        BookableItemCreatedEventHandler.Counter = 0;
	        var cmd = new
	            {
	                CommandId = new Guid("EBF513F4-F0A2-4454-AB77-A28A67B2BE82"),
                    ResourceId = Guid.NewGuid(),
	                Description = "Laptop"
	            };

	        var item = new Resource(cmd.ResourceId, cmd.Description);
            Repository.Save(item, cmd.CommandId, k => k.Add("key", "secret"));

            var loaded = Repository.GetById<Resource>(cmd.ResourceId);

		    FlushExecutionQueue();

            Assert.AreEqual(1, loaded.Version);
            Assert.AreEqual(item.Description, loaded.Description);
			Assert.AreEqual(1, BookableItemCreatedEventHandler.Counter);
        }
    }
}