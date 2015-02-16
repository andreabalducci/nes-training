using System;

namespace Bookings.Tests.IntegrationTests.Sample
{
    public class ItemId
    {
        public Guid Id { get; set; }

        public ItemId()
        {
            
        }

        public ItemId(Guid id)
        {
            Id = id;
        }
    }
}