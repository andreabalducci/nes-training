using System;

namespace SimpleEventStore.Domain.Events
{
	public class ItemLoaded
	{
        public string Id { get; set; }
		public decimal Quantity { get; set; }
        public ItemLoaded(string id, decimal quantity)
		{
			Id = id;
			Quantity = quantity;
		}
	}
}