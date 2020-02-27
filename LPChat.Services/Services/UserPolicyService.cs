using LPChat.Common;
using LPChat.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace LPChat.Services
{
    public class UserPolicyService : IUserPolicyService
    {
        private readonly IAuthorizationContext _authContext;
        private readonly IUserService _userService;

        public UserPolicyService(IAuthorizationContext authContext, IUserService userService)
        {
            _authContext = authContext;
            _userService = userService;
        }

        public async Task SetPolicies(string userId)
        {
            var user = await _userService.GetById(new Guid(userId));

            _authContext.SetPolicy(UserPolicies.UserAdministration, user.IsAdmin);
        }
    }
}
