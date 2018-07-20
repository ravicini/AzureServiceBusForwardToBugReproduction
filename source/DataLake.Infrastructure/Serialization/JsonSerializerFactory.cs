using System.Globalization;
using Newtonsoft.Json;

namespace DataLake.Infrastructure.Serialization
{
    public static class JsonSerializerFactory
    {
        public static JsonSerializer CreateInstance()
        {
            return new JsonSerializer {
                Formatting = Formatting.Indented,
                Culture = CultureInfo.InvariantCulture,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind
            };
        }
    }
}