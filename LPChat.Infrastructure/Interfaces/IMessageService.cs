using LPChat.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LPChat.Infrastructure.Interfaces
{
    public interface IMessageService
    {
        List<Message> Messages { get; }

        Task AddMessage(Message message);
        List<Message> GetMessages(Message lastMessage);
    }
}