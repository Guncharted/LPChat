using System;
using System.Threading.Tasks;
using LPChat.Infrastructure.ViewModels;
using LPChat.Domain.Results;

namespace LPChat.Infrastructure.Interfaces
{
	public interface IChatService
	{
		Task<OperationResult> Create(ChatModel chatForCreate);
		void GetChatInfo(Guid chatId);
		Task<OperationResult> UpdatePersonList(ChatStateViewModel newChatState);
	}
}