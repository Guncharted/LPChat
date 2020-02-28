using AutoMapper;
using LPChat.Common.Models;
using LPChat.Services;
using LPChat.Services.Interfaces;
using LPChat.Services.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LPChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IChatService _chatservice;
        private readonly IMapper _mapper;
        private readonly IInstantMessagingService _instantMessagingService;

        public ChatController(IMessageService messageService,
                              IChatService chatService,
                              IInstantMessagingService instantMessagingService,
                              IMapper mapper)
        {
            _messageService = messageService;
            _chatservice = chatService;
            _mapper = mapper;
            _instantMessagingService = instantMessagingService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddMessage(MessageViewModel messageViewModel)
        {
            var message = _mapper.Map<MessageModel>(messageViewModel);
            messageViewModel.PersonId = User.GetPersonId();
            await _messageService.AddMessage(message);
            return Ok();
        }

        [HttpGet("poll")]
        public IActionResult Poll(MessageViewModel messageViewModel)
        {
            var message = _mapper.Map<MessageModel>(messageViewModel);
            var result = _instantMessagingService.GetMessages(message);

            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateNewChat(ChatCreateViewModel chatForCreate)
        {
            var chat = _mapper.Map<ChatModel>(chatForCreate);
            var result = await _chatservice.Create(chat);

            return Ok(result);
        }

        [HttpPost("updatePersons")]
        public async Task<IActionResult> UpdatePersons(ChatStateViewModel chatState)
        {
            var chat = _mapper.Map<ChatModel>(chatState);
            var result = await _chatservice.Update(chat);

            return Ok(result);
        }
    }
}