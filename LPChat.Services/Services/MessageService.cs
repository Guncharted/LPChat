using LPChat.Common.DbContracts;
using LPChat.Common.Models;
using LPChat.Common.Models.Extensions;
using LPChat.Data.MongoDb.Entities;
using LPChat.Domain;
using LPChat.Domain.Results;
using LPChat.Services.Interfaces;
using LPChat.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LPChat.Services
{
    public class MessageService : IMessageService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IUserService _personInfoService;
        private readonly IInstantMessagingService _instantMessagingService;

        public MessageService(IRepositoryManager repositoryManager, 
                              IUserService personInfoService, 
                              IInstantMessagingService instantMessagingService)
        {
            _repositoryManager = repositoryManager;
            _personInfoService = personInfoService;
            _instantMessagingService = instantMessagingService;
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

            _instantMessagingService.AddMessage(messageToAdd);

            //create result
            var result = messageToAdd.ID != null && messageToAdd.ID != Guid.Empty;
            if (result)
                return new OperationResult(result, "Created", messageToAdd);
            else
                return new OperationResult(result, "Failed to create");
        }

        
    }
}
