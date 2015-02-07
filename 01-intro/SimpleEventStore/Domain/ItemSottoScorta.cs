using System;

namespace SimpleEventStore.Domain
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