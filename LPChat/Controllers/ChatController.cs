using LPChat.Core.Entities;
using LPChat.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LPChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public ChatController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost("add")]
        public IActionResult AddMessage(Message message)
        {
            _messageService.AddMessage(message);
            return Ok();
        }

        [HttpGet("poll")]
        public IActionResult Poll(DateTime? since = null)
        {
            var result = _messageService.GetMessages(since);

            return Ok(result);
        }

        [HttpPost("create")]
        public IActionResult CreateNewChat()
        {
            return Ok();
        }

        [HttpPost("addusers")]
        public IActionResult AddUsersToChat()
        {
            return Ok();
        }
    }
}