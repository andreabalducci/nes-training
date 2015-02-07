using System;
using System.Collections.Generic;

namespace SimpleEventStore.Tests
{
    public class Journal : Dictionary<Guid, JournalItem>
    {
        public JournalItem GetOrCreateItem(Guid id)
        {
            JournalItem ji;
            TryGetValue(id, out ji);
            if (ji == null)
            {
                ji = new JournalItem();
                this[id] = ji;
            }
            return ji;
        }
    }
}
