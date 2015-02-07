using System;

namespace SimpleEventStore.Domain
{
	public class ItemLoaded
	{
        public Guid Id { get; set; }
		public decimal Quantity { get; set; }
		public ItemLoaded(Guid id, decimal quantity)
		{
			Id = id;
			Quantity = quantity;
		}
	}
}