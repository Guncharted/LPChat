using LPChat.Infrastructure.ViewModels;
using LPChat.Domain.Entities;
using LPChat.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Security.Claims;

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
        public async Task<IActionResult> AddMessage(MessageViewModel message)
        {
            var authorId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            message.PersonId = authorId;
            await _messageService.AddMessage(message);
            return Ok();
        }

        [HttpGet("poll")]
        public IActionResult Poll(MessageViewModel lastMessage)
        {
            var result = _messageService.GetMessages(lastMessage);

            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateNewChat(ChatCreateViewModel chatForCreate)
        {
			var result = await _chatservice.Create(chatForCreate);

			return Ok(result);
		}

        [HttpPost("updatePersons")]
        public async Task<IActionResult> UpdatePersons(ChatStateViewModel chatState)
        {
            var result = await _chatservice.UpdatePersonList(chatState);

            return Ok(result);
        }
    }
}