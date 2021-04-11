using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Ultz.Extensions.Logging
{
    /// <summary>
    /// An implementation of <see cref="ILogger" /> but with support for extra customization. This is the heart of
    /// the Ultz Logger.
    /// </summary>
    public sealed class UltzLogger : ILogger, IDisposable, IUltzLoggerObject
    {
        private readonly CancellationTokenSource _cancellationToken = new();
        private readonly BlockingCollection<string> _logMessages = new();
        private readonly ManualResetEventSlim _noMessagesResetEvent = new();
        private readonly Task _logTask;
        private readonly string _name;

        internal UltzLogger(string name)
        {
            _name = name;
            _logTask = Task.Run(CoreLog, _cancellationToken.Token);
            MessageFormatter = CoreMessageFormatter;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _logMessages.Dispose();
            Shutdown();
            _cancellationToken.Dispose();
        }

        /// <inheritdoc />
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
        {
            _logMessages.TryAdd((MessageFormatter ?? CoreMessageFormatter)(logLevel, eventId,
                formatter(state, exception)));
        }

        /// <inheritdoc />
        public bool IsEnabled(LogLevel logLevel)
        {
            return LogLevels.Contains(logLevel);
        }

        /// <inheritdoc />
        public IDisposable BeginScope<TState>(TState state)
        {
            return ScopeProvider?.Push(state) ?? NopScopeProvider.NopScope.Instance;
        }

        /// <inheritdoc />
        public List<LogLevel> LogLevels { get; internal set; } = new List<LogLevel>();

        /// <inheritdoc />
        public Dictionary<LogLevel, string> LogLevelStrings { get; internal set; } = new Dictionary<LogLevel, string>();

        /// <inheritdoc />
        public string MessageFormat { get; set; } = UltzLoggerProvider.DefaultFormat;

        /// <inheritdoc />
        public Func<LogLevel, EventId, string, string>? MessageFormatter { get; set; }

        /// <inheritdoc />
        public List<IOutput> Outputs { get; internal set; } = new List<IOutput>();

        /// <inheritdoc />
        public IExternalScopeProvider? ScopeProvider { get; set; }

        /// <inheritdoc cref="IUltzLoggerObject" />
        public void WaitForIdle()
        {
            _noMessagesResetEvent.Wait();
        }

        /// <inheritdoc cref="IUltzLoggerObject" />
        public void Shutdown() => _cancellationToken.Cancel();

        /// <inheritdoc cref="IUltzLoggerObject" />
        public void WaitAndShutdown()
        {
            WaitForIdle();
            Shutdown();
        }

        private void CoreLog()
        {
            foreach (var message in _logMessages.GetConsumingEnumerable(_cancellationToken.Token))
            {
                _noMessagesResetEvent.Reset();
                foreach (var writer in Outputs)
                {
                    lock (writer)
                    {
                        foreach (var (subMsg, colour) in Output.EnumerateSubMessages(message))
                        {
                            writer.Write(subMsg, colour);
                        }

                        writer.Write(Environment.NewLine, null);
                    }
                }

                if (_logMessages.Count == 0)
                {
                    _noMessagesResetEvent.Set();
                }
            }
        }

        private string CoreMessageFormatter(LogLevel arg1, EventId arg2, string arg3)
        {
            return string.Join("\n", arg3.Split('\n').Select(x => string.Format(MessageFormat, _name,
                LogLevelStrings[arg1], x, arg2, GetSyslogSeverityString(arg1), DateTime.Now)));
        }

        private static string GetSyslogSeverityString(LogLevel logLevel)
        {
            // 'Syslog Message Severities' from https://tools.ietf.org/html/rfc5424.
            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    return "<7>"; // debug-level messages
                case LogLevel.Information:
                    return "<6>"; // informational messages
                case LogLevel.Warning:
                    return "<4>"; // warning conditions
                case LogLevel.Error:
                    return "<3>"; // error conditions
                case LogLevel.Critical:
                    return "<2>"; // critical conditions
                case LogLevel.None:
                default:
                    return string.Empty;
            }
        }
    }
}