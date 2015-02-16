using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;
using Rebus;
using Rebus.Castle.Windsor;
using Rebus.Configuration;
using Rebus.Transports.Msmq;

namespace Bookings.Tests.RebusTests
{
    [TestFixture]
    public class RebusPingPongTest
    {
        public class Message
        {
            public string Text { get; set; }
        }

        public class MessageHandler : IHandleMessages<Message>
        {
            public void Handle(Message message)
            {
                Debug.WriteLine("Message received:{0}", (object) message.Text);
            }
        }

        private IWindsorContainer _container;
        private IBus _bus;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _container = new WindsorContainer();
            _container.Register(
                Classes
                    .FromThisAssembly()
                    .BasedOn(typeof(IHandleMessages<>))
                    .WithServiceFirstInterface()
            );

            _bus = Configure
                .With(new WindsorContainerAdapter(_container))
                .Transport(t => t.UseMsmq("rebus.test.input", "rebus.test.error"))
                .MessageOwnership(mo => mo.Use( new MessageOwnership()))
                .CreateBus()
                .Start();
        }

        private string GetQueueName(string q)
        {
            return string.Format(@"{0}\private$\{1}", Environment.MachineName, q);
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            _bus.Dispose();
            _container.Dispose();

            MessageQueue.Delete(GetQueueName("rebus.test.input"));
            MessageQueue.Delete(GetQueueName("rebus.test.error"));
        }

        [Test]
        public void ping_pong()
        {
            _bus.Send(new Message{Text = "Hello!"});
            Thread.Sleep(1000);
        }
    }
}
