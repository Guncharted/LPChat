using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace LPChat.Domain.Exceptions
{
	[Serializable]
	public class ChatAppException : Exception
	{
		public ChatAppException() { }

		public ChatAppException(string message) : base(message) { }

		public ChatAppException(string message, Exception innerException) : base(message, innerException) { }

		protected ChatAppException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}
