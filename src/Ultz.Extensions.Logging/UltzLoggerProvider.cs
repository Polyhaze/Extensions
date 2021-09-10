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
    /// An implementation of <see cref="ILoggerProvider" /> which produces <see cref="UltzLogger" />s.
    /// </summary>
    public class UltzLoggerProvider : ILoggerProvider, ISupportExternalScope
    {
        /// <summary>
        /// The default <see cref="MessageFormat" /> used by the default <see cref="MessageFormatter" />.
        /// </summary>
        public const string DefaultFormat = "§7[§3{5:HH}:{5:mm}:{5:ss}§7] [{1} §9{0}§7] §f{2}";

        /// <summary>
        /// The default <see cref="MessageFormat" /> used by the default <see cref="MessageFormatter" />.
        /// </summary>
        public const string ExtendedDefaultFormat =
            "§8{4}{0}[{3}] §7{5:dd}/{5:MM}/{5:yyyy} {5:HH}:{5:mm}:{5:ss} [{1}] §f{2}";

        /// <summary>
        /// The default <see cref="LogLevels" /> used. Encapsulates all <see cref="LogLevel" /> values.
        /// </summary>
        public static readonly LogLevel[] DefaultLogLevels = Enum.GetValues(typeof(LogLevel))
            .Cast<LogLevel>()
            .ToArray();

        private readonly ConcurrentDictionary<string, UltzLogger> _loggers = new();
        private readonly CancellationTokenSource _cancellationToken = new();
        private readonly BlockingCollection<string> _logMessages = new();
        private readonly object _syncRoot = new();
        private readonly Func<bool> _coreWaitForIdle;
        private Thread? _logThread;

        public UltzLoggerProvider()
        {
            MessageFormatter = Logging.MessageFormatter.Default;
            _coreWaitForIdle = () => _logThread is null;
        }

        /// <inheritdoc />
        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, new UltzLogger(categoryName, this));
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _logMessages.Dispose();
            Shutdown();
            _cancellationToken.Dispose();
        }

        /// <inheritdoc />
        public void SetScopeProvider(IExternalScopeProvider scopeProvider)
        {
            ScopeProvider = scopeProvider;
        }

        /// <summary>
        /// Gets a list of log levels for which this logger is applicable.
        /// </summary>
        public List<LogLevel> LogLevels { get; } = DefaultLogLevels.ToList();

        /// <summary>
        /// Gets a dictionary outlining the string representations of each log level.
        /// </summary>
        public Dictionary<LogLevel, string> LogLevelStrings { get; } = new()
        {
            {LogLevel.None, Output.ColourCharacter + "rNONE" + Output.ColourCharacter + "7"},
            {LogLevel.Critical, Output.ColourCharacter + "cSEVERE" + Output.ColourCharacter + "7"},
            {LogLevel.Error, Output.ColourCharacter + "4ERROR" + Output.ColourCharacter + "7"},
            {LogLevel.Debug, Output.ColourCharacter + "aDEBUG" + Output.ColourCharacter + "7"},
            {LogLevel.Information, Output.ColourCharacter + "aINFO" + Output.ColourCharacter + "7"},
            {LogLevel.Warning, Output.ColourCharacter + "eWARN" + Output.ColourCharacter + "7"},
            {LogLevel.Trace, Output.ColourCharacter + "dTRACE" + Output.ColourCharacter + "7"}
        };

        /// <summary>
        /// Gets or sets the external scope provider.
        /// </summary>
        public IExternalScopeProvider? ScopeProvider { get; set; }

        /// <summary>
        /// Waits until there is a window in which there are no messages being output to console.
        /// </summary>
        [Obsolete("Methods which \"wait for idle\" are no longer accurate and should be deferred.")]
        public void WaitForIdle() => SpinWait.SpinUntil(_coreWaitForIdle);

        /// <summary>
        /// Cancels the background logging task.
        /// </summary>
        /// <remarks>
        /// Done automatically in the <see cref="Dispose" /> method. Generally there's no reason to use this method.
        /// </remarks>
        public void Shutdown() => _cancellationToken.Dispose();

        /// <summary>
        /// Equivalent to <see cref="WaitForIdle"/> and <see cref="Shutdown"/>.
        /// </summary>
        [Obsolete("Methods which \"wait for idle\" are no longer accurate and should be deferred.")]
        public void WaitAndShutdown()
        {
            WaitForIdle();
            Shutdown();
        }

        private void CoreLog()
        {
            while (_logMessages.TryTake(out var message, 100, _cancellationToken.Token))
            {
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
            }

            _logThread = null;
        }

        /// <summary>
        /// Gets the syslog message severity for the given loglevel, as defined in https://tools.ietf.org/html/rfc5424.
        /// </summary>
        /// <param name="logLevel">The log level to get a syslog severity for.</param>
        /// <returns>The syslog severity of the given loglevel.</returns>
        public static string GetSyslogSeverityString(LogLevel logLevel)
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


        /// <summary>
        /// Gets or sets the message format of this logger.
        /// </summary>
        /// <remarks>
        /// This property is used by the default <see cref="MessageFormatter" />. If <see cref="MessageFormatter" /> is
        /// set to a formatter that doesn't use this property, this value will be entirely ignored.
        /// </remarks>
        public string MessageFormat { get; set; } = DefaultFormat;

        /// <summary>
        /// Gets or sets the message formatter, used to construct the final form of log messages.
        /// </summary>
        public IMessageFormatter? MessageFormatter { get; set; }

        /// <summary>
        /// Gets a list of output buffers to which log messages are written.
        /// </summary>
        public List<IOutput> Outputs { get; } = new() {ConsoleOutput.Instance};

        internal void Log<TState>(string name, LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (!LogLevels.Contains(logLevel))
            {
                return;
            }

            _logMessages.TryAdd(
                (MessageFormatter ?? Logging.MessageFormatter.Default).Format(name, logLevel, eventId, state, exception,
                    formatter, this));
            if (_logThread is not null)
            {
                return;
            }

            lock (_syncRoot)
            {
                if (_logThread is not null)
                {
                    return;
                }

                _logThread = new(CoreLog) { IsBackground = false };
                _logThread.Start();
            }
        }
    }
}