using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AppTecnici.Shared.Models
{
    public class Impianto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Indirizzo { get; set; } = string.Empty;
        public string Descrizione { get; set; } = string.Empty;
        public string? CartinaBase64 { get; set; }
        public bool Sincronizzato { get; set; } = true;

        [NotMapped] // [NotMapped] dice a EF Core di NON creare/cercare la colonna nel DB SQL
        [JsonIgnore] // [JsonIgnore] la ignora durante il passaggio dati API
        public bool IsNuovoOffline { get; set; } = false;
    }
}