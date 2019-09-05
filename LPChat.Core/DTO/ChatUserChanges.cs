using System;
using System.Collections.Generic;

namespace LPChat.Core.DTO
{
    public class ChatUserChanges
    {
        public Guid ChatId { get; set; }
        public IEnumerable<Guid> PersonIds { get; set; }
    }
}
