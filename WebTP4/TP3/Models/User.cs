using Microsoft.AspNetCore.Identity;

namespace TP3.Models
{
    public class User : IdentityUser
    {
        public virtual List<Galerie> Galeries { get; set; } = null!;

    }
}
