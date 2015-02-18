using System;

namespace SimpleEventStore.Domain.Events
{
	public class ItemCreated
	{
        public string Id { get; set; }
		public string Code { get; set; }
		public string Description { get; set; }
		public string Uom { get; set; }
		public decimal SafetyStockLevel { get; set; }

        public ItemCreated(string id, string code, string description, string uom, decimal safetyStockLevel)
		{
			Id = id;
			Code = code;
			Description = description;
			Uom = uom;
            SafetyStockLevel = safetyStockLevel;
		}
	}
}