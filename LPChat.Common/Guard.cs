using System;

namespace LPChat.Domain
{
    public class Guard
    {
        protected Guard() { }

        public static void NotNull(object value, string name)
        {
            _ = value ?? throw new ArgumentNullException(name);
        }

        public static void NotNullOrEmpty(string value, string name)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(name);
        }
    }
}
