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

            //*********************************************************
            if (await _userManager.FindByEmailAsync("Admin@example.com") == null)
            {
                var user = new AppUser()
                {
                    DisplayName = "Admin",
                    UserName = "Admin@example.com",
                    Email = "Admin@example.com",
                    UserType = UserType.Admin,
                    CreatedAt = DateTime.UtcNow,
                    RefreshTokenExpiryTime = DateTime.MinValue, 
                    IsDeleted = false
                };
                var result = await _userManager.CreateAsync(user, "Admin@123");

                if (result.Succeeded)
                    await _userManager.AddToRoleAsync(user, "Admin");
            }
            //*********************************************************
            if (await _userManager.FindByEmailAsync("Yousef@example.com") == null)
            {
                var user = new AppUser
                {
                    DisplayName = "Yousef Ayman",
                    UserName = "Yousef@example.com",
                    Email = "Yousef@example.com",
                    UserType = UserType.User,
                    CreatedAt = DateTime.UtcNow,
                    RefreshTokenExpiryTime = DateTime.MinValue,
                    IsDeleted = false
                };

                var result = await _userManager.CreateAsync(user, "Yousef@123");

                if (result.Succeeded)
                    await _userManager.AddToRoleAsync(user, "User");
            }
            //*********************************************************
            if (await _userManager.FindByEmailAsync("Salma@example.com") == null)
            {
                var user = new AppUser
                {
                    DisplayName = "Salma Mohmed",
                    UserName = "Salma@example.com",
                    Email = "Salma@example.com",
                    UserType = UserType.User,
                    CreatedAt = DateTime.UtcNow,
                    RefreshTokenExpiryTime = DateTime.MinValue,
                    IsDeleted = false
                };

                var result = await _userManager.CreateAsync(user, "Salma@123");

                if (result.Succeeded)
                    await _userManager.AddToRoleAsync(user, "User");
            }
        }
    }
}
