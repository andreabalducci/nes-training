using Bookings.Domain.Messaging;

namespace Bookings.Tests.IntegrationTests.Sample
{
	/// <summary>
	/// TODO: fix
	/// </summary>
	public class ItemCreated : IMessage
	{
        // tip: something missing?

        public ItemId Id { get; private set; }
		public string Description { get; private set; }

        public ItemCreated(ItemId id, string description)
		{
			Id = id;
			Description = description;
		}
	}
}