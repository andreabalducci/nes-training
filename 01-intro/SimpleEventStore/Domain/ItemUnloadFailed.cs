using System;

namespace SimpleEventStore.Domain
{
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