using System;

namespace SimpleEventStore.Domain.Events
{
	public class ItemUnloaded
	{
        public string Id { get; set; }
		public decimal Quantity { get; set; }

        public ItemUnloaded(string id, decimal quantity)
		{
			Id = id;
			Quantity = quantity;
		}
	}
}