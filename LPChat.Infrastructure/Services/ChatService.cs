using LPChat.Common.DbContracts;
using LPChat.Common.Exceptions;
using LPChat.Common.Models;
using LPChat.Common.Models.Extensions;
using LPChat.Data.MongoDb.Entities;
using LPChat.Domain.Results;
using LPChat.Infrastructure.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPChat.Infrastructure.Services
{
    public class ChatService : IChatService
    {
        private readonly IUserService _personInfoService;
        private readonly IRepositoryManager _repoManager;

        public ChatService(IRepositoryManager repoManager, IUserService personInfoService)
        {
            _personInfoService = personInfoService;
            _repoManager = repoManager;
        }

        public async Task<OperationResult> Create(ChatModel chatForCreate)
        {
            var chat = new Chat(chatForCreate.IsPublic);

            //PRIVATE CHAT: positive case - number of persons is 2
            if (!chat.IsPublic && chatForCreate.PersonIds.Count() == 2)
            {
                //check to avoid duplicate private chats
                var chatExists = await PrivateChatExists(chatForCreate.PersonIds);

                //if exists, then return creation error
                if (chatExists)
                {
                    throw new ChatAppException("Chat already exists.");
                }
            }
            //PRIVATE CHAT: case of incorrect number of persons
            else if (!chat.IsPublic && chatForCreate.PersonIds.Count() != 2)
            {
                throw new ChatAppException("Failed to create private chat - incorrect number of persons");
            }

            chat.Name = chatForCreate.Name;
            chat.PersonIds = chatForCreate.PersonIds;
            chat.LastUpdatedUtcDate = DateTime.UtcNow;

            var repository = _repoManager.GetRepository<Chat>();
            await repository.CreateAsync(chat);

            return new OperationResult(true, "Chat has been created!", chat);
        }

        public async Task<OperationResult> Update(ChatModel patch)
        {
            var repository = _repoManager.GetRepository<Chat>();
            var chat = (await repository.GetAsync(c => c.ID == patch.ID))?.FirstOrDefault();

            if (chat == null || chat.ID != patch.ID || !chat.IsPublic)
            {
                throw new ChatAppException("Unable to update user list");
            }

            //creating new list of persons to rewrite based on specified action
            //TODO.add check for existing users
            var newPersonIdsList = patch.PersonIds.Distinct();

            chat.PersonIds = newPersonIdsList;
            var result = await repository.UpdateAsync(chat);

            if (result > 0)
            {
                return new OperationResult(true, "Users list has been updated");
            }

            throw new ChatAppException("Failed to update user list");
        }

        //for current chat
        public void GetChatInfo(Guid chatId)
        {
            throw new NotImplementedException();
        }

        //for sidebar
        public async Task<IEnumerable<ChatModel>> GetPersonChatList(Guid personId)
        {
            var repository = _repoManager.GetRepository<Chat>();
            var chats = (await repository.GetAsync(c => c.PersonIds.Contains(personId))).ToList();

            if (chats.Count == 0)
                return new List<ChatModel>();

            if (chats.Any(c => !c.IsPublic))
            {
                //getting ids of companions
                var companionIds = chats
                    .Where(c => !c.IsPublic)
                    .Select(p => p.PersonIds.First(id => id != personId));

                //retrieving companions profiles
                var companionsInfo = await _personInfoService.Get(companionIds);

                //setting names of private chats equal to companion's name
                chats.ForEach(chat =>
                {
                    if (!chat.IsPublic)
                    {
                        var companionId = chat.PersonIds.First(i => i != personId);
                        var companion = companionsInfo.First(p => p.ID == companionId);
                        chat.Name = companion.GetDisplayName();
                    }
                });
            }

            var chatList = chats.Select(c => new ChatModel
            {
                ID = c.ID,
                Name = c.Name,
                IsPublic = c.IsPublic
            });

            return chatList;
        }

        private async Task<bool> PrivateChatExists(IEnumerable<Guid> userIds)
        {
            var repository = _repoManager.GetRepository<Chat>();
            var result = await repository
                .GetAsync(c => !c.IsPublic && userIds.All(uid => c.PersonIds.Contains(uid)));

            return result?.Count() > 0;
        }
    }
}
