using Core.Entities;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Repository.Data
{
    public static class StoreContextSeed
    {
        public async static Task SeedAsync(StoreContext _dbcontext, UserManager<AppUser> _userManager)
        {
            var options = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() }
            };

            var tasksJson = File.ReadAllText("../Repository/Data/DataSeeding/tasksD.json");

            var tasksData = JsonSerializer.Deserialize<List<Tasks>>(tasksJson, options);

            if (tasksData == null || !tasksData.Any())
                return;

            if (_dbcontext.Tasks.Any())
                return;

            foreach (var task in tasksData)
            { 
                var user = await _userManager.FindByEmailAsync(task.UserId);

                if (user == null)
                    continue; 

                var newTask = new Tasks()
                {
                    Title = task.Title,
                    Description = task.Description,
                    Priority = task.Priority,
                    Status = task.Status,
                    CreatedAt = DateTime.Now,
                    UserId = user.Id,
                    UserName = task.UserId
                };

                _dbcontext.Tasks.Add(newTask);
            }

            await _dbcontext.SaveChangesAsync();
        }
    }
}