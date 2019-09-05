using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace LPChat.Core.Entities
{
    public class Chat
    {
        [BsonId]
        public Guid ID { get; set; }
        public string Name { get; set; }
        public IEnumerable<Guid> PersonIds { get; set; }

        [BsonRequired]
        public bool IsPublic { get; private set; }

        public Chat(bool isPublic)
        {
            IsPublic = isPublic;
        }
    }
}
