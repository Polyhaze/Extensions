using System;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Ultz.Extensions.Logging
{
    public sealed class MessageFormatter : IMessageFormatter
    {
        public static readonly IMessageFormatter Default = new MessageFormatter();

        private MessageFormatter()
        {
            // do nothing, just want this private
        }

        public string Format<TState>(string name, LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter, UltzLoggerProvider loggerProvider) =>
            string.Join("\n", formatter(state, exception).Split('\n').Select(x =>
                string.Format(loggerProvider.MessageFormat,
                    name,
                    loggerProvider.LogLevelStrings[logLevel], x, eventId,
                    UltzLoggerProvider.GetSyslogSeverityString(logLevel), DateTime.Now)));
    }
}