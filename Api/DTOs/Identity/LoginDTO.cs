using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Identity
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { set; get; }

        [Required]
        public string Password { set; get; }

    }
}
