using LPChat.Common.Models;
using LPChat.Domain.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LPChat.Infrastructure.Interfaces
{
    public interface IMessageService
    {
        List<MessageModel> Messages { get; }

        Task<OperationResult> AddMessage(MessageModel messageToAdd);
        List<MessageModel> GetMessages(MessageModel lastMessage);
    }
}