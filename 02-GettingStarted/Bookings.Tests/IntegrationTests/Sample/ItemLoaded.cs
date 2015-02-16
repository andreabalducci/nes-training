using Bookings.Domain.Messaging;

namespace Bookings.Tests.IntegrationTests.Sample
{
    public class ItemLoaded : IMessage
    {
        // tip: something missing?

        public ItemId Id { get; private set; }
        public decimal Qty { get; set; }

        public ItemLoaded(ItemId id, decimal qty)
        {
            Id = id;
            Qty = qty;
        }
    }
}