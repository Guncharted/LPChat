using System;

namespace LPChat.Infrastructure.ViewModels
{
	public class ChatInfoViewModel
	{
		public Guid ID { get; set; }
		public string Name { get; set; }
		public bool IsPublic { get; set; }
		public DateTime LastUpdateUtcDate { get; set; }
	}
}
