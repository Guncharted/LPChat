using System;
using System.Collections.Generic;
using System.Text;

namespace LPChat.Domain.Exceptions
{
	public class ChatAppException : Exception
	{
		public ChatAppException(string message) : base(message)
		{
		}
	}
}
