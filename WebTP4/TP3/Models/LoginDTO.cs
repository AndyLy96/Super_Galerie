using System.ComponentModel.DataAnnotations;

namespace TP3.Models
{
    public class LoginDTO
    {
        [Required]
        public string Username { get; set; } = null;


        [Required]
        public string Password { get; set; } = null;
    }
}
