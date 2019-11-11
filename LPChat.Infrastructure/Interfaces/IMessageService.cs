using LPChat.Domain.Entities;
using LPChat.Domain.Results;
using LPChat.Infrastructure.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LPChat.Infrastructure.Interfaces
{
	public interface IMessageService
    {
        List<MessageViewModel> Messages { get; }

        Task<OperationResult> AddMessage(MessageViewModel message);
        List<MessageViewModel> GetMessages(MessageViewModel lastMessage);
    }
}