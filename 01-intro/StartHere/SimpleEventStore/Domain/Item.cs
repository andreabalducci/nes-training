using System;
using SimpleEventStore.Domain.Events;
using SimpleEventStore.Eventstore;

namespace SimpleEventStore.Domain
{
    public class Item : AggregateBase
    {
        private decimal SafetyStockLevel { get; set; }
        internal decimal InStock { get; private set; }
        internal bool Disabled { get; private set; }

        public Item()
        {
        }

        public Item(string id, string code, string description, string uom, decimal safetyStockLevel)
        {
            RaiseEvent(new ItemCreated(id, code, description, uom, safetyStockLevel));
        }

        public void Disable()
        {
            if (Disabled)
                return;
            
            RaiseEvent(new ItemDisabled {Id = Id});
        }

        public void Apply(ItemCreated evt)
        {
            Id = evt.Id;
            SafetyStockLevel = evt.MinQta;
        }

        public void Apply(ItemDisabled evt)
        {
            Disabled = true;
        }
    }
}