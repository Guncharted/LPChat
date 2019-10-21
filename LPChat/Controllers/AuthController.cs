using LPChat.Domain.DTO;
using LPChat.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LPChat.Controllers
{
    //TODO. Finalize the auth
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLogin userForLogin)
        {
            var result = await _authService.LoginAsync(userForLogin);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegister userForRegister)
        {
            var result = await _authService.RegisterAsync(userForRegister);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [Authorize]
        public async Task<IActionResult> ChangePassword(UserPasswordChange user)
        {
            var requestorId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var result = await _authService.ChangePasswordAsync(user, requestorId);
            return Ok();
        }

        public IActionResult ResetPassword()
        {
            return Ok();
        }
    }
}