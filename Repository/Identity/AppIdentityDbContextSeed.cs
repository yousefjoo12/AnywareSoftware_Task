using Core.Entities.Identity;
using Core.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;  

namespace Project.Repository.Data.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> _userManager)
        {
            if (_userManager.Users.Count() == 0)
            {
                var user = new AppUser()
                {
                    DisplayName = "Yousef Ayman",
                    Email = " admin@example.com",
                    UserName = "yousef_ayamn",
                    PhoneNumber = "01117811572",
                    RefreshToken = "",
                    RefreshTokenExpiryTime = DateTime.MinValue,
                    UserType = UserType.Admin
                };
                await _userManager.CreateAsync(user, "Admin@123");
            }
        }
    }
}
