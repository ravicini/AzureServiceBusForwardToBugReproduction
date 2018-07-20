using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLake.Infrastructure.Azure.Messaging;
using DataLogistics.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.ServiceBus.Messaging;
using NLog;

namespace Retrigger.TargetProvider
{
    public class ServiceBusTarget
    {
        private const int MaxWaitForServerBusy = 60;
        private readonly int batchSize;
        private readonly Logger logger;
        private readonly ServiceBusWrapper serviceBusWrapper;

        public ServiceBusTarget(IConfiguration configuration, Logger logger, ServiceBusWrapper serviceBusWrapper)
        {
            this.logger = logger;
            this.serviceBusWrapper = serviceBusWrapper;
            batchSize = configuration.ServicebusBatchSize();
        }

        public async Task SendBatch(IEnumerable<RetriggerDto> batchToSend, int callCount = 1)
        {
            try {
                await Task.WhenAll(SendBatches(batchToSend));
            } catch (ServerBusyException) {
                var waitForInSeconds = callCount * 10;
                var timeSpanToWait = TimeSpan.FromSeconds(waitForInSeconds <= MaxWaitForServerBusy ? waitForInSeconds : MaxWaitForServerBusy);
                logger.Info($"Server busy, waiting for: {timeSpanToWait.TotalSeconds}s");
                await Task.Delay(timeSpanToWait);
                await SendBatch(batchToSend, callCount + 1);
            }
        }

        private IEnumerable<BrokeredMessage> Map(IEnumerable<RetriggerDto> batchToSend)
        {
            var messages = batchToSend.Where(_ => !_.Ignore).Take(batchSize).Select(d =>
                BrokeredMessageBuilder.New()
                    .WithContentId(d.Id)
                    .WithContentProvider(d.ContentProvider)
                    .WithContentSource(d.ContentSource)
                    .WithCreationDate(d.CreatedAt)
                    .WithMessageText(new RiverDtoStoredEvent(new BlobStorageLocation($"{d.ContentProvider}-{d.ContentSource}", $"{d.Id}.json")))
                    .Build());

            return messages;
        }

        private IEnumerable<Task> SendBatches(IEnumerable<RetriggerDto> batchToSend)
        {
            var sendTasks = new List<Task>();
            var hasMore = true;
            do {
                var mapped = Map(batchToSend);

                if (mapped.Any()) {
                    try {
                        sendTasks.Add(serviceBusWrapper.CreateSendTask(mapped));
                        batchToSend = batchToSend.Skip(batchSize);
                    } catch (Exception ex) {
                        logger.Error(ex, $"SendBatch to service bus failed: {string.Join(", ", batchToSend.Select(_ => _.Id))}");
                    }
                } else {
                    hasMore = false;
                }
            } while (hasMore);

            return sendTasks;
        }
    }
}