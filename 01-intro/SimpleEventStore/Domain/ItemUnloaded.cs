using System;

namespace CqrsEsSample.Domain
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


    public class ItemSottoScorta
	{
        public Guid Id { get; set; }

        public ItemSottoScorta(Guid id)
		{
			Id = id;
        }
	}

    public class ItemUnloadFailed
    {
        public ItemUnloadFailed(Guid id, decimal qta)
        {
            Id = id;
            Qta = qta;
        }

        public Guid Id { get; set; }
        public Decimal Qta { get; set; }

    }
}