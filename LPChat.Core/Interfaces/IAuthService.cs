using LPChat.Core.DTO;
using LPChat.Core.Results;
using System.Threading.Tasks;

namespace LPChat.Core.Interfaces
{
    public interface IAuthService
    {
        Task<OperationResult> Login(UserForLogin userForLoginDto);
        Task<OperationResult> Register(UserForRegister userForRegister);
    }
}
