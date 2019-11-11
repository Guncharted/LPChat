using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LPChat.Helpers
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
