using LPChat.Core.DTO;
using LPChat.Core.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LPChat.Core.Interfaces
{
    public interface IChatManagerService
    {
        Task<OperationResult> GetChatList(Guid personId);
        Task<OperationResult> Create(ChatForCreate chatForCreate);
        void GetChatInfo(Guid chatId);
        Task<OperationResult> UpdatePersonList(ChatState newChatState);
    }
}
