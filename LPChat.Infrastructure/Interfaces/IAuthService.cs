using LPChat.Infrastructure.ViewModels;
using LPChat.Domain.Results;
using System;
using System.Threading.Tasks;

namespace LPChat.Infrastructure.Interfaces
{
    public interface IAuthService
    {
        Task<OperationResult> LoginAsync(UserSecurityModel userForLogin);
        Task<OperationResult> RegisterAsync(UserSecurityModel userForRegister);
        Task<OperationResult> ChangePasswordAsync(UserSecurityModel user);
        Task<OperationResult> ChangePasswordAsync(UserSecurityModel user, Guid? requestorId);
    }
}
