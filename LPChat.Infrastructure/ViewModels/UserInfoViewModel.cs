﻿using System;

namespace LPChat.Infrastructure.ViewModels
{
	public class UserInfoViewModel
	{
		public Guid ID { get; set; }
		public string Username { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
	}
}
