using System;

namespace SimpleEventStore.Domain.Events
{
    public class ItemBelowSafetyStockLevel
    {
        public string Id { get; set; }

        public ItemBelowSafetyStockLevel(string id)
        {
            Id = id;
        }
    }
}