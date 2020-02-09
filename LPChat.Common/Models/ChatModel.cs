using System;
using System.Collections.Generic;

namespace LPChat.Common.Models
{
    public class ChatModel
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        public bool IsPublic { get; set; }

        public IEnumerable<Guid> PersonIds { get; set; }

        public DateTime LastUpdateUtcDate { get; set; }
    }
}
