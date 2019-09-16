using System;
using System.Threading.Tasks;
using LPChat.Domain.DTO;
using LPChat.Domain.Results;

namespace LPChat.Domain.Interfaces
{
	public interface IChatService
	{
		Task<OperationResult> Create(ChatForCreate chatForCreate);
		void GetChatInfo(Guid chatId);
		Task<OperationResult> UpdatePersonList(ChatState newChatState);
	}
}