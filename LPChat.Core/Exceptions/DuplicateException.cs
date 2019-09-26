using System;
using System.Collections.Generic;
using System.Text;

namespace LPChat.Domain.Exceptions
{
	public class DuplicateException : Exception
	{
		public DuplicateException(string message) : base(message)
		{

		}

		public DuplicateException(string message, Exception innerException) : base(message, innerException)
		{

		}
	}
}
