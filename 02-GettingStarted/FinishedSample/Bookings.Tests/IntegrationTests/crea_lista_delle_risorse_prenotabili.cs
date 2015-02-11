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
            var pc = new Risorsa(Guid.NewGuid(), "Pc Fisso");
            var portatitle = new Risorsa(Guid.NewGuid(), "Pc Portatile");
            var proiettore = new Risorsa(Guid.NewGuid(), "Proiettore");

            pc.RendiPrenotabile();
            proiettore.RendiPrenotabile();
            portatitle.RendiPrenotabile();
            portatitle.RendiNonPrenotabile();

            pc.Prendi();
            pc.Restituisci();
            proiettore.Prendi();


            Repository.Save(pc, Guid.NewGuid(), null);
            Repository.Save(portatitle, Guid.NewGuid(), null);
            Repository.Save(proiettore, Guid.NewGuid(), null);

            FlushExecutionQueue();
        }
    }
}
