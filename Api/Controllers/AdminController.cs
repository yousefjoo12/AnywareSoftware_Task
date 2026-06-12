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
using System.Security.Claims;

namespace API.Controllers
{
    public class AdimnController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public AdimnController(UserManager<AppUser> userManager,IMapper mapper )
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
        [Authorize(Roles = "Admin")]
        [HttpGet("GetUsers_Active")]
        public async Task<ActionResult<UserDTO>> GetUsers_Active()
        {
            var users = await _userManager.Users.Where(X =>X.IsDeleted == false).ToListAsync();
            var  mapUser = _mapper.Map<List<UserResponseDTO>>(users);
            return Ok(mapUser);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        [HttpGet("GetUsers_NotActive")]
        public async Task<ActionResult<UserDTO>> GetUsers_NotActive()
        {
            var users = await _userManager.Users.Where(X => X.IsDeleted == true).ToListAsync();
            var mapUser = _mapper.Map<List<UserResponseDTO>>(users);
            return Ok(mapUser);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null) 
            return BadRequest(new ApiResponse(400, "User already exists"));


            var user = new AppUser
            {
                Email = model.Email,
                UserName = model.Email,
                DisplayName = model.DisplayName,
                UserType = model.UserType,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400, "Erorr while Create User"));

            // Assign Role
            await _userManager.AddToRoleAsync(user, model.UserType.ToString());

            return Ok(new
            {
                message = "User created successfully",
                userId = user.Id
            });
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser( string id)
        {
            var user = await _userManager.FindByIdAsync(id); 
            if (user == null)
                return NotFound(new ApiResponse(404, "User not found"));

            user.IsDeleted = true;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400, "Erorr while deleting"));

            return Ok(new ApiResponse(200, "User deleted successfully"));
        }

    }
}