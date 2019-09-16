using LPChat.Domain.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace LPChat.Domain.Entities
{
    public class Chat : IEntity
    {
        [BsonId]
        public Guid ID { get; set; }
        public string Name { get; set; }
        public IEnumerable<Guid> PersonIds { get; set; }

        [BsonRequired]
        public bool IsPublic { get; private set; }

		public DateTime LastUpdatedUtcDate { get; set; }

		public Chat(bool isPublic)
        {
            IsPublic = isPublic;
        }
    }
}
