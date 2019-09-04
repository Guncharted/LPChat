using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPChat.Core.Entities
{
    public class Conversation
    {
        [BsonId]
        public Guid ID { get; set; }
        public string Name { get; set; }
        public List<Guid> PersonIds { get; set; }

        [BsonRequired]
        public bool IsPublic { get; private set; }

        public Conversation(bool isPublic)
        {
            IsPublic = isPublic;
        }
    }
}
