using System;

namespace SimpleEventStore.Domain.Events
{
    public class ItemUnloadFailed
    {
        public ItemUnloadFailed(string id, decimal qta)
        {
            Id = id;
            Qta = qta;
        }

        public string Id { get; set; }
        public Decimal Qta { get; set; }

    }
}