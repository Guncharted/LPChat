using LPChat.Domain.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace LPChat.Domain.Entities
{
    public class Person : IEntity
    {
        [BsonId]
        public Guid ID { get; set; }

        [BsonRequired]
        public string Username { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        [BsonRequired]
        public byte[] PasswordHash { get; set; }

        [BsonRequired]
        public byte[] PasswordSalt { get; set; }
        public DateTime CreatedUtcDate { get; set; }

        public DateTime LastUpdatedUtcDate { get; set; }
    }
}
