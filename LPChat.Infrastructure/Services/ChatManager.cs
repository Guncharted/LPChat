using LPChat.Domain.DTO;
using LPChat.Domain.Entities;
using LPChat.Domain.Interfaces;
using LPChat.Domain.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LPChat.Infrastructure.Services
{
    public class ChatManager
    {
        private readonly IChatService _chatService;
        private readonly IMessageService _messageService;

        public ChatManager(IChatService chatService, IMessageService messageService)
        {
            _chatService = chatService;
            _messageService = messageService;
        }

        #region Messaging
        public void AddMessage(Message message)
        {
            _messageService.AddMessage(message);
        }

        public List<Message> GetMessages(Message lastMessage)
        {
            return _messageService.GetMessages(lastMessage);
        }
        #endregion

        #region Chat Management
        public async Task<OperationResult> CreateChat(ChatForCreate chatForCreate)
        {
            return await _chatService.Create(chatForCreate);
        }

        public void GetChatInfo(Guid chatId)
        {
            _chatService.GetChatInfo(chatId);
        }

        public async Task<OperationResult> UpdatePersonList(ChatState newChatState)
        {
            return await _chatService.UpdatePersonList(newChatState);
        }
        #endregion
    }
}
