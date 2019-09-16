using LPChat.Domain.Entities;
using System;
using System.Collections.Generic;

namespace LPChat.Domain.Interfaces
{
    public interface IMessageService
    {
        List<Message> Messages { get; }

        void AddMessage(Message message);
        List<Message> GetMessages(DateTime? since);
    }
}