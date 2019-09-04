using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPChat.Core.Entities
{
    public class Message
    {
        [BsonId]
        public Guid ID { get; set; }
        public Guid ConversationID { get; set; }
        public string Text { get; set; }
        public Guid PersonId { get; set; }
        public DateTime CreatedUtcDate { get; set; }
    }
}
