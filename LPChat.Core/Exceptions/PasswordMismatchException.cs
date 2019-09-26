using System;
using System.Collections.Generic;
using System.Text;

namespace LPChat.Domain.Exceptions
{
	public class PasswordMismatchException : Exception
	{
		public PasswordMismatchException(string message) : base (message)
		{
		}

		public PasswordMismatchException(string message, Exception innerException) : base (message, innerException)
		{
		}
	}
}
