using LPChat.Domain.DTO;
using LPChat.Domain.Results;
using System;
using System.Threading.Tasks;

namespace LPChat.Domain.Interfaces
{
    public interface IAuthService
    {
        Task<OperationResult> LoginAsync(UserForLogin userForLoginDto);
        Task<OperationResult> RegisterAsync(UserForRegister userForRegister);
        Task<OperationResult> ChangePasswordAsync(UserPasswordChange user);
        Task<OperationResult> ChangePasswordAsync(UserPasswordChange user, Guid? requestorId);
    }
}
