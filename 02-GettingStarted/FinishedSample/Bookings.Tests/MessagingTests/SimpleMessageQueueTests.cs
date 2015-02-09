using System;
using System.Diagnostics;
using Bookings.Domain.Messaging;
using NUnit.Framework;

namespace Bookings.Tests.MessagingTests
{
    [TestFixture]
    public class SimpleMessageQueueTests
    {
        public class UnHandledMessage : IMessage
        {
        }

        public class SampleMessage : IMessage
        {
            public Guid Id { get; private set; }
            public string Text { get; private set; }
            public SampleMessage(string text)
            {
                Text = text;
                Id = Guid.NewGuid();
            }

            public override string ToString()
            {
                return string.Format("Id: {0}, Text: {1}", Id, Text);
            }
        }

        public class ChildMessage : IMessage
        {
            public Guid Id { get; private set; }
            public string Text { get; private set; }

            public ChildMessage(string text)
            {
                Id = Guid.NewGuid();
                Text = text;
            }
            public override string ToString()
            {
                return string.Format("Id: {0}, Text: {1}", Id, Text);
            }
        }

        public class SampleMessageHandler : IMessageHandler<SampleMessage>, IMessageHandler<ChildMessage>
        {
            public static IBus ApplicationBus = null;
            public static int Counter;

            public void Handle(SampleMessage message)
            {
                Debug.WriteLine("Sample message {0} at {1}",
                    (object)message,
                    (object)DateTime.Now.ToString("hh:mm:ss.fff")
                );
                ApplicationBus.Send(new ChildMessage(string.Format("Child for {0}", message.Text)));
                Counter++;

                if (Counter % 2 == 0)
                    throw new Exception("Shooted by Chaos Monkey!");
            }

            public void Handle(ChildMessage message)
            {
                Debug.WriteLine("Child  message {0} at {1}",
                    (object)message,
                    (object)DateTime.Now.ToString("hh:mm:ss.fff"));
                Counter++;
            }
        }

        public class CommandWithMultipleHandlers : IMessage
        {
        }

        public class FirstCommandHandler : IMessageHandler<CommandWithMultipleHandlers>
        {
            public void Handle(CommandWithMultipleHandlers message)
            {
                throw new NotImplementedException();
            }
        }

        public class SecondCommandHandler : IMessageHandler<CommandWithMultipleHandlers>
        {
            public void Handle(CommandWithMultipleHandlers message)
            {
                throw new NotImplementedException();
            }
        }

        [Test]
        public void all_messages_should_have_been_handled_after_queue_shutdown()
        {
            SampleMessageHandler.Counter = 0;
            using (var bus = new SimpleMessageQueue(200))
            {
                SampleMessageHandler.ApplicationBus = bus;
                bus.RegisterAssemblyOf<SampleMessageHandler>();

                bus.Send(new SampleMessage("Message 1"));
                bus.Send(new SampleMessage("Message 2"));
                bus.Send(new SampleMessage("Message 3"));
                bus.Send(new SampleMessage("Message 4"));
            }

            // 4*2 (parent + child) + 3 * 2 (retries for Chaos Monkey generated failures)
            Assert.AreEqual(8 + 6, SampleMessageHandler.Counter);
        }

        [Test]
        public void sending_a_message_without_handler_should_trow()
        {
            using (var bus = new SimpleMessageQueue(10))
            {
                var ex = Assert.Throws<Exception>(() => bus.Send(new UnHandledMessage()));
                Assert.IsTrue(ex.Message.StartsWith("Missing handler for message"));
            }
        }

        [Test]
        public void publishing_of_message_without_handlers_should_not_throw()
        {
            using (var bus = new SimpleMessageQueue(10))
            {
                Assert.DoesNotThrow(() => bus.Publish(new UnHandledMessage()));
            }
        }

        [Test]
        public void messages_should_have_only_one_handler()
        {
            using (var bus = new SimpleMessageQueue(10))
            {
                bus.RegisterAssemblyOf<SampleMessageHandler>();
                var ex = Assert.Throws<TooManyMessageHandlersException>(() => bus.Send(new CommandWithMultipleHandlers()));
            }
        }
    }
}
