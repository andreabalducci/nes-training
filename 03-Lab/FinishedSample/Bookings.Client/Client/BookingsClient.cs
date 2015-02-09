using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Bookings.Shared.Domain.BookingContext.BookableItem;
using Bookings.Shared.Domain.BookingContext.BookableItem.Commands;
using Bookings.Shared.Domain.BookingContext.RichiestaDiPrenotazione;
using Bookings.Shared.Domain.BookingContext.RichiestaDiPrenotazione.Commands;
using Bookings.Shared.Messaging;
using Bookings.Shared.Projections;
using Rebus;

namespace Bookings.Client.Client
{
    public interface IClient
    {
        void Run();
    }

    class BookingsClient : IClient, IHandleMessages<ReadModelUpdatedMessage>
    {
        private readonly IBus _bus;
        private readonly IReadModel _readModel;
        private int _counter;
        private Stopwatch _timer;
        public BookingsClient(IBus bus, IReadModel readModel)
        {
            _bus = bus;
            _readModel = readModel;
        }

        public void Run()
        {
            DisplayMenu();
            while (true)
            {
                Console.Write("Please enter a command: ");
                var cmd = Console.ReadLine().Trim().ToLowerInvariant();
                switch (cmd)
                {
                    case "c":
                        CreateBookableItem();
                        break;

                    case "b":
                        StartBatch();
                        break;

                    case "ls":
                        ListBookableItems();
                        break;

                    case "lsr":
                        ListRichiesteDiPrenotazione();
                        break;

                    case "del":
                        DeleteBookableItem();
                        break;

                    case "r":
                        RichiediNuovaPrenotazione();
                        break;

                    case "a":
                        ApprovaPrenotazione();
                        break;

                    case "q":
                        return;

                    case "":
                        DisplayMenu();
                        break;

                    default:
                        Console.WriteLine("Unknown command");
                        break;
                }
            }
        }

        private void ApprovaPrenotazione()
        {
            var list = _readModel.ListRichiesteDiPrenotazione();
            PrintListRichiesteDiPrenotazione(list);

            while (true)
            {
                Console.WriteLine();
                Console.Write("Selezionare la richiesta di prenotazione [1-{0}] (0 to cancel): ", list.Count);
                var input = Console.ReadLine().Trim();
                int idx;
                if (int.TryParse(input, out idx))
                {
                    if (idx == 0)
                        return;

                    if (idx >= 1 && idx <= list.Count)
                    {
                        var itemId = list[idx - 1].Id;

                        _bus.Send(new RichiediApprovazionePrenotazione(new RichiestaDiPrenotazioneId(itemId)));
                        return;
                    }
                }
            }
        }

        private void RichiediNuovaPrenotazione()
        {
            Console.WriteLine();
            Console.Write("Utente (empty cancel): ");
            var utente = Console.ReadLine().Trim();
            if (string.IsNullOrWhiteSpace(utente))
                return;

            Console.Write("Causale (empty cancel): ");
            var motivazione = Console.ReadLine().Trim();
            if (string.IsNullOrWhiteSpace(motivazione))
                return;

            DateTime da = DateTime.Today.AddDays(1);
            DateTime a = DateTime.Today.AddDays(3);

            var list = _readModel.ListItems();
            PrintList(list);

            while (true)
            {
                Console.WriteLine();
                Console.Write("Delete item nr [1-{0}] (0 to cancel): ", list.Count);
                var input = Console.ReadLine().Trim();
                int idx;
                if (int.TryParse(input, out idx))
                {
                    if (idx == 0)
                        return;

                    if (idx >= 1 && idx <= list.Count)
                    {
                        var itemId = list[idx - 1].Id;
                        _bus.Send(new CreaRichiestaDiPrenotazione(new RichiestaDiPrenotazioneId(), new BookableItemId(itemId), motivazione, utente, da, a));
                        return;
                    }
                }
            }
        }
        
        private void StartBatch()
        {
            _counter = 0;
            _timer = new Stopwatch();
            _timer.Start();

            for (int c = 1; c <= 100; c++)
            {
                _bus.Send(new CreateBookableItem(new BookableItemId(Guid.NewGuid()), "Articolo " + c + "+backup"));
            }
        }

        private void ListBookableItems()
        {
            var list = _readModel.ListItems();
            PrintList(list);
        }

        private void PrintList(IList<BookableItemInListReadModel> list)
        {
            if (list.Count == 0)
            {
                Console.WriteLine("No items sorry.");
                return;
            }
            for (int c = 0; c < list.Count; c++)
            {
                var item = list[c];
                Console.WriteLine("{0} | {1} | {2}", c + 1, item.Id, item.Description);
            }
        }

        private void ListRichiesteDiPrenotazione()
        {
            var list = _readModel.ListRichiesteDiPrenotazione();
            PrintListRichiesteDiPrenotazione(list);
        }

        private void PrintListRichiesteDiPrenotazione(IList<ListaRichiesteDiPrenotazioneReadModel> list)
        {
            if (list.Count == 0)
            {
                Console.WriteLine("No richieste sorry.");
                return;
            }
            for (int c = 0; c < list.Count; c++)
            {
                var item = list[c];
                Console.WriteLine("{0} | {1} | {2} | {3} | {4} | {5} | {6}", c + 1, item.Utente, item.BookableItemDescription, item.Da, item.A, item.Causale, item.Stato);
            }
        }

        private void CreateBookableItem()
        {
            Console.WriteLine();
            Console.Write("Description (empty cancel): ");
            var description = Console.ReadLine().Trim();
            if (string.IsNullOrWhiteSpace(description))
                return;

            _bus.Send(new CreateBookableItem(new BookableItemId(Guid.NewGuid()), description));
        }

        private void DeleteBookableItem()
        {
            var list = _readModel.ListItems();
            PrintList(list);

            while (true)
            {
                Console.WriteLine();
                Console.Write("Delete item nr [1-{0}] (0 to cancel): ", list.Count);
                var input = Console.ReadLine().Trim();
                int idx;
                if (int.TryParse(input, out idx))
                {
                    if (idx == 0)
                        return;

                    if (idx >= 1 && idx <= list.Count)
                    {
                        var itemId = list[idx - 1].Id;
                        _bus.Send(new DeleteBookableItem(itemId));
                        return;
                    }
                }
            }
        }

        private void DisplayMenu()
        {
            Console.Clear();

            Console.WriteLine("==================================================");
            Console.WriteLine(" Bookings Client ");
            Console.WriteLine("==================================================");
            Console.WriteLine(" c       -> Create new BookableItem");
            Console.WriteLine(" ls      -> list BookableItem(s)");
            Console.WriteLine(" del     -> delete item");
            Console.WriteLine(" lsr     -> list richieste prenotazione");
            Console.WriteLine(" a       -> approva prenotazione");
            Console.WriteLine(" q       -> Quit");

            Console.WriteLine(" r       -> Richiedi nuova prenotazione");

            Console.WriteLine(" <enter> -> Clear screen");
            Console.WriteLine();
            Console.WriteLine("==================================================");
        }

        public void Handle(ReadModelUpdatedMessage message)
        {
            Interlocked.Increment(ref _counter);
            if (_counter == 200)
            {
                _timer.Stop();

                Console.WriteLine("Finito in " + _timer.ElapsedMilliseconds + "ms");
            }
        }
    }
}
