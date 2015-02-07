using System;

namespace SimpleEventStore.Domain.Events
{
    public class ItemSottoScorta
    {
        public Guid Id { get; set; }

        public ItemSottoScorta(Guid id)
        {
            Id = id;
        }
    }
}