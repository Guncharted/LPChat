using LPChat.Common.DbContracts;
using LPChat.Common.Models;
using LPChat.Common.Models.Extensions;
using LPChat.Data.MongoDb.Entities;
using LPChat.Domain;
using LPChat.Domain.Results;
using LPChat.Infrastructure.Interfaces;
using LPChat.Infrastructure.Mapping;
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

        public async Task<OperationResult> AddMessage(MessageModel messageToAdd)
        {
            Guard.NotNull(messageToAdd, nameof(messageToAdd));
            Guard.NotNull(messageToAdd.ChatId, nameof(messageToAdd.ChatId));
            Guard.NotNull(messageToAdd.PersonId, nameof(messageToAdd.PersonId));

            var message = DataMapper.Map<MessageModel, Message>(messageToAdd);
            var repo = _repositoryManager.GetRepository<Message>();

            //adding message to Db
            await repo.CreateAsync(message);

            //retrieve person information for ViewModel
            var user = await _personInfoService.GetById(messageToAdd.PersonId.Value);

            messageToAdd.ID = message.ID;
            messageToAdd.CreatedUtcDate = message.CreatedUtcDate;
            messageToAdd.PersonName = user.GetDisplayName();

            lock (threadLock)
            {
                Messages.Add(messageToAdd);
            }

            //create result
            var result = messageToAdd.ID != null && messageToAdd.ID != Guid.Empty;
            if (result)
                return new OperationResult(result, "Created", messageToAdd);
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
