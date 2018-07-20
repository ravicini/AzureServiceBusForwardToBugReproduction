using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Retrigger.TargetProvider;

namespace Retrigger.SourceProvider
{
    public class DummySourceProvider : ISourceProvider
    {
        public async Task LoadBatches(Func<RetriggerDto[], Task> onBatchLoaded)
        {
            var dtos = Enumerable
                .Range(0, 10000)
                .Select(i =>
                    new RetriggerDto(
                        "provider",
                        "source",
                        DateTimeOffset.Now,
                        i.ToString(CultureInfo.InvariantCulture),
                        false))
                .ToArray();

            while (true) {
                await onBatchLoaded(dtos);
            }
        }
    }
}