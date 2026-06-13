using Core.Entities.Identity;
using Core.Enums;
using Core.Services.Contract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<AppUser> _userManager;

        public AdminService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IReadOnlyList<AppUser>> GetActiveUsersAsync()
        {
            return await _userManager.Users
                .Where(u => !u.IsDeleted)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<AppUser>> GetDeletedUsersAsync()
        {
            return await _userManager.Users
                .Where(u => u.IsDeleted)
                .ToListAsync();
        }

        public async Task<(bool Success, string Message)> CreateUserAsync(
            string email, string displayName, string password, UserType userType)
        {
            var userExists = await _userManager.FindByEmailAsync(email);
            if (userExists != null)
                return (false, "User already exists");

            var user = new AppUser
            {
                Email = email,
                UserName = email,
                DisplayName = displayName,
                UserType = userType,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return (false, string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, userType.ToString());

            return (true, "User created successfully");
        }

        public async Task<(bool Success, string Message)> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return (false, "User not found");

            if (user.IsDeleted)
                return (false, "User already deleted");

            user.IsDeleted = true;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return (false, "Error while deleting user");

            return (true, "User deleted successfully");
        }
    }
}