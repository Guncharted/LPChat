using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LPChat.Common.Models;

namespace LPChat.Infrastructure.Interfaces
{
	public interface IUserService
	{
		Task<IEnumerable<UserModel>> Get(IEnumerable<Guid> Ids);
		Task<UserModel> GetById(Guid userId);
	}
}