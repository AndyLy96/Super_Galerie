using System.Text.Json.Serialization;

namespace TP3.Models
{
    public class PhotoImage
    {

        public int Id { get; set; }
        public string? FileName { get; set; }
        public string? MimeType { get; set; }

        [JsonIgnore]
        public virtual Galerie? Galerie { get; set; }

    }
}
