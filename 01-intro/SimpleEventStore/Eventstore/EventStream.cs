using System.Collections.Generic;

namespace CqrsEsSample.Eventstore
{
    public class EventStream
    {
        public IList<object> Events { get; set; }
        public int Version { get; set; }
        public int LastDispatchedEventIdx { get; set; }
    }
}