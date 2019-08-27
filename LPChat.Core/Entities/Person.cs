using System;
using System.Collections.Generic;
using System.Text;

namespace LPChat.Core.Entities
{
    public class Person
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
