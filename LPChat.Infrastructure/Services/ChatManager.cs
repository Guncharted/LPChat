using LPChat.Common.Models;
using LPChat.Domain.Results;
using LPChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
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
        public void AddMessage(MessageModel message)
        {
            _messageService.AddMessage(message);
        }

        public List<MessageModel> GetMessages(MessageModel lastMessage)
        {
            return _messageService.GetMessages(lastMessage);
        }
        #endregion

        #region Chat Management
        public async Task<OperationResult> CreateChat(ChatModel chatForCreate)
        {
            return await _chatService.Create(chatForCreate);
        }

        public void GetChatInfo(Guid chatId)
        {
            _chatService.GetChatInfo(chatId);
        }

        //public async Task<OperationResult> UpdatePersonList(ChatStateViewModel newChatState)
        //{
        //    return await _chatService.UpdatePersonList(newChatState);
        //}
        #endregion
    }
}
