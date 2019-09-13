﻿using LPChat.Core.DTO;
using LPChat.Core.Entities;
using LPChat.Core.Enums;
using LPChat.Core.Exceptions;
using LPChat.Core.Interfaces;
using LPChat.Core.Results;
using LPChat.Infrastructure.Repositories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPChat.Infrastructure.Services
{
    public class ChatService : IChatService
	{
        private readonly MongoDbService<Chat> _chatContext;
		private readonly PersonInfoService _personInfoService;
        public ChatService(MongoDbService<Chat> chatContext, PersonInfoService personInfoService)
        {
            _chatContext = chatContext;
			_personInfoService = personInfoService;
        }

        public async Task<OperationResult> Create(ChatForCreate chatForCreate)
        {
            var chat = new Chat(chatForCreate.IsPublic);

            //PRIVATE CHAT: positive case - number of persons is 2
            if (!chat.IsPublic && chatForCreate.PersonIds.Count() == 2)
            {
                //check to avoid duplicate private chats
                var exists = await PrivateChatExists(chatForCreate.PersonIds);

                //if exists, then return creation error
                if (exists)
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

            await _chatContext.Insert(chat);

            return new OperationResult(true, "Chat has been created!", chat);
        }

        public async Task<OperationResult> UpdatePersonList(ChatState newChatState)
        {
            var chat = (await _chatContext.GetAsync(c => c.ID == newChatState.ID))?.FirstOrDefault();

            if (chat == null || chat.ID != newChatState.ID || !chat.IsPublic)
            {
				throw new ChatAppException("Unable to update user list");
            }

            //creating new list of persons to rewrite based on specified action
			//TODO.add check for existing users
            var newPersonIdsList = newChatState.PersonIds.Distinct();

			//creating definition
			var updateDef = Builders<Chat>.Update
				.Set(c => c.PersonIds, newPersonIdsList)
				.Set(c => c.LastUpdatedUtcDate, DateTime.UtcNow);

            //try update
            var result = await _chatContext
				.UpdateOneAsync(c => c.ID == newChatState.ID && c.LastUpdatedUtcDate == chat.LastUpdatedUtcDate, updateDef);

            if (result.ModifiedCount > 0)
            {
                return new OperationResult(true, "Users list has been updated");
            }

			throw new ChatAppException("Failed to update user list");
        }

		//for current chat
        public void GetChatInfo(Guid chatId)
        {
			//determine use cases first
        }

		//for sidebar
		public async Task<IEnumerable<ChatInfo>> GetPersonChatList(Guid personId)
		{
			var chats = await _chatContext.GetAsync(c => c.PersonIds.Contains(personId));

			if (chats.Count == 0)
			{
				return new List<ChatInfo>();
			}

			if (chats.Any(c => !c.IsPublic))
			{
				//getting ids of companions
				var companionIds = chats
					.Where(c => !c.IsPublic)
					.Select(p => p.PersonIds.First(id => id != personId));

				//retrieving companions profiles
				var companionsInfo = await _personInfoService.GetManyAsync(companionIds);

				//setting names of private chats equal to companion's name
				chats.ForEach(c =>
				{
					if (!c.IsPublic)
					{
						var compId = c.PersonIds.First(i => i != personId);
						var comp = companionsInfo.First(p => p.ID == compId);
						c.Name = _personInfoService.GetPersonDisplayName(comp);
					}
				});
			}

			var chatList = chats.Select(c => new ChatInfo
			{
				ID = c.ID,
				Name = c.Name,
				IsPublic = c.IsPublic
			});

			return chatList;
		}

        private async Task<bool> PrivateChatExists(IEnumerable<Guid> userIds)
        {
            var result = await _chatContext
                .GetAsync(c => !c.IsPublic && userIds.All(uid => c.PersonIds.Contains(uid)));

            return result.Count > 0 ? true : false;
        }


    }
}
