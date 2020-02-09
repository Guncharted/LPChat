using LPChat.Domain.Results;
using LPChat.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LPChat.Infrastructure.Interfaces
{
    public interface IMessageService
    {
        List<MessageModel> Messages { get; }

        Task<OperationResult> AddMessage(MessageModel message);
        List<MessageModel> GetMessages(MessageModel lastMessage);
    }
}