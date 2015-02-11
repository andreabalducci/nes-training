using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Bookings.Domain.Support;

namespace Bookings.Domain.Messaging
{
    /// <summary>
    /// Single Threaded message queue.
    /// Just for the samples to avoid a real service bus
    /// </summary>
    public class SimpleMessageQueue : IBus, IDisposable
    {
        private readonly int _delay;
        private readonly ConcurrentQueue<IMessage> _messages = new ConcurrentQueue<IMessage>();
        private readonly ManualResetEvent _reset = new ManualResetEvent(false);
        private readonly ManualResetEvent _exited = new ManualResetEvent(false);
        private readonly ManualResetEvent _running = new ManualResetEvent(false);
        private readonly IDictionary<Type, IList<IMessageHandler>> _handlers = new Dictionary<Type, IList<IMessageHandler>>();

        public SimpleMessageQueue(int delay = 1000)
        {
            _delay = delay;
            ThreadPool.QueueUserWorkItem(MessagePump);
        }

        public void Send(IMessage message)
        {
            if (!_handlers.ContainsKey(message.GetType()))
                throw new Exception(string.Format("Missing handler for message {0}", message.GetType()));

            var handlers = _handlers[message.GetType()];

            if (handlers.Count > 1)
                throw new TooManyMessageHandlersException(message.GetType(), handlers.Select(x => x.GetType()));

            _messages.Enqueue(message);
        }

        public void Publish(IMessage message)
        {
            if (_handlers.ContainsKey(message.GetType()))
            {
                _messages.Enqueue(message);
            }
        }

        private void MessagePump(object state)
        {
            while (!_reset.WaitOne(_delay))
            {
                _running.Set();

                do
                {
                    IMessage message;
                    if (_messages.TryPeek(out message))
                    {
                        try
                        {
                            Process(message);
                            if (!_messages.TryDequeue(out message))
                                throw new Exception("Cannot dequeue handled message");
                        }
                        catch (Exception ex)
                        {
                            // log
                        }

                        Thread.Sleep(_delay);
                    }
                } while (_messages.Any());
            }

            _exited.Set();
        }

        private void Process(IMessage message)
        {
            foreach (var messageHandler in _handlers[message.GetType()])
            {
                ((dynamic)messageHandler).Handle((dynamic)message);
            }
        }

        public void Dispose()
        {
            _running.WaitOne();
            _reset.Set();
            _exited.WaitOne();
        }

        public void RegisterAssemblyOf<T>()
        {
            RegisterAssembly(typeof(T).Assembly);
        }

        public void RegisterAssembly(Assembly assembly)
        {
            var types = assembly.GetTypes()
                          .Where(x => x.IsClass && !x.IsAbstract && typeof(IMessageHandler).IsAssignableFrom(x));

            foreach (var type in types)
            {
                var handler = (IMessageHandler)Activator.CreateInstance(type);
                var interfaces = type.GetInterfaces().Where(x => typeof(IMessageHandler).IsAssignableFrom(x) && x.GetGenericArguments().Length == 1);
                foreach (var @interface in interfaces)
                {
                    var messageType = @interface.GetGenericArguments().Single();
                    IList<IMessageHandler> handlersOfMessage = null;
                    if (!_handlers.TryGetValue(messageType, out handlersOfMessage))
                    {
                        handlersOfMessage = new List<IMessageHandler>();
                        _handlers[messageType] = handlersOfMessage;
                    }

                    handlersOfMessage.Add(handler);
                }
            }
        }

        public bool HasPendingMessages
        {
            get
            {
                IMessage msg;
                return _messages.TryPeek(out msg);
            }
        }
    }
}
