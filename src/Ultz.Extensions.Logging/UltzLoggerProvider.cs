using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Ultz.Extensions.Logging
{
    /// <summary>
    /// An implementation of <see cref="ILoggerProvider" /> which produces <see cref="UltzLogger" />s.
    /// </summary>
    public class UltzLoggerProvider : ILoggerProvider, IUltzLoggerObject, ISupportExternalScope
    {
        /// <summary>
        /// The default <see cref="MessageFormat" /> used by the default <see cref="MessageFormatter" />.
        /// </summary>
        public const string DefaultFormat = "§8{4}{0}[{3}] §7{5:dd}/{5:MM}/{5:yyyy} {5:HH}:{5:mm}:{5:ss} [{1}] §f{2}";

        /// <summary>
        /// The default <see cref="LogLevels" /> used. Encapsulates all <see cref="LogLevel" /> values.
        /// </summary>
        public static readonly LogLevel[] DefaultLogLevels = Enum.GetValues(typeof(LogLevel))
            .Cast<LogLevel>()
            .ToArray();

        private readonly ConcurrentDictionary<string, UltzLogger> _loggers =
            new ConcurrentDictionary<string, UltzLogger>();

        /// <inheritdoc />
        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, new UltzLogger(categoryName)
            {
                LogLevels = LogLevels,
                Outputs = Outputs,
                MessageFormat = MessageFormat,
                MessageFormatter = MessageFormatter,
                ScopeProvider = ScopeProvider,
                LogLevelStrings = LogLevelStrings
            });
        }

        /// <inheritdoc />
        public void Dispose()
        {
            foreach (var logger in _loggers.Values)
            {
                logger.Dispose();
            }
        }

        /// <inheritdoc />
        public void SetScopeProvider(IExternalScopeProvider scopeProvider)
        {
            ScopeProvider = scopeProvider;
        }

        /// <inheritdoc />
        public List<LogLevel> LogLevels { get; } = DefaultLogLevels.ToList();

        /// <inheritdoc />
        public Dictionary<LogLevel, string> LogLevelStrings { get; } = new Dictionary<LogLevel, string>
        {
            {LogLevel.None, Output.ColourCharacter + "rNONE" + Output.ColourCharacter + "7"},
            {LogLevel.Critical, Output.ColourCharacter + "cSEVERE" + Output.ColourCharacter + "7"},
            {LogLevel.Error, Output.ColourCharacter + "4ERROR" + Output.ColourCharacter + "7"},
            {LogLevel.Debug, Output.ColourCharacter + "aDEBUG" + Output.ColourCharacter + "7"},
            {LogLevel.Information, Output.ColourCharacter + "aINFO" + Output.ColourCharacter + "7"},
            {LogLevel.Warning, Output.ColourCharacter + "eWARN" + Output.ColourCharacter + "7"},
            {LogLevel.Trace, Output.ColourCharacter + "dTRACE" + Output.ColourCharacter + "7"}
        };

        /// <inheritdoc />
        public IExternalScopeProvider? ScopeProvider { get; set; } = NopScopeProvider.Instance;

        /// <inheritdoc cref="IUltzLoggerObject" />
        public void WaitForIdle() => Task.WaitAll(_loggers.Values.Select(x => Task.Run(x.WaitForIdle)).ToArray());

        /// <inheritdoc cref="IUltzLoggerObject" />
        public void Shutdown() => Task.WaitAll(_loggers.Values.Select(x => Task.Run(x.Shutdown)).ToArray());

        /// <inheritdoc cref="IUltzLoggerObject" />
        public void WaitAndShutdown()
            => Task.WaitAll(_loggers.Values.Select(x => Task.Run(x.WaitAndShutdown)).ToArray());

        /// <inheritdoc />
        public string MessageFormat { get; set; } = DefaultFormat;

        /// <inheritdoc />
        public Func<LogLevel, EventId, string, string>? MessageFormatter { get; set; }

        /// <inheritdoc />
        public List<IOutput> Outputs { get; } = new List<IOutput> {ConsoleOutput.Instance};
    }
}