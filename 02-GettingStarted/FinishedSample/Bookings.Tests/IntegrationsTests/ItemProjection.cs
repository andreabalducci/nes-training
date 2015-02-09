using System;
using System.Configuration;
using System.Diagnostics;
using Bookings.Domain.Messaging;
using MongoDB.Driver;

namespace Bookings.Tests.IntegrationsTests
{
	public class ItemReadModel
	{
		public Guid Id { get; set; }
		public string Description { get; set; }
	}

	/// <summary>
	/// TODO: Fix
	/// </summary>
	public class ItemProjection : IMessageHandler<ItemCreated>
	{
		private MongoCollection<ItemReadModel> _collection;

		public ItemProjection()
		{
			var url = new MongoUrl(ConfigurationManager.ConnectionStrings["readmodel"].ConnectionString);
			var client = new MongoClient(url);
		    var db = client.GetServer().GetDatabase(url.DatabaseName);

			_collection = db.GetCollection<ItemReadModel>("Items");
		}

		public static int Counter = 0;
		public void Handle(ItemCreated message)
		{
			Counter++;
			Debug.WriteLine("Item created");

            // what's wrong?
			_collection.Save(new ItemReadModel()
				                 {
					                 Id = message.Id.Id,
					                 Description = message.Description
				                 });
//		    ItemReadModel caricato = _collection.FindOneById(message.Id);
		}
	}
}