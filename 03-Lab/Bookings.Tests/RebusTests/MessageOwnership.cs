using System;
using Rebus;

namespace Bookings.Tests.RebusTests
{
    public class MessageOwnership : IDetermineMessageOwnership
    {
        public string GetEndpointFor(Type messageType)
        {
            return "rebus.test.input";
        }
    }
}