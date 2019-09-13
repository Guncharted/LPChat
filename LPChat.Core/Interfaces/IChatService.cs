using System;
using System.Threading.Tasks;
using LPChat.Core.DTO;
using LPChat.Core.Results;

namespace LPChat.Core.Interfaces
{
	public interface IChatService
	{
		Task<OperationResult> Create(ChatForCreate chatForCreate);
		void GetChatInfo(Guid chatId);
		Task<OperationResult> UpdatePersonList(ChatState newChatState);
	}
}