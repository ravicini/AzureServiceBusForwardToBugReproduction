using System;

namespace Retrigger.TargetProvider
{
    public class RetriggerDto
    {
        public RetriggerDto(string contentProvider, string contentSource, DateTimeOffset createdAt, string id, bool ignore)
        {
            ContentProvider = contentProvider;
            ContentSource = contentSource;
            CreatedAt = createdAt;
            Id = id;
            Ignore = ignore;
        }

        public string ContentProvider { get; }

        public string ContentSource { get; }

        public DateTimeOffset CreatedAt { get; }

        public string Id { get; }

        public bool Ignore { get; }

        public static RetriggerDto Create(string contentProvider, string contentSource, DateTimeOffset createdAt, string id)
        {
            return new RetriggerDto(contentProvider, contentSource, createdAt, id, false);
        }

        public static RetriggerDto CreateIgnored()
        {
            return new RetriggerDto(default, default, default, default, true);
        }
    }
}