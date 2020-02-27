using LPChat.Common.DbContracts;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace LPChat.Data.MongoDb.Entities
{
    public class User : IMongoEntity
    {
        [BsonId]
        public Guid ID { get; set; }

        [BsonRequired]
        public string Email { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        [BsonRequired]
        public byte[] PasswordHash { get; set; }

        [BsonRequired]
        public byte[] PasswordSalt { get; set; }

        public DateTime CreatedUtcDate { get; set; }

        public DateTime LastUpdatedUtcDate { get; set; }

        public bool? IsAdmin { get; set; }
    }
}
