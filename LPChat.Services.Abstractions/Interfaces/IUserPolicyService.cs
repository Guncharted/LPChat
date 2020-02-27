using System.Threading.Tasks;

namespace LPChat.Services
{
    public interface IUserPolicyService
    {
        Task SetPolicies(string userId);
    }
}