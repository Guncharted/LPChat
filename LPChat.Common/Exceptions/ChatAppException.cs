using System;
using System.Runtime.Serialization;

namespace LPChat.Common.Exceptions
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
