using System;

namespace LPChat.Domain
{
	public class Guard
    {
        public static void NotNull(object value, string name)
        {
            if (value == null)
                throw new ArgumentNullException(name);
        }
        public static void NotNullOrEmpty(string value, string name)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(value);
        }
    }
}
