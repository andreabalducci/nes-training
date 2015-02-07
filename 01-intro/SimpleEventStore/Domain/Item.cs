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

        public Item(string id, string code, string description, string uom, decimal minQta)
        {
            RaiseEvent(new ItemCreated(id, code, description, uom, minQta));
        }

        public void Load(decimal quantity)
        {
            if (Disabled)
                throw new Exception("Item disabled");
            RaiseEvent(new ItemLoaded(Id, quantity));
        }

        public void Unload(decimal quantity)
        {
            if (Disabled)
                throw new Exception("Item disabled");
            if (InStock < quantity)
            {
                RaiseEvent(new ItemUnloadFailed(Id, quantity));
            }
            else
            {
                RaiseEvent(new ItemUnloaded(Id, quantity));
                if (SafetyStockLevel > InStock)
                {
                    RaiseEvent(new ItemBelowSafetyStockLevel(Id));
                }
            }
        }

        public void Disable()
        {
            if (Disabled)
                return;
            //    throw new Exception("Item already disabled");
            //if (_qta > 0)
            //    throw new Exception("Item qta");
            RaiseEvent(new ItemDisabled {Id = Id});
        }

        public void Apply(ItemCreated evt)
        {
            Id = evt.Id;
            SafetyStockLevel = evt.MinQta;
        }

        public void Apply(ItemLoaded evt)
        {
            InStock += evt.Quantity;
        }

        public void Apply(ItemUnloaded evt)
        {
            InStock -= evt.Quantity;
        }

        public void Apply(ItemDisabled evt)
        {
            Disabled = true;
        }

        public void Apply(ItemBelowSafetyStockLevel evt)
        {
        }

        public void Apply(ItemUnloadFailed evt)
        {
        }
    }
}