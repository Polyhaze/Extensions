using System;
using Microsoft.Extensions.Logging;

namespace Ultz.Extensions.Logging
{
    public interface IMessageFormatter
    {
        string Format<TState>(string name, LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter, UltzLoggerProvider loggerProvider);
    }
}