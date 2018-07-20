using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Retrigger.SourceProvider;
using Retrigger.TargetProvider;

namespace Retrigger
{
    public static class Program
    {
        static Program()
        {
            ServicePointManager.UseNagleAlgorithm = false;
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.DefaultConnectionLimit = 60000;
            ThreadPool.SetMinThreads(100, 100);
        }

        public static async Task Main(string[] args)
        {
            var logger = LoggerFactory.ConfigureLogger();
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddCommandLine(args)
                .Build();

            if (string.IsNullOrEmpty(configuration.Queue())) {
                Console.WriteLine("Please provide a queue name (--queue <name>)");
                return;
            }

            var (source, error) = SourceProviderFactory.GetSourceProvider(configuration, logger);
            if (!string.IsNullOrEmpty(error)) {
                Console.WriteLine(error);
                return;
            }

            var serviceBusWrapper = new ServiceBusWrapper(configuration, logger);
            var target = new ServiceBusTarget(configuration, logger, serviceBusWrapper);

            logger.Info("Started loading data");
            var batchCounter = 0L;
            var loadTimer = Stopwatch.StartNew();
            await source.LoadBatches(batch =>
            {
                loadTimer.Stop();
                batchCounter++;
                logger.Info($"Sending batch {batchCounter}, load took: {loadTimer.Elapsed.TotalSeconds}s");
                loadTimer.Restart();
                return target.SendBatch(batch);
            });
        }
    }
}