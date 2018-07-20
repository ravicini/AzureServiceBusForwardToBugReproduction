using System;
using System.Threading.Tasks;
using Retrigger.TargetProvider;

namespace Retrigger.SourceProvider
{
    public interface ISourceProvider
    {
        Task LoadBatches(Func<RetriggerDto[], Task> onBatchLoaded);
    }
}