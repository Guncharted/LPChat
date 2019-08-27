using System;
using System.Collections.Generic;
using System.Text;

namespace LPChat.Core.Entities
{
    public class Message
    {
        public Guid ID { get; set; }
        public string Text { get; set; }
        public string Person { get; set; }
        public DateTime CreatedUtcDate { get; set; }
    }
}
