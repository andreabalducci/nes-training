using System;
using Bookings.Domain.Messaging;

namespace Bookings.Tests.IntegrationsTests
{
	/// <summary>
	/// TODO: fix
	/// </summary>
	public class ItemCreated : IMessage
	{
        // tip: something missing?

        public ItemId Id { get; private set; }
		public string Description { get; private set; }

        public ItemCreated(ItemId id, string description)
		{
			Id = id;
			Description = description;
		}
	}

    public class ItemLoaded : IMessage
    {
        // tip: something missing?

        public ItemId Id { get; private set; }
        public decimal Qty { get; set; }

        public ItemLoaded(ItemId id, decimal qty)
        {
            Id = id;
            Qty = qty;
        }
    }
}