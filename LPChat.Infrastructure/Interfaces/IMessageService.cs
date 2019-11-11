using LPChat.Domain.Entities;
using LPChat.Infrastructure.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LPChat.Infrastructure.Interfaces
{
	public interface IMessageService
    {
        List<MessageViewModel> Messages { get; }

        Task AddMessage(MessageViewModel message);
        List<MessageViewModel> GetMessages(MessageViewModel lastMessage);
    }
}