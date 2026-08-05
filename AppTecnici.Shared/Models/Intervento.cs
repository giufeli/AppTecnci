using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AppTecnici.Shared.Models
{
    public class Intervento
    {
        public int Id { get; set; }
        public int ImpiantoId { get; set; }
        public string Titolo { get; set; } = string.Empty;
        public string Descrizione { get; set; } = string.Empty;
        public DateTime DataIntervento { get; set; } = DateTime.Now;
        public string Stato { get; set; } = "In attesa";
        public bool Sincronizzato { get; set; } = true;

        [NotMapped]
        [JsonIgnore]
        public bool IsNuovoOffline { get; set; } = false;
    }
}