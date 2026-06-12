using Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Identity
{
    public class CreateUserDTO
    {
        [Required] 
        public string DisplayName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]

        public UserType UserType { get; set; }
        [Required]
        public string Password { set; get; }
    }
}
