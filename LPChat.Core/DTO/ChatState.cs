using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LPChat.Domain.DTO
{
    public class ChatState
    {
		[Required]
        public Guid ID { get; set; }
		public string Name { get; set; }
		public IEnumerable<Guid> PersonIds { get; set; }

		[Required]
		public bool IsPublic { get; }
	}
}
