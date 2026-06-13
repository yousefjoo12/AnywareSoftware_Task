using Core.Entities.Identity;
using Core.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Contract
{
    public interface IAdminService
    {
        Task<IReadOnlyList<AppUser>> GetActiveUsersAsync();
        Task<IReadOnlyList<AppUser>> GetDeletedUsersAsync();
        Task<(bool Success, string Message)> CreateUserAsync(string email, string displayName, string password, UserType userType);
        Task<(bool Success, string Message)> DeleteUserAsync(string id);

    }
}
