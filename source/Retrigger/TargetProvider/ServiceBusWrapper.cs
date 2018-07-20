using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.ServiceBus.Messaging;
using NLog;

namespace Retrigger.TargetProvider
{
    public class ServiceBusWrapper
    {
        private readonly MessageSender messageSender;
        private Logger logger;

        public ServiceBusWrapper(IConfiguration configuration, Logger logger)
        {
            this.logger = logger;
            var messagingFactory = MessagingFactory.CreateFromConnectionString(configuration.ServicebusConnection());
            messageSender = messagingFactory.CreateMessageSender(configuration.Queue());
        }

        protected ServiceBusWrapper()
        {
        }

        public virtual Task CreateSendTask(IEnumerable<BrokeredMessage> batchToSend)
        {
            return messageSender.SendBatchAsync(batchToSend);
        }
    }
}