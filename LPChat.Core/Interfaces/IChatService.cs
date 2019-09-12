using System;
using System.Threading.Tasks;
using LPChat.Core.DTO;
using LPChat.Core.Results;

namespace LPChat.Core.Interfaces
{
	public interface IChatService : IMessageService, IChatManagerService
	{
	}
}