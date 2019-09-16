using LPChat.Domain.DTO;
using LPChat.Domain.Entities;
using LPChat.Domain.Interfaces;
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

        [HttpPost("updatePersons")]
        public async Task<IActionResult> UpdatePersons(ChatState chatState)
        {
            var result = await _chatservice.UpdatePersonList(chatState);

            return Ok(result);
        }
    }
}