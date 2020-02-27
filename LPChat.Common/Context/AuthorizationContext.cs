using LPChat.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPChat.Common
{
    public class AuthorizationContext : IAuthorizationContext
    {
        private readonly Dictionary<Enum, bool> _policies = new Dictionary<Enum, bool>();
        private readonly Dictionary<Type, object> _context = new Dictionary<Type, object>();

        public void SetPolicy<T>(T policy, bool value) where T : Enum
        {
            if (!_policies.ContainsKey(policy))
            {
                _policies.Add(policy, value);
            }
        }

        public bool GetPolicy<T>(T policy) where T : Enum
        {
            if (_policies.TryGetValue(policy, out var value))
            {
                return value;
            }

            return false;
        }

        public void SetContext<T>(T value) where T : class
        {
            if (!_context.ContainsKey(typeof(T)))
            {
                _context.Add(typeof(T), value);
            }
            else
                throw new DuplicateException($"{typeof(T)} is aready present in the context dictionary");
        }

        public T GetContext<T>() where T : class
        {
            if (_context.TryGetValue(typeof(T), out var value))
            {
                return (T)value;
            }

            throw new KeyNotFoundException($"{typeof(T).Name} is requested but was not found");
        }
    }
}
