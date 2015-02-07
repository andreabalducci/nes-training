using System.Collections.Generic;

namespace SimpleEventStore.Tests
{
    public class ElencoArticoli
    {
        public List<Articolo> Articoli { get; set; }
        public List<Articolo> ArticoliSottoScorta { get; set; }
        public List<PrelievoFallito> PrelieviFalliti { get; set; }

        public ElencoArticoli()
        {
            Articoli = new List<Articolo>();
            ArticoliSottoScorta = new List<Articolo>();
            PrelieviFalliti = new List<PrelievoFallito>();
        }
    }
}