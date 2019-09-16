using System;
using System.Collections.Generic;
using System.Text;

namespace LPChat.Domain.Interfaces
{
    public interface IRepositoryManager
    {
        IRepository<T> GetRepository<T>() where T : class;
    }
}
