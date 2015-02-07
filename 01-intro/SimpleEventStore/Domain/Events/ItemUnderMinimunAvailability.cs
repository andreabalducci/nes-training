using System;

namespace SimpleEventStore.Domain.Events
{
    public class ItemUnderMinimunAvailability
    {
        public string Id { get; set; }

        public ItemUnderMinimunAvailability(string id)
        {
            Id = id;
        }
    }
}