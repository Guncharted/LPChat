using LPChat.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPChat.Services.Interfaces
{
    public interface IInstantMessagingService
    {
        List<MessageModel> Messages { get; }

        void AddMessage(MessageModel message);
        List<MessageModel> GetMessages(MessageModel lastMessage);
    }
}
