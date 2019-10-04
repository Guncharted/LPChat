using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace LPChat.Domain.Exceptions
{
	[Serializable]
	public class PersonNotFoundException : Exception
	{
		public PersonNotFoundException() { }

		public PersonNotFoundException(string message) : base(message) { }

		public PersonNotFoundException(string message, Exception innerException) : base(message, innerException) { }

		protected PersonNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}
