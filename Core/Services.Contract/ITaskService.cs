using Core.Entities;
using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Contract
{
   public interface ITaskService
    {
        Task<IReadOnlyList<Tasks>> GetUserTasksAsync(string userId);
        Task<Tasks?> GetTaskByIdAsync(int id, string userId);
        Task<(bool Success, string Message, Tasks? Task)> CreateTaskAsync(string title, string description, Priority priority, string userId);
        Task<(bool Success, string Message)> UpdateTaskStatusAsync(int id, Status status, string userId);
    }
}
