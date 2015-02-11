using System;
using Bookings.Domain.Messaging;

namespace Bookings.Domain.BookingContenxt.Events
{
	public class ResourceCreated : IMessage
	{
		public Guid Id { get; protected set; }
		public string Description { get; protected set; }

		public ResourceCreated(Guid id, string description)
		{
			Id = id;
			Description = description;
		}
	}
}