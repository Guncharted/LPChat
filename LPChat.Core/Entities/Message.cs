using LPChat.Domain.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace LPChat.Domain.Entities
{
    public class Message : IEntity
    {
        [BsonId]
        public Guid ID { get; set; }
        public Guid ChatId { get; set; }
        public string Text { get; set; }
        public Guid PersonId { get; set; }
        public DateTime CreatedUtcDate { get; set; }
        public DateTime LastUpdatedUtcDate { get; set; }
    }
}
