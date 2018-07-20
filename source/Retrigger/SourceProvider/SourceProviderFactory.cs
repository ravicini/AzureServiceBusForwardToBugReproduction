namespace Retrigger.SourceProvider
{
    public static class SourceProviderFactory
    {
        public static (ISourceProvider sourceProvider, string error) GetSourceProvider()
        {
            return (new DummySourceProvider(), string.Empty);
        }
    }
}