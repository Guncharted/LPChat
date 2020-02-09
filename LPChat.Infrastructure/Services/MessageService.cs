using LPChat.Common.DbContracts;
using LPChat.Common.Models;
using LPChat.Data.MongoDb.Entities;
using LPChat.Domain;
using LPChat.Domain.Results;
using LPChat.Infrastructure.Interfaces;
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
        private List<MessageModel> _messages;
        public List<MessageModel> Messages => _messages = _messages ?? new List<MessageModel>();

        private readonly IRepositoryManager _repositoryManager;
        private readonly IUserService _personInfoService;
        private readonly object threadLock = new object();

        public MessageService(IRepositoryManager repositoryManager, IUserService personInfoService)
        {
            _repositoryManager = repositoryManager;
            _personInfoService = personInfoService;
        }

        public async Task<OperationResult> AddMessage(MessageModel message)
        {
            Guard.NotNull(message, nameof(message));
            Guard.NotNull(message.ChatId, nameof(message.ChatId));
            Guard.NotNull(message.PersonId, nameof(message.PersonId));

            // TODO. Remove this sh*t and use Automapper
            var messageModel = new Message
            {
                Text = message.Text,
                ChatId = message.ChatId.Value,
                PersonId = message.PersonId.Value
            };

            var repo = _repositoryManager.GetRepository<Message>();

            //adding message to Db
            await repo.CreateAsync(messageModel);

            //retrieve person information for ViewModel
            var personInfo = await _personInfoService.GetOneAsync(message.PersonId.Value);

            // TODO. Remove this sh*t and use Automapper [2]
            message.ID = messageModel.ID;
            message.CreatedUtcDate = messageModel.CreatedUtcDate;
            message.PersonName = _personInfoService.GetPersonDisplayName(personInfo);

            lock (threadLock)
            {
                Messages.Add(message);
            }

            //create result
            var result = message.ID != null && message.ID != Guid.Empty;
            if (result)
                return new OperationResult(result, "Created", message);
            else
                return new OperationResult(result, "Failed to create");
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
