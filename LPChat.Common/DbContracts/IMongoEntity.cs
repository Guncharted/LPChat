using System;

namespace LPChat.Common.DbContracts
{
	public interface IMongoEntity
    {
        Guid ID { get; set; }

        DateTime CreatedUtcDate { get; set; }

        DateTime LastUpdatedUtcDate { get; set; }
    }
}
