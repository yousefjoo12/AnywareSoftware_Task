using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Identity
{
    public class RegisterDTO
    {
        [Required]
        public string DisplayName { set; get; }
        [Required]
        [EmailAddress]
        public string Email { set; get; }
        [Required]
        public string PhoneNumber { set; get; }
        [Required]
        public string Password { set; get; }
        [Required]
        public string UserType { get; set; }
    }
}
