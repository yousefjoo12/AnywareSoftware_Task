using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Repository.Data
{
    public static class StoreContextSeed
    {
        public async static Task SeedAsync(StoreContext _dbcontext)
        {
            var options = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() }  
            };
            var Tasksjson = File.ReadAllText("../Repository/Data/DataSeeding/tasksD.json");
            var TasksData = JsonSerializer.Deserialize<List<Tasks>>(Tasksjson, options);

            if (TasksData.Count() > 0)
            {
                if (_dbcontext.Tasks.Count() == 0)
                {
                    TasksData = TasksData.Select(b => new Tasks()
                    {
                        Description = b.Description,
                        Title = b.Title,
                        Priority = b.Priority,
                        UserId = b.UserId,
                        CreatedAt = DateTime.Now,
                        Status = b.Status
                    }).ToList();

                    foreach (var tasks in TasksData)
                    {
                        _dbcontext.Set<Tasks>().Add(tasks);
                    }
                    await _dbcontext.SaveChangesAsync();
                }
            }


        }
    }
}
