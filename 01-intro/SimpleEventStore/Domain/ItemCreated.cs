using System;

namespace CqrsEsSample.Domain
{
	public class ItemCreated
	{
		public Guid Id { get; set; }
		public string Code { get; set; }
		public string Description { get; set; }
		public string Uom { get; set; }
		public decimal MinQta { get; set; }

        public ItemCreated(Guid id, string code, string description, string uom, decimal minQta)
		{
			Id = id;
			Code = code;
			Description = description;
			Uom = uom;
            MinQta = minQta;
		}
	}
}