using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LPChat.Services.ViewModels
{
    public class ChatStateViewModel
    {
		[Required]
        public Guid ID { get; set; }
		public string Name { get; set; }
		public IEnumerable<Guid> PersonIds { get; set; }

		[Required]
		public bool IsPublic { get; }
	}
}
