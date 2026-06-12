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
        [HttpGet("GetUsers")]
        public async Task<ActionResult<UserDTO>> GetUsers()
        {
            var users = await _userManager.Users.Where(X =>X.IsDeleted == false).ToListAsync();
            var  mapUser = _mapper.Map<List<UserResponseDTO>>(users);
            return Ok(mapUser);
        }
    }
}