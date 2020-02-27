using System;

namespace LPChat.Common.Models
{
	public class UserModel
	{
		public Guid ID { get; set; }
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public bool IsAdmin { get; set; }
	}
}
