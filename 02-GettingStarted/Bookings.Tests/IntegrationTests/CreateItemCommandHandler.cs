using Bookings.Domain.Messaging;
using CommonDomain.Persistence;

namespace Bookings.Tests.IntegrationTests
{
	/// <summary>
	/// TODO: fix
	/// </summary>
	public class CreateItemCommandHandler : IMessageHandler<CreateItemCommand>
	{
		public static IRepository Repository { get; set; }

		public void Handle(CreateItemCommand message)
		{
			var item = new Item(message.AggregateId, message.Description);
            Repository.Save(item, message.CommandId);
		}
	}
}