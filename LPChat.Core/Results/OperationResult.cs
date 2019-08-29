using System;
using System.Collections.Generic;
using System.Text;

namespace LPChat.Core.Results
{
    public class OperationResult
    {
        public bool Success { get; private set; }
        public string Message { get; private set; }
        public object Payload { get; private set; }

        public OperationResult(bool success, string message, object payload = null)
        {
            (Success, Message, Payload) = (success, message, payload ?? new object());
        }
    }
}
