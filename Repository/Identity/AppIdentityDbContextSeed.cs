using Core.Entities.Identity;
using Core.Enums;
using Microsoft.AspNetCore.Identity; 

namespace Project.Repository.Data.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> _userManager, RoleManager<IdentityRole> _roleManager)
        {

            if (!await _roleManager.RoleExistsAsync("Admin"))
                await _roleManager.CreateAsync(new IdentityRole("Admin"));

            if (!await _roleManager.RoleExistsAsync("User"))
                await _roleManager.CreateAsync(new IdentityRole("User"));


            if (_userManager.Users.Count() == 0)
            {
                var user = new AppUser()
                {
                    DisplayName = "Admin",
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    UserType = UserType.Admin,
                    CreatedAt = DateTime.UtcNow,
                    RefreshTokenExpiryTime = DateTime.MinValue, 
                    IsDeleted = false
                };
                var result = await _userManager.CreateAsync(user, "Admin@123");

                if (result.Succeeded)
                    await _userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}
