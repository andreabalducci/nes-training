using System;

namespace SimpleEventStore.Domain
{
	public class ItemUnloaded
	{
        public Guid Id { get; set; }
		public decimal Quantity { get; set; }

        public ItemUnloaded(Guid id, decimal quantity)
		{
			Id = id;
			Quantity = quantity;
		}
	}
}