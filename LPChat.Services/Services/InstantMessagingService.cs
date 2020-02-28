using LPChat.Common.Models;
using LPChat.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LPChat.Services
{
    public class InstantMessagingService : IInstantMessagingService
    {
        //TODO. To be removed and changed to memory cache
        private List<MessageModel> _messages;
        public List<MessageModel> Messages => _messages ??= new List<MessageModel>();

        private readonly object threadLock = new object();

        public void AddMessage(MessageModel message)
        {
            lock (threadLock)
            {
                Messages.Add(message);
            }
        }

        public List<MessageModel> GetMessages(MessageModel lastMessage)
        {
            var startDate = DateTime.UtcNow;

            while (true)
            {
                var messagesForReturn = Messages.Where(m => m.ChatId == lastMessage.ChatId && m.CreatedUtcDate > lastMessage.CreatedUtcDate).ToList();

                if (messagesForReturn.Count > 0 || DateTime.UtcNow >= startDate + TimeSpan.FromMinutes(5))
                {
                    return messagesForReturn;
                }

                Thread.Sleep(1000);
            }
        }
    }
}
