using System;

namespace LPChat.Common.Models
{
    public class UserSecurityModel
    {
        public Guid ID { get; set; }

        public string Username { get; set; }

        public string OldPassword { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
