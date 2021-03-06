﻿using LPChat.Domain.Results;
using System;
using System.Threading.Tasks;
using LPChat.Common.Models;

namespace LPChat.Services.Interfaces
{
    public interface IAuthService
    {
        Task<OperationResult> LoginAsync(UserSecurityModel userForLogin);
        Task<OperationResult> RegisterAsync(UserSecurityModel userForRegister);
        Task<OperationResult> ChangePasswordAsync(UserSecurityModel userToChange);
    }
}
