using System.Text.Json.Serialization;
using TP3.Data;

namespace TP3.Models
{
    public class Galerie
    {
        private TP3Context context;

        

        public string Name { get; set; }
        public bool IsPublic { get; set; }
        public int Id { get; set; }
        public string? FileName { get; set; }
        public string? MimeType { get; set; }
        [JsonIgnore]
        public virtual List<User>? User { get; set; }

        public virtual List<PhotoImage>? PhotoImages { get; set; }


    }
}
