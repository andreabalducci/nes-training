using Bookings.ProcessManager.Messaggi;
using NEventStore;
using NEventStore.Dispatcher;
using NEventStore.Persistence;
using Rebus;

namespace Bookings.ProcessManager.Support
{
    public class CommitsDispatcher : IDispatchCommits
    {
        private IBus _bus;

        public CommitsDispatcher(IBus bus)
        {
            _bus = bus;
        }

        public void Dispose()
        {
        }

        public void Dispatch(ICommit commit)
        {
            foreach (var header in commit.Headers)
            {
                if (header.Key.StartsWith("UndispatchedMessage."))
                {
                    var cmd = header.Value;
                    if (cmd is TimeoutApprovazione)
                    {
                        var t = (TimeoutApprovazione) cmd;
                        _bus.Defer(t.Delay, t);
                    }
                    else
                    {
                        _bus.Send(header.Value);
                    }
                }
            }
        }
    }
}