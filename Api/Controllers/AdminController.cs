using API.Controllers;
using API.DTOs.Identity;
using API.Erorrs;
using AutoMapper;
using Core.Entities.Identity;
using Core.Enums;
using Core.Services.Contract;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using Service;
using System.Security.Claims;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "Admin")]
    public class AdimnController : BaseApiController
    {
        private readonly IAdminService _adminService;
        private readonly IMapper _mapper;

        public AdimnController(IAdminService adminService, IMapper mapper )
        {
            _adminService = adminService;
            _mapper = mapper;
        } 
        [HttpGet("GetUsers_Active")]
        public async Task<ActionResult<IReadOnlyList<UserResponseDTO>>> GetActiveUsers()
        {
            var users = await _adminService.GetActiveUsersAsync();
            return Ok(_mapper.Map<IReadOnlyList<UserResponseDTO>>(users));
        }
          
        [HttpGet("GetUsers_NotActive")]
        public async Task<ActionResult<IReadOnlyList<UserResponseDTO>>> GetDeletedUsers()
        {
            var users = await _adminService.GetDeletedUsersAsync();
            return Ok(_mapper.Map<IReadOnlyList<UserResponseDTO>>(users));
        } 
       
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var data = await _adminService.CreateUserAsync(
                model.Email, model.DisplayName, model.Password, model.UserType);

            if (!data.Success)
                return BadRequest(new ApiResponse(400, data.Message));

            return Ok(new ApiResponse(201, data.Message));
        }  
        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var data = await _adminService.DeleteUserAsync(id);

            if (!data.Success)
                return NotFound(new ApiResponse(404, data.Message));

            return Ok(new ApiResponse(200, data.Message));
        }
    }
}