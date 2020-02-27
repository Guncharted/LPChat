using LPChat.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace LPChat.Filters
{
    public class UserAuthFilter : Attribute, IAuthorizationFilter
    {
        private readonly IUserPolicyService _userPolicyService;

        public UserAuthFilter(IUserPolicyService userPolicyService)
        {
            _userPolicyService = userPolicyService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var userId = context.HttpContext.User.GetPersonId();
                _userPolicyService.SetContext(userId);
            }
        }
    }
}
