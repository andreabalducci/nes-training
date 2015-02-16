using System;
using Bookings.Domain.Messaging;

namespace Bookings.Tests.IntegrationTests.Sample
{
	/// <summary>
	/// TODO: fix
	/// </summary>
	public class CreateItemCommand : IMessage
	{
        public Guid CommandId { get; set; }
        public ItemId AggregateId { get; set; }

        public string Code { get; set; }
        public string Description { get; set; }

	    protected CreateItemCommand()
	    {
	    }

	    public CreateItemCommand(ItemId aggregateId, string description)
	    {
            CommandId = Guid.NewGuid();
            
            Description = description;
	        AggregateId = aggregateId;
	    }
	}
}