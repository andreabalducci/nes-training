using System;
using System.Collections.Generic;

namespace Bookings.Domain.Query
{
    public class InfoRisorsaReadModel2
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public bool Ritirata { get; set; }

        public List<Guid> Eventi { get; set; }

        public InfoRisorsaReadModel2()
        {
            this.Eventi = new List<Guid>();
        }
    }
}