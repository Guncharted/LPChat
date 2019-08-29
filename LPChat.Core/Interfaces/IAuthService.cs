using LPChat.Core.DTO;
using LPChat.Core.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LPChat.Core.Interfaces
{
    public interface IAuthService
    {
        Task<OperationResult> Login(UserForLogin userForLoginDto);
        Task<OperationResult> Register(UserForRegister userForRegister);
    }
}
