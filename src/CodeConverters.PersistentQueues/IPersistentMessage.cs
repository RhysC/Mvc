using System;
using CodeConverters.Core.Persistence;

namespace CodeConverters.PersistentQueues
{
    public interface IPersistentMessage : IAuditable
    {
        Guid Id { get; set; }
        Guid AggregateId { get; set; }
        long Sequence { get; set; }
        string MessageType { get; set; }
        string MessageContent { get; set; }
        DateTimeOffset? ProcessedOn { get; set; }
    }
}