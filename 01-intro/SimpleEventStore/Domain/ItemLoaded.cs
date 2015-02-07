using System;

namespace CqrsEsSample.Domain
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

    public class ItemDisabled
    {
        public Guid Id { get; set; }

    }

}