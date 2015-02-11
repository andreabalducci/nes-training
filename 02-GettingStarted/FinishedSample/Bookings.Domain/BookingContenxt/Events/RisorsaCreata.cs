using System;
using Bookings.Domain.Messaging;

namespace Bookings.Domain.BookingContenxt.Events
{
	public class RisorsaCreata : IMessage
	{
		public Guid Id { get; protected set; }
		public string Description { get; protected set; }

		public RisorsaCreata(Guid id, string description)
		{
			Id = id;
			Description = description;
		}
	}
}