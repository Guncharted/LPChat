using System;

namespace LPChat.Infrastructure.ViewModels
{
	public class UserModel
	{
		public Guid ID { get; set; }
		public string Username { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
	}
}
