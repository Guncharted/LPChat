using LPChat.Infrastructure.ViewModels;
using LPChat.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using LPChat.Helpers;

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
        public async Task<IActionResult> Login(UserLoginViewModel userForLogin)
        {
            var result = await _authService.LoginAsync(userForLogin);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterViewModel userForRegister)
        {
            var result = await _authService.RegisterAsync(userForRegister);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [Authorize]
		[HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword(UserPasswordChangeViewModel user)
        {
            var requestorId = User.GetPersonId();

            var result = await _authService.ChangePasswordAsync(user, requestorId);
            return Ok();
        }

		[HttpPost("resetPassword")]
        public IActionResult ResetPassword()
        {
            return Ok();
        }
    }
}