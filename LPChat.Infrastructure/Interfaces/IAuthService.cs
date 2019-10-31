using LPChat.Infrastructure.ViewModels;
using LPChat.Domain.Results;
using System;
using System.Threading.Tasks;

namespace LPChat.Infrastructure.Interfaces
{
    public interface IAuthService
    {
        Task<OperationResult> LoginAsync(UserLoginViewModel userForLogin);
        Task<OperationResult> RegisterAsync(UserRegisterViewModel userForRegister);
        Task<OperationResult> ChangePasswordAsync(UserPasswordChangeViewModel user);
        Task<OperationResult> ChangePasswordAsync(UserPasswordChangeViewModel user, Guid? requestorId);
    }
}
