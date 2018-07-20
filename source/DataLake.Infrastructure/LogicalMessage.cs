using System;

namespace DataLake.Infrastructure
{
    public class LogicalMessage<TMessage>
    {
        public LogicalMessage(TMessage message, Guid id, string contentProvider, string contentSource, DateTimeOffset creationTime)
        {
            Message = message;
            Id = id;
            ContentProvider = contentProvider.ToLowerInvariant();
            ContentSource = contentSource.ToLowerInvariant();
            CreationTime = creationTime;
        }

        public string ContentProvider { get; }

        public string ContentSource { get; }

        public DateTimeOffset CreationTime { get; }

        public Guid Id { get; }

        public TMessage Message { get; }
    }
}