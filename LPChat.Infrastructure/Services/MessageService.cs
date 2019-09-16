using LPChat.Domain.Entities;
using LPChat.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace LPChat.Infrastructure.Services
{
    public class MessageService : IMessageService
    {
        private List<Message> _messages;

        private readonly object threadLock = new object();

        public List<Message> Messages => _messages = _messages ?? new List<Message>();

        public void AddMessage(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException();
            }
            message.CreatedUtcDate = DateTime.UtcNow;

            lock (threadLock)
            {
                Messages.Add(message);
            }
        }

        public List<Message> GetMessages(DateTime? since)
        {
            if (since == null)
            {
                return Messages;
            }

            since = since.Value.ToUniversalTime();

            var startDate = DateTime.UtcNow;

            while (true)
            {
                var messagesForReturn = Messages.Where(m => m.CreatedUtcDate >= since).ToList();

                if (messagesForReturn.Count > 0 || DateTime.UtcNow >= startDate + TimeSpan.FromMinutes(5))
                {
                    return messagesForReturn;
                }

                Thread.Sleep(1000);
            }

        }
    }
}
