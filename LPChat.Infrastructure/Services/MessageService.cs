using LPChat.Domain;
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
        //TODO. To be removed and changed to memory cache
        private List<MessageViewModel> _messages;
        public List<MessageViewModel> Messages => _messages = _messages ?? new List<MessageViewModel>();

        private readonly IRepositoryManager _repositoryManager;
        private readonly IPersonInfoService _personInfoService;
        private readonly object threadLock = new object();

        public MessageService(IRepositoryManager repositoryManager, IPersonInfoService personInfoService)
        {
            _repositoryManager = repositoryManager;
            _personInfoService = personInfoService;
        }

        public async Task AddMessage(MessageViewModel message)
        {
            Guard.NotNull(message, nameof(message));
            Guard.NotNull(message.ChatId, nameof(message.ChatId));
            Guard.NotNull(message.PersonId, nameof(message.PersonId));

            //TODO. Remove this sh*t and use Automapper
            var messageModel = new Message 
            { 
                Text = message.Text,
                ChatId = message.ChatId.Value,
                PersonId = message.PersonId.Value
            };

            var repo = _repositoryManager.GetRepository<Message>();

            await repo.CreateAsync(messageModel);

            var personInfo = await _personInfoService.GetOneAsync(message.PersonId.Value);

            //TODO. Remove this sh*t and use Automapper [2]
            message.ID = messageModel.ID;
            message.CreatedUtcDate = messageModel.CreatedUtcDate;
            message.PersonName = _personInfoService.GetPersonDisplayName(personInfo);

            lock (threadLock)
            {
                Messages.Add(message);
            }
        }

        public List<MessageViewModel> GetMessages(MessageViewModel lastMessage)
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
