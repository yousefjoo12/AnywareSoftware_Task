using Api.DTOs; 
using API.Erorrs;
using AutoMapper; 
using Core.Services.Contract;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TaskController : BaseApiController
    {
        private readonly ITaskService _taskService;
        private readonly IResponseCacheService _cacheService;
        private readonly IMapper _mapper;

        public TaskController(ITaskService taskService,IResponseCacheService cacheService, IMapper mapper)
        {
            _taskService = taskService;
            _cacheService = cacheService;
            _mapper = mapper;
        }
         
        [HttpGet]
        public async Task<IActionResult> GetMyTasks()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var tasks = await _taskService.GetUserTasksAsync(userId);
            return Ok(_mapper.Map<IReadOnlyList<TaskResponseDTO>>(tasks));
        } 
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cacheKey = $"task_{id}_{userId}";

            // 1. Check Redis
            var cached = await _cacheService.GetCachedResponseAsync(cacheKey);
            if (cached != null)
            {
                var cachedTask = JsonSerializer.Deserialize<TaskResponseDTO>(cached,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return Ok(cachedTask);
            }

            // 2. من DB
            var task = await _taskService.GetTaskByIdAsync(id, userId);
            if (task == null)
                return NotFound(new ApiResponse(404, "Task not found"));

            var mapped = _mapper.Map<TaskResponseDTO>(task);

            // 3. Save في Redis
            await _cacheService.CacheResponseAsync(cacheKey, mapped, TimeSpan.FromMinutes(10));

            return Ok(mapped);
        } 
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var (success, message, task) = await _taskService.CreateTaskAsync(
                model.Title, model.Description, model.Priority, userId);

            if (!success)
                return BadRequest(new ApiResponse(400, message));

            return Ok(_mapper.Map<TaskResponseDTO>(task));
        }
        [HttpPatch("status/{id}")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateTaskStatusDTO model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var (success, message) = await _taskService.UpdateTaskStatusAsync(id, model.Status, userId);

            if (!success)
                return NotFound(new ApiResponse(404, message));
 
            await _cacheService.RemoveCacheAsync($"task_{id}_{userId}");

            return Ok(new ApiResponse(200, message));
        }
    }
}