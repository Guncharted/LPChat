using System;
using System.Collections.Generic;
using System.Text;

namespace LPChat.Common.Models.Extensions
{
    public static class UserExtensions
    {
        public static string GetDisplayName(this UserModel personInfo)
        {
            if (string.IsNullOrWhiteSpace(personInfo.FirstName) || string.IsNullOrWhiteSpace(personInfo.LastName))
            {
                return personInfo.Username;
            }

            return string.Format($"{personInfo.FirstName} {personInfo.LastName}");
        }
    }
}
