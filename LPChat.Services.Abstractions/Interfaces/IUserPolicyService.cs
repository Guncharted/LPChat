using System;
using System.Threading.Tasks;

namespace LPChat.Services
{
    public interface IUserPolicyService
    {
        Task SetContext(Guid? userId);
    }
}