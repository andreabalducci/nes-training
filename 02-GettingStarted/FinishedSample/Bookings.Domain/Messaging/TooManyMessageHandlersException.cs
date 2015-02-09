using System;
using System.Collections.Generic;

namespace Bookings.Domain.Messaging
{
    public class TooManyMessageHandlersException : Exception
    {
        public Type MessageType { get; private set; }
        public IEnumerable<Type> HandlerTypes { get; private set; }

        public TooManyMessageHandlersException(Type messageType, IEnumerable<Type> handlerTypes)
        {
            MessageType = messageType;
            HandlerTypes = handlerTypes;
        }
    }
}