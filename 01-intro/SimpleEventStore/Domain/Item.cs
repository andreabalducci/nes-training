using System;
using SimpleEventStore.Domain.Events;
using SimpleEventStore.Eventstore;

namespace SimpleEventStore.Domain
{
    public class Item : AggregateBase
    {
        public Item()
        {
        }

        private decimal _qta;
        public decimal Qta { get { return _qta; }
            set { _qta = value; }
        }


        private bool _disabled;
        private decimal _minQta;
        public bool Disabled { get { return _disabled; } }

        public Item(Guid id, string code, string description, string uom, decimal minQta)
        {
            RaiseEvent(new ItemCreated(id, code, description, uom, minQta));
        }

        public void Load(decimal quantity)
        {
            if (_disabled)
                throw new Exception("Item disabled");
            RaiseEvent(new ItemLoaded(Id, quantity));
        }

        public void Unload(decimal quantity)
        {
            if (_disabled)
                throw new Exception("Item disabled");
            if (_qta < quantity)
            {
                RaiseEvent(new ItemUnloadFailed(Id, quantity));
            }
            else
            {
                RaiseEvent(new ItemUnloaded(Id, quantity));
                if (_minQta > _qta)
                {
                    RaiseEvent(new ItemSottoScorta(Id));
                }
            }
        }

        public void Disable()
        {
            if (_disabled)
                return;
            //    throw new Exception("Item already disabled");
            //if (_qta > 0)
            //    throw new Exception("Item qta");
            RaiseEvent(new ItemDisabled(){Id = Id});
        }

        public void Apply(ItemCreated evt)
        {
            this.Id = evt.Id;
            _minQta = evt.MinQta;
        }

        public void Apply(ItemLoaded evt)
        {
            _qta += evt.Quantity;
        }

        public void Apply(ItemUnloaded evt)
        {
            _qta -= evt.Quantity;
            
        }

        public void Apply(ItemDisabled evt)
        {
            _disabled = true;
        }

        public void Apply(ItemSottoScorta evt)
        {
        }

        public void Apply(ItemUnloadFailed evt)
        {
        }
    }
}