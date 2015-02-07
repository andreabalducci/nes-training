using System;

namespace SimpleEventStore.Domain.Events
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