using System;
using System.Collections.Generic;
using System.Text;

namespace LPChat.Domain.Exceptions
{
	public class PersonNotFoundException : Exception
	{
		public PersonNotFoundException(string message) : base(message)
		{
		}

		public PersonNotFoundException(string message, Exception innerException) 
			: base (message, innerException)
		{ 
		}
	}
}
