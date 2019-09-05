using LPChat.Core.DTO;
using LPChat.Core.Entities;
using LPChat.Core.Enums;
using LPChat.Core.Results;
using LPChat.Infrastructure.Repositories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPChat.Infrastructure.Services
{
    public class ChatService
    {
        private readonly MongoDbService<Chat> _chatContext;
        public ChatService(MongoDbService<Chat> chatContext)
        {
            _chatContext = chatContext;
        }

        public async Task<OperationResult> Create(ChatForCreate chatForCreate)
        {
            var chat = new Chat(chatForCreate.IsPublic);

            //PRIVATE CHAT: positive case - number of persons is 2
            if (!chat.IsPublic && chatForCreate.Persons.Count() == 2)
            {
                //check to avoid duplicate private chats
                var exists = await PrivateChatExists(chatForCreate.Persons);

                //if exists, then return creation error
                //TODO. return success result instead
                if (exists)
                {
                    return new OperationResult(false, "Chat already exists!");
                }
            }
            //PRIVATE CHAT: case of incorrect number of persons
            else if (!chat.IsPublic && chatForCreate.Persons.Count() != 2)
            {
                return new OperationResult(false, "Failed to create private chat - incorrect number of persons");
            }

            chat.Name = chatForCreate.Name;
            chat.PersonIds = chatForCreate.Persons;

            await _chatContext.Insert(chat);

            return new OperationResult(true, "Chat has been created!");
        }

        public async Task<OperationResult> UpdatePersonList(ChatUserChanges changes, DbAction actionType)
        {
            var chat = (await _chatContext.GetAsync(c => c.ID == changes.ChatId))?.FirstOrDefault();

            if (chat == null || chat.ID != changes.ChatId || !chat.IsPublic)
            {
                return new OperationResult(false, "Unable to update user list");
            }

            //creating new list of persons to rewrite based on specified action
            IEnumerable<Guid> newPersonIdsList = GenerateNewPersonList(chat.PersonIds, changes.PersonIds, actionType);

            //creating definitions
            var updateDef = Builders<Chat>.Update.Set(c => c.PersonIds, newPersonIdsList);
            var filterDef = Builders<Chat>.Filter.Eq(c => c.ID, chat.ID);

            //try update
            var result = await _chatContext.UpdateOne(filterDef, updateDef);

            if (result.IsAcknowledged)
            {
                return new OperationResult(true, "Users list has been updated");
            }

            return new OperationResult(false, "Failed to update user list");
        }

        public void GetChatInfo(Guid chatId)
        {

        }

        public void GetUserChats(Guid personId)
        {

        }

        private async Task<bool> PrivateChatExists(IEnumerable<Guid> userIds)
        {
            var result = await _chatContext
                .GetAsync(c => !c.IsPublic && c.PersonIds.Intersect(userIds).Count() == 2);

            return result.Count > 0 ? true : false;
        }

        private IEnumerable<Guid> GenerateNewPersonList(IEnumerable<Guid> current, IEnumerable<Guid> changes, DbAction action)
        {
            switch (action)
            {
                case DbAction.Update:
                    return current.Union(changes).Distinct();
                case DbAction.Delete:
                    return current.Where(id => changes.Any(p => p != id));
                default:
                    return current;
            }
        }
    }
}
