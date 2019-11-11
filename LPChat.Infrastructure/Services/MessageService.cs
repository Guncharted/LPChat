using LPChat.Domain.Entities;
using LPChat.Infrastructure.Interfaces;
using LPChat.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LPChat.Infrastructure.Services
{
    public class MessageService : IMessageService
    {
        private List<Message> _messages;
        private readonly IRepositoryManager _repositoryManager;
        private readonly object threadLock = new object();

        public List<Message> Messages => _messages = _messages ?? new List<Message>();

        public MessageService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task AddMessage(MessageViewModel message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("Message cannot be null!");
            }
            
            if (message.ChatId == null)
            {
                throw new ArgumentException("Chat ID cannot be null!");
            }

            var messageModel = new Message { 
                Text = message.Text,
                ChatId = message.ChatId.Value,
            };

            var repo = _repositoryManager.GetRepository<Message>();

            await repo.CreateAsync(messageModel);
            lock (threadLock)
            {
                Messages.Add(messageModel);
            }
        }

        public List<Message> GetMessages(MessageViewModel lastMessage)
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
