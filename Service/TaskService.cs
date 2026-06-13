using Core;
using Core.Entities;
using Core.Enums;
using Core.Services.Contract;

namespace Service
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TaskService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
         
        public async Task<IReadOnlyList<Tasks>> GetUserTasksAsync(string userId)
        {
            var tasks = await _unitOfWork.Repository<Tasks>().GetAllWithFilterAsync(t => t.UserId == userId);

            return tasks
                .OrderByDescending(t => t.Priority)
                .ThenBy(t => t.CreatedAt)
                .ToList();
        }

        public async Task<Tasks?> GetTaskByIdAsync(int id, string userId)
        {
            var task = await _unitOfWork.Repository<Tasks>().GetById(id);

            if (task == null || task.UserId != userId)
                return null;

            return task;
        }

        public async Task<(bool Success, string Message, Tasks? Task)> CreateTaskAsync(string title, string description, Priority priority, string userId)
        { 
            var today = DateTime.UtcNow.Date;

            var existing = await _unitOfWork.Repository<Tasks>().GetAllWithFilterAsync(T => T.UserId == userId && T.Title.ToLower() == title.ToLower() && T.CreatedAt.Date == today);

            if (existing.Any())
                return (false, "Task with same title already exists today", null);

            var task = new Tasks
            {
                Title = title,
                Description = description,
                Priority = priority,
                Status = Status.Pending,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<Tasks>().AddAsync(task);
            await _unitOfWork.CompleteAsync();

            return (true, "Task created successfully", task);
        }

        public async Task<(bool Success, string Message)> UpdateTaskStatusAsync(int id, Status status, string userId)
        {
            var task = await _unitOfWork.Repository<Tasks>().GetById(id);

            if (task == null || task.UserId != userId)
                return (false, "Task not found");

            task.Status = status;

            await _unitOfWork.Repository<Tasks>().UpdateAsync(task);
            await _unitOfWork.CompleteAsync();

            return (true, "Status updated successfully");
        } 
    }
}