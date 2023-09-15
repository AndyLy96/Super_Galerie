using System.ComponentModel.DataAnnotations;

namespace TP3.Models
{
    public class RegisterDTO
    {

        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [Required]
        public string ConfirmPassword  { get; set; } = null!;
    }
}
