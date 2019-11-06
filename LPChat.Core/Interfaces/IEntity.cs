using System;

namespace LPChat.Domain.Interfaces
{
	public interface IEntity
    {
        Guid ID { get; set; }

        DateTime CreatedUtcDate { get; set; }
        DateTime LastUpdatedUtcDate { get; set; }
    }
}
