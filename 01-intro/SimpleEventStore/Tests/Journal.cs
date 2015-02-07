using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CqrsEsSample.Domain;

namespace CqrsEsSample.Tests
{
    public class Journal : Dictionary<Guid, JournalItem>
    {
        public JournalItem GetOrCreateItem(Guid id)
        {
            JournalItem ji;
            TryGetValue(id, out ji);
            if (ji == null)
            {
                ji = new JournalItem();
                this[id] = ji;
            }
            return ji;
        }
    }

    public class JournalItem
    {
        public string Code { get; set; }

        public string Description { get; set; }

        public decimal Total { get; set; }
    }

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

    public class Articolo
    {
        public Guid Id { get; set; }
        public string Codice { get; set; }
        public string Descrizione { get; set; }
    }

    public class PrelievoFallito
    {
        public Guid Id { get; set; }
        public string Codice { get; set; }
        public string Descrizione { get; set; }
        public decimal QuantitàRichiesta { get; set; }

    }
}
