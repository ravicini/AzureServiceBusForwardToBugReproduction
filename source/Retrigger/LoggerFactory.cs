using NLog;
using NLog.Config;
using NLog.Targets;

namespace Retrigger
{
    public static class LoggerFactory
    {
        public static Logger ConfigureLogger()
        {
            var consoleTarget = new ColoredConsoleTarget { Layout = @"${message}" };

            var layout = "${longdate}|${event-properties:item=EventId.Id}|${uppercase:${level}}|${logger}|${message} ${replace-newlines:${exception:format=ToString}}";

            var fileTarget = new FileTarget {
                FileName = "${basedir}/log.txt",
                Layout = layout
            };

            var errorTarget = new FileTarget {
                FileName = "${basedir}/log.err.txt",
                Layout = layout
            };

            var config = new LoggingConfiguration();
            config.AddTarget("console", consoleTarget);
            config.AddTarget("file", fileTarget);
            config.AddTarget("errorFile", errorTarget);

            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, consoleTarget));
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, fileTarget));
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Error, errorTarget));

            LogManager.Configuration = config;

            return LogManager.GetLogger("logger");
        }
    }
}
