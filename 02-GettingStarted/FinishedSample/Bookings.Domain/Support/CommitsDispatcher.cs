using Bookings.Domain.Messaging;
using NEventStore;
using NEventStore.Dispatcher;
using NEventStore.Persistence;

namespace Bookings.Domain.Support
{
    public class CommitsDispatcher : IDispatchCommits
    {
	    private readonly IBus _bus;

	    public CommitsDispatcher(IBus bus)
	    {
		    _bus = bus;
	    }

	    public void Dispose()
        {
        }

        public void Dispatch(ICommit commit)
        {
	        foreach (var eventMessage in commit.Events)
	        {
		        var msg = (IMessage) eventMessage.Body;
				_bus.Publish(msg);
	        }
        }
    }
}