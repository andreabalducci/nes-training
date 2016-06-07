//using Bookings.Shared;
//using Castle.MicroKernel;
//using NEventStore;
//using Rebus;

//namespace Bookings.Service.Support
//{
//    public class CommitsDispatcher : IDispatchCommits
//    {
//        private IKernel _kernel;
//        private IBus _bus;
//        public CommitsDispatcher(IKernel kernel, IBus bus)
//        {
//            _kernel = kernel;
//            _bus = bus;
//        }

//        public void Dispose()
//        {
//        }

//        public void Dispatch(ICommit commit)
//        {
//            // in process
//            foreach (var eventMessage in commit.Events)
//            {
//                var message  =eventMessage.Body;
//                var handlerType = typeof (IEventHandler<>).MakeGenericType(new[] {message.GetType()});
//                var handlers = _kernel.ResolveAll(handlerType); // singleton handlers
//                foreach (var handler in handlers)
//                {
//                    ((dynamic) handler).On((dynamic) message);
//                }
//            }

//            // publish on bus
//            foreach (var eventMessage in commit.Events)
//            {
//                var message = eventMessage.Body;
//                _bus.Publish(message);
//            }
//        }
//    }
//}