using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using LPChat.Common.DbContracts;

namespace LPChat.Data.MongoDb.Entities
{
    public class Chat : IMongoEntity
    {
        [BsonId]
        public Guid ID { get; set; }
        public string Name { get; set; }
        public IEnumerable<Guid> PersonIds { get; set; }

        [BsonRequired]
        public bool IsPublic { get; private set; }
        public DateTime CreatedUtcDate { get; set; }

        public DateTime LastUpdatedUtcDate { get; set; }

        public Chat(bool isPublic)
        {
            IsPublic = isPublic;
        }
    }
}
