using System;
using System.Collections.Generic;
using System.Text;

namespace LPChat.Core.DTO
{
	public class ChatInfo
	{
		public Guid ID { get; set; }
		public string Name { get; set; }
		public bool IsPublic { get; set; }
		public DateTime LastUpdateUtcDate { get; set; }
	}
}
