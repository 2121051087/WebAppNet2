using System.ComponentModel.DataAnnotations;

namespace WebAppNet2.Models.Auth
{
    public class LogInVM
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required, MinLength(6)]
        public string Password { get; set; } = null!;
    }
}
