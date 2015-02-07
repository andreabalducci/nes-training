using System;
using System.Collections.Generic;

namespace SimpleEventStore.Tests
{
    public class Journal : Dictionary<string, JournalItem>
    {
        public JournalItem GetOrCreateItem(string id)
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
