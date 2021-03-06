﻿namespace LPChat.Domain.Results
{
    public class OperationResult
    {
        public bool Succeeded { get; private set; }
        public string Title { get; private set; }
        public object Payload { get; private set; }

        public OperationResult(bool succeeded, string message, object payload = null)
        {
            (Succeeded, Title, Payload) = (succeeded, message, payload ?? new object());
        }
    }
}
