using API.Controllers;
using API.DTOs.Identity;
using API.Erorrs;
using Core.Entities.Identity;
using Core.Enums;
using Core.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using System.Security.Claims;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthService _authService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IAuthService authService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return Unauthorized(new ApiResponse(401, "Invalid email or password"));

            // Soft Delete
            if (user.IsDeleted)
                return Unauthorized(new ApiResponse(401, "Account has been deactivated"));

            var checkPassword = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!checkPassword.Succeeded)
                return Unauthorized(new ApiResponse(401, "Invalid email or password"));

            var tokens = await _authService.CreateTokenAsync(user, _userManager);

            return Ok(new UserDTO
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                UserType = user.UserType.ToString(),
                Token = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken
            });
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) != null)
                return BadRequest(new ApiResponse(400, "Email already exists"));

            var user = new AppUser
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
                UserType = UserType.User,
                CreatedAt = DateTime.UtcNow
                
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest(new ApiValidationErrorResponse
                {
                    Errors = result.Errors.Select(e => e.Description).ToArray()
                });

            await _userManager.AddToRoleAsync(user, "User");

            var tokens = await _authService.CreateTokenAsync(user, _userManager);

            return Ok(new UserDTO
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                UserType = user.UserType.ToString(),
                Token = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken
            });
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<UserDTO>> RefreshToken(TokenRequestDTO tokenRequest)
        {
            var user = await _userManager.FindByEmailAsync(tokenRequest.Email);
            if (user == null || user.RefreshToken != tokenRequest.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return Unauthorized(new ApiResponse(401, "Invalid or expired refresh token"));

            var tokens = await _authService.CreateTokenAsync(user, _userManager);

            return Ok(new UserDTO
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                UserType = user.UserType.ToString(),
                Token = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken
            });
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email)) return Unauthorized();

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return NotFound();

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = DateTime.MinValue;

            var result = await _userManager.UpdateAsync(user); 
            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400, "Logout failed"));
            return Ok(new ApiResponse(200, "Logged out successfully")); 
        }

    }
}