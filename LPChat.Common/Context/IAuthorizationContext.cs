using System;

namespace LPChat.Common
{
    public interface IAuthorizationContext
    {
        T GetContext<T>() where T : class;
        bool GetPolicy<T>(T policy) where T : Enum;
        void SetContext<T>(T value) where T : class;
        void SetPolicy<T>(T policy, bool value) where T : Enum;
    }
}