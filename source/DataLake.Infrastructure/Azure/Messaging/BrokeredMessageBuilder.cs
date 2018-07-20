using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;

namespace DataLake.Infrastructure.Azure.Messaging
{
    public sealed class BrokeredMessageBuilder : IBuildableMessageSyntax, IDisposable
    {
        private readonly string correlationId;
        private readonly Dictionary<string, object> headers = new Dictionary<string, object>();
        private readonly Stream messageTextStream = new MemoryStream();

        private BrokeredMessageBuilder()
        {
            correlationId = Guid.NewGuid().ToString();
        }

        private BrokeredMessageBuilder(BrokeredMessage message)
        {
            correlationId = message.CorrelationId;
            foreach (var property in message.Properties) {
                headers.Add(property.Key, property.Value);
            }
        }

        public static IDefineMessageDetailsSyntax BasedOn(BrokeredMessage message)
        {
            return new BrokeredMessageBuilder(message);
        }

        public BrokeredMessage Build()
        {
            var message = new BrokeredMessage(messageTextStream, false) {
                ContentType = "application/json",
                CorrelationId = correlationId
            };

            foreach (var header in headers) {
                message.Properties.Add(header.Key, header.Value);
            }

            return message;
        }

        public void Dispose()
        {
            messageTextStream?.Dispose();
        }

        public static IDefineMessageDetailsSyntax New()
        {
            return new BrokeredMessageBuilder();
        }

        public IDefineMessageDetailsSyntax WithContentId(string id)
        {
            return WithHeader(HeaderKeys.Id, id.ToLowerInvariant());
        }

        public IDefineMessageDetailsSyntax WithContentProvider(string provider)
        {
            return WithHeader(HeaderKeys.ContentProvider, provider.ToLowerInvariant());
        }

        public IDefineMessageDetailsSyntax WithContentSource(string source)
        {
            return WithHeader(HeaderKeys.ContentSource, source.ToLowerInvariant());
        }

        public IDefineMessageDetailsSyntax WithCreationDate(DateTimeOffset creationDate)
        {
            return WithHeader(HeaderKeys.CreationTime, creationDate.ToString("O", CultureInfo.InvariantCulture));
        }

        public IDefineMessageDetailsSyntax WithHeader(string key, string value)
        {
            if (headers.ContainsKey(key)) {
                headers[key] = value;
            } else {
                headers.Add(key, value);
            }

            return this;
        }

        public IBuildableMessageSyntax WithMessageText<T>(T messageText)
        {
            var serializer = new JsonSerializer {
                Formatting = Formatting.Indented,
                Culture = CultureInfo.InvariantCulture,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind
            };

            var streamWriter = new StreamWriter(messageTextStream);
            var writer = new JsonTextWriter(streamWriter);

            serializer.Serialize(writer, messageText);
            streamWriter.Flush();
            messageTextStream.Flush();
            messageTextStream.Position = 0;

            return this;
        }
    }
}