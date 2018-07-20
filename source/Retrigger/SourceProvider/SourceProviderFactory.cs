using Microsoft.Extensions.Configuration;
using NLog;

namespace Retrigger.SourceProvider
{
    public static class SourceProviderFactory
    {
        public static (ISourceProvider sourceProvider, string error) GetSourceProvider(IConfiguration configuration, Logger logger)
        {
            return (new DummySourceProvider(), string.Empty);
        }
    }
}