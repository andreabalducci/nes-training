using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using Bookings.Shared.Domain.BookingContext.RichiestaDiPrenotazione.Events;
using Bookings.Shared.Messaging;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Newtonsoft.Json;
using Rebus;
using Rebus.Castle.Windsor;
using Rebus.Configuration;
using Rebus.MongoDb;
using Rebus.OldLog4Net;
using Rebus.Transports.Msmq;

namespace Bookings.Client.Support
{
    public class BusInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var cstring = ConfigurationManager.ConnectionStrings["bus"].ConnectionString;

            container.Register(
                Component.For<IHandleMessages<ReadModelUpdatedMessage>>()
                         .ImplementedBy<HandlerNotifier>()
                         .LifeStyle.Transient
            );

            container.Register(
                Component.For<IHandleMessages<RichiestaDiPrenotazioneCreata>, IHandleMessages<RichiestaDiPrenotazioneApprovata>>()
                         .ImplementedBy<RichiestaDiPrenotazioneCreataNotifier>()
                         .LifeStyle.Transient
            );

            var bus = Configure.With(new WindsorContainerAdapter(container))
                .Logging(l => l.OldLog4Net())
                .Transport(t => t.UseMsmqAndGetInputQueueNameFromAppConfig())
                .MessageOwnership(d => d.FromRebusConfigurationSection())
                .Timeouts(t => t.StoreInMongoDb(cstring, "client-timeouts"))
                .Subscriptions(s => s.StoreInMongoDb(cstring, "client-subscriptions"))
                .CreateBus().Start();

            bus.Subscribe<ReadModelUpdatedMessage>();
            bus.Subscribe<RichiestaDiPrenotazioneCreata>();
            bus.Subscribe<RichiestaDiPrenotazioneApprovata>();
        }
    }

    public class HandlerNotifier : IHandleMessages<ReadModelUpdatedMessage>
    {
        public void Handle(ReadModelUpdatedMessage message)
        {
            Console.WriteLine("\nReadmodel updated:\n{0}\n", JsonConvert.SerializeObject(message, Formatting.Indented));
        }
    }

    public class RichiestaDiPrenotazioneCreataNotifier : 
        IHandleMessages<RichiestaDiPrenotazioneCreata>,
        IHandleMessages<RichiestaDiPrenotazioneApprovata>
    {
        public void Handle(RichiestaDiPrenotazioneCreata message)
        {
            Console.WriteLine("Richiesta di '{0}' creata!", message.Utente);
        }

        public void Handle(RichiestaDiPrenotazioneApprovata message)
        {
            Console.WriteLine("Richiesta di prenotazione '{0}' approvata!", message.Id);
        }
    }
}
