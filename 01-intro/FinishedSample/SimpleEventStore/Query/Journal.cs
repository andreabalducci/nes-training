using System.Collections.Generic;

namespace SimpleEventStore.Query
{
    public class Journal : Dictionary<string, JournalItemModel>
    {
        public JournalItemModel GetOrCreateItem(string id)
        {
            JournalItemModel ji;
            TryGetValue(id, out ji);
            if (ji == null)
            {
                ji = new JournalItemModel();
                this[id] = ji;
            }
            return ji;
        }
    }
}
