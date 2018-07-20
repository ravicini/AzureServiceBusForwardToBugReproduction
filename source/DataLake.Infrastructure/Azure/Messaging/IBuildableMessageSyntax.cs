using Microsoft.ServiceBus.Messaging;

namespace DataLake.Infrastructure.Azure.Messaging
{
    /// <summary>
    ///     Fluent syntax interface.
    /// </summary>
    public interface IBuildableMessageSyntax : IDefineMessageDetailsSyntax
    {
        BrokeredMessage Build();
    }
}