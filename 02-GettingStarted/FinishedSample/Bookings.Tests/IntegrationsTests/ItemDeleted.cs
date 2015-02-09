using System;
using Bookings.Domain.Messaging;

namespace Bookings.Tests.IntegrationsTests
{
	public class ItemDeleted : IMessage
	{
		public Guid Id { get; private set; }

		public ItemDeleted(Guid id)
		{
			Id = id;
		}
	}
}