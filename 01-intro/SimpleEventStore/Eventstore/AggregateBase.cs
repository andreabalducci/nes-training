using System;
using System.Collections.Generic;

namespace SimpleEventStore.Eventstore
{
    public abstract class AggregateBase
	{
        public string Id { get; protected set; }
		private IList<object> _events = new List<object>();
		public int Version {
			get { return _events.Count; }
		}

        private int CommittedVersion { get; set; }

		public IList<object> Events
		{
			get { return _events; }
		}

		protected void RaiseEvent(object evt)
		{
			_events.Add(evt);
			((dynamic) this).Apply((dynamic)evt);
		}

		public void Save(EventStream stream)
		{
			stream.Events = this.Events;
		    stream.Version = this.Version;
		    stream.LastDispatchedEventIdx = CommittedVersion;
		}

		private void Load(EventStream stream)
		{
			if(Version > 0)
				throw new InvalidOperationException("Aggregate has an invalid state");

		    CommittedVersion = stream.Version;

			this._events= stream.Events;

			foreach (var @event in Events)
			{
				((dynamic)this).Apply((dynamic)@event);
			}

            if(Version != stream.Version)
                throw new Exception("Error restoring aggregate");
		}

        public static T Load<T>(EventStream stream)  where T:AggregateBase, new()
		{
			var aggregate = new T();
			aggregate.Load(stream);
			return aggregate;
		}
	}
}