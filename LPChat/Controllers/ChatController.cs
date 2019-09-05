using LPChat.Core.DTO;
using LPChat.Core.Entities;
using LPChat.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LPChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IMessageService _messageService;
		private readonly IChatService _chatservice;

        public ChatController(IMessageService messageService, IChatService chatService)
        {
            _messageService = messageService;
			_chatservice = chatService;
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
        public async Task<IActionResult> CreateNewChat(ChatForCreate chatForCreate)
        {
			var result = await _chatservice.Create(chatForCreate);

			return Ok(result);
		}

        [HttpPost("addusers")]
        public IActionResult AddUsersToChat()
        {
            return Ok();
        }
    }
}