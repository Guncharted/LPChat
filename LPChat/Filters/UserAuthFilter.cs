using LPChat.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Security.Claims;

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
            context.HttpContext.Request.Path.Value.Contains("auth");
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var userId = context.HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                _userPolicyService.SetPolicies(userId);
            }
        }
    }
}
