using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPChat.Domain.Interfaces
{
    public interface IEntity
    {
        Guid ID { get; set; }

        DateTime CreatedUtcDate { get; set; }
        DateTime LastUpdatedUtcDate { get; set; }
    }
}
