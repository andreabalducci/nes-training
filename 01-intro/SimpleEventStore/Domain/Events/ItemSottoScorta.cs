using System;

namespace SimpleEventStore.Domain.Events
{
    public class ItemSottoScorta
    {
        public string Id { get; set; }

        public ItemSottoScorta(string id)
        {
            Id = id;
        }
    }
}