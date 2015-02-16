using System;
using Bookings.Domain.BookingContenxt;
using NUnit.Framework;

namespace Bookings.Tests.IntegrationTests
{
// ReSharper disable InconsistentNaming
    public class crea_lista_delle_risorse_prenotabili :AbstractIntegrationTest
    {
        [Test]
        public void crea_lista()
        {
            var pc = new Resource(Guid.NewGuid(), "Pc Fisso");
            var portatitle = new Resource(Guid.NewGuid(), "Pc Portatile");
            var proiettore = new Resource(Guid.NewGuid(), "Proiettore");

            //pc.MakeAvailable();
            //proiettore.MakeAvailable();
            //portatitle.MakeAvailable();
            //portatitle.MakeUnavailable();

            //pc.Lend();
            //pc.Return();
            //proiettore.Lend();

            Repository.Save(pc, Guid.NewGuid(), null);
            Repository.Save(portatitle, Guid.NewGuid(), null);
            Repository.Save(proiettore, Guid.NewGuid(), null);

            FlushExecutionQueue();
        }
    }
}
