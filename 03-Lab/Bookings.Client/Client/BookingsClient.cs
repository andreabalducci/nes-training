using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Bookings.Shared.Domain.BookingContext.BookableItem;
using Bookings.Shared.Domain.BookingContext.BookableItem.Commands;
using Bookings.Shared.Messaging;
using Bookings.Shared.Projections;
using Rebus;

namespace Bookings.Client.Client
{
    class BookingsClient : IClient, IHandleMessages<ReadModelUpdatedMessage>
    {
        private readonly IBus _bus;
        private readonly IReadModel _readModel;
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

                    case "cb":
                        CreateBookableItemBatch();
                        break;

                    case "ls":
                        ListBookableItems();
                        break;

                    case "del":
                        DeleteBookableItem();
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

        private void CreateBookableItem()
        {
            Console.WriteLine();
            Console.Write("Description (empty cancel): ");
            var description = Console.ReadLine().Trim();
            if (string.IsNullOrWhiteSpace(description))
                return;

            _bus.Send(new CreateBookableItem(new BookableItemId(Guid.NewGuid()), description));
        }

        private void CreateBookableItemBatch()
        {
            Console.WriteLine();
            Console.Write("Description (empty cancel): ");
            var description = Console.ReadLine().Trim();
            if (string.IsNullOrWhiteSpace(description))
                return;
            for (int i = 0; i < 10000; i++)
            {
                _bus.Send(new CreateBookableItem(new BookableItemId(Guid.NewGuid()), description));
            }
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
            Console.WriteLine(" del     -> delete item");
            Console.WriteLine(" ls      -> list BookableItem(s)");
            Console.WriteLine(" q       -> Quit");

            Console.WriteLine(" <enter> -> Clear screen");
            Console.WriteLine();
            Console.WriteLine("==================================================");
        }

        public void Handle(ReadModelUpdatedMessage message)
        {
            /* push updates */
        }
    }
}
