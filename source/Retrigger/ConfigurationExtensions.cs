using System;
using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace Retrigger
{
    public static class ConfigurationExtensions
    {
        public static string Queue(this IConfiguration configuration)
        {
            return configuration["queue"];
        }

        public static int ServicebusBatchSize(this IConfiguration configuration)
        {
            return Convert.ToInt32(configuration["servicebusBatchSize"], CultureInfo.InvariantCulture);
        }

        public static string ServicebusConnection(this IConfiguration configuration)
        {
            return configuration["serviceBusConnection"];
        }
    }
}