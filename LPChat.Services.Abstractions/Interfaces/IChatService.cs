﻿using LPChat.Common.Models;
using LPChat.Domain.Results;
using System;
using System.Threading.Tasks;

namespace LPChat.Services.Interfaces
{
    public interface IChatService
    {
        Task<OperationResult> Create(ChatModel chatForCreate);
        void GetChatInfo(Guid chatId);
        Task<OperationResult> Update(ChatModel patch);
    }
}