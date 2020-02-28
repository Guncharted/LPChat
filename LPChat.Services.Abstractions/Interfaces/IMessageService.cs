using LPChat.Common.Models;
using LPChat.Domain.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LPChat.Services.Interfaces
{
    public interface IMessageService
    {
        Task<OperationResult> AddMessage(MessageModel messageToAdd);
    }
}