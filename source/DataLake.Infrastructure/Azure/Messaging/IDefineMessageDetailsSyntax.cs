using System;

namespace DataLake.Infrastructure.Azure.Messaging
{
    /// <summary>
    ///     Fluent syntax interface
    /// </summary>
    public interface IDefineMessageDetailsSyntax
    {
        IDefineMessageDetailsSyntax WithContentId(string id);

        IDefineMessageDetailsSyntax WithContentProvider(string provider);

        IDefineMessageDetailsSyntax WithContentSource(string source);

        IDefineMessageDetailsSyntax WithCreationDate(DateTimeOffset creationDate);

        IDefineMessageDetailsSyntax WithHeader(string key, string value);

        IBuildableMessageSyntax WithMessageText<T>(T messageText);
    }
}