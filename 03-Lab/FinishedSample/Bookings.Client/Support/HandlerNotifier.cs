using System;
using Bookings.Shared.Messaging;
using Newtonsoft.Json;
using Rebus;

namespace Bookings.Client.Support
{
    public class HandlerNotifier : IHandleMessages<ReadModelUpdatedMessage>
    {
        public void Handle(ReadModelUpdatedMessage message)
        {
            Console.WriteLine("\nReadmodel updated:\n{0}\n", JsonConvert.SerializeObject(message, Formatting.Indented));
        }
    }
}