using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bookings.Shared.Messaging
{
    public abstract class Command
    {
        public Guid CommandId { get; set; }

        protected Command()
        {
            this.CommandId = Guid.NewGuid();
        }
    }
}
