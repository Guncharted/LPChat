using AutoMapper;
using LPChat.Common.Models;
using LPChat.Services;
using LPChat.Services.Interfaces;
using LPChat.Services.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LPChat.Controllers
{
    //TODO. Finalize the auth
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginViewModel userForLogin)
        {
            var user = _mapper.Map<UserSecurityModel>(userForLogin);
            var result = await _authService.LoginAsync(user);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterViewModel userForRegister)
        {
            var user = _mapper.Map<UserSecurityModel>(userForRegister);

            var result = await _authService.RegisterAsync(user);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [Authorize]
        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword(UserPasswordChangeViewModel patch)
        {
            var user = _mapper.Map<UserSecurityModel>(patch);

            var result = await _authService.ChangePasswordAsync(user);
            return Ok();
        }

        [HttpPost("resetPassword")]
        public IActionResult ResetPassword()
        {
            throw new NotImplementedException("Not supported");
        }
    }
}