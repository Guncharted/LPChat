using System;
using System.Collections.Generic;
using LPChat.Core.Entities;

namespace LPChat.Core.Interfaces
{
    public interface IMessageService
    {
        List<Message> Messages { get; }

        void AddMessage(Message message);
        List<Message> GetMessages(DateTime? since);
    }
}