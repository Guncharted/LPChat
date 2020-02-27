using LPChat.Common;
using LPChat.Common.Models;
using LPChat.Domain;
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

        public async Task SetContext(Guid? userId)
        {
            Guard.NotNull(userId, nameof(userId));
            var user = await _userService.GetById(userId.Value);
            if (user != null)
            {
                _authContext.SetContext(user);
            }
            _authContext.SetPolicy(UserPolicies.UserAdministration, user.IsAdmin);
        }
    }
}
