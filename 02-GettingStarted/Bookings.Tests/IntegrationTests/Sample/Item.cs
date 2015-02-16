using CommonDomain.Core;

namespace Bookings.Tests.IntegrationTests.Sample
{
	/// <summary>
	/// TODO:	explain
	///		:	add Delete, Load, Unload
	/// </summary>
	public class Item : AggregateBase
	{
	    private ItemId _id;

		protected Item()
		{
		}

        public Item(ItemId id, string description)
		{
			RaiseEvent(new ItemCreated(id, description));
		}

        public void Load(decimal qty)
        {
            //RaiseEvent(new ItemLoaded(new ItemId(this.Id), qty));
            RaiseEvent(new ItemLoaded(_id, qty));
        }

		private void Apply(ItemCreated evt)
		{
			this.Id = evt.Id.Id;
		    _id = evt.Id;
		}

        private void Apply(ItemLoaded evt)
        {

        }
	}
}