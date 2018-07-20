using System.IO;
using Newtonsoft.Json;

namespace DataLake.Infrastructure.Serialization
{
    public static class JsonSerializerExtensions
    {
        public static T DeserializeFrom<T>(this JsonSerializer serializer, Stream contentStream)
            where T : class
        {
            contentStream.Position = 0;
            using (var streamReader = new StreamReader(contentStream))
            using (var textReader = new JsonTextReader(streamReader)) {
                return serializer.Deserialize<T>(textReader);
            }
        }
    }
}