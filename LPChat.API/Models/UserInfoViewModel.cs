using System;

namespace LPChat.Services.ViewModels
{
	public class UserInfoViewModel
	{
		public Guid ID { get; set; }
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
	}
}
