using LPChat.Domain.DTO;
using LPChat.Domain.Results;
using System.Threading.Tasks;

namespace LPChat.Domain.Interfaces
{
    public interface IAuthService
    {
        Task<OperationResult> Login(UserForLogin userForLoginDto);
        Task<OperationResult> Register(UserForRegister userForRegister);
    }
}
