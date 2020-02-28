using LPChat.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace LPChat.Filters
{
    public class UserAuthFilter : Attribute, IAsyncAuthorizationFilter
    {
        private readonly IUserPolicyService _userPolicyService;

        public UserAuthFilter(IUserPolicyService userPolicyService)
        {
            _userPolicyService = userPolicyService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var userId = context.HttpContext.User.GetPersonId();
                await _userPolicyService.SetContext(userId);
            }
        }
    }
}
