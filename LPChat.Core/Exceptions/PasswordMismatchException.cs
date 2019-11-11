﻿using System;
using System.Runtime.Serialization;

namespace LPChat.Domain.Exceptions
{
	[Serializable]
	public class PasswordMismatchException : Exception
	{
		public PasswordMismatchException() { }

		public PasswordMismatchException(string message) : base(message) { }

		public PasswordMismatchException(string message, Exception innerException) : base(message, innerException) { }

		protected PasswordMismatchException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}
