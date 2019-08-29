using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LPChat.Core.DTO;
using LPChat.Core.Interfaces;
using LPChat.Core.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LPChat.Controllers
{
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
            if (!ModelState.IsValid)
                return BadRequest(new OperationResult(false, "Bad credentials"));

            var result = await _authService.Login(userForLogin);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegister userForRegister)
        {
            if (!ModelState.IsValid)
                return BadRequest(new OperationResult(false, "Bad credentials"));

            var result = await _authService.Register(userForRegister);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}