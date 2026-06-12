using Core.Enums;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities.Identity
{ 
    public class AppUser : IdentityUser
    { 
        public string DisplayName { get; set; }
        public UserType UserType { get; set; }
        public DateTime CreatedAt { get; set; } 
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        // is deleted for softDeleting
        public bool IsDeleted { get; set; } = false;
    }
}