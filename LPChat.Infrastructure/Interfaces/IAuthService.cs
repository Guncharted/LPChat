using LPChat.Infrastructure.ViewModels;
using LPChat.Domain.Results;
using System;
using System.Threading.Tasks;

namespace LPChat.Infrastructure.Interfaces
{
    public interface IAuthService
    {
        Task<OperationResult> LoginAsync(PersonLoginViewModel userForLogin);
        Task<OperationResult> RegisterAsync(PersonRegisterViewModel userForRegister);
        Task<OperationResult> ChangePasswordAsync(PersonPasswordChangeViewModel user);
        Task<OperationResult> ChangePasswordAsync(PersonPasswordChangeViewModel user, Guid? requestorId);
    }
}
