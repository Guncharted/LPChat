using System;
using System.Security.Claims;

namespace LPChat.Infrastructure
{
    public static class IdentityExtensions
    {
        public static Guid? GetPersonId(this ClaimsPrincipal User)
        {
            Guid personId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (personId == Guid.Empty)
                return null;

            return personId;
        }
    }
}
