using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Ultz.Extensions.Logging
{
    /// <summary>
    /// Encapsulates options for logger objects.
    /// </summary>
    public interface IUltzLoggerObject
    {
        /// <summary>
        /// Gets a list of log levels for which this logger is applicable.
        /// </summary>
        List<LogLevel> LogLevels { get; }
        /// <summary>
        /// Gets a dictionary outlining the string representations of each log level.
        /// </summary>
        Dictionary<LogLevel, string> LogLevelStrings { get; }
        /// <summary>
        /// Gets or sets the message format of this logger.
        /// </summary>
        /// <remarks>
        /// This property is used by the default <see cref="MessageFormatter"/>. If <see cref="MessageFormatter"/> is
        /// set to a formatter that doesn't use this property, this value will be entirely ignored.
        /// </remarks>
        string MessageFormat { get; set; }
        /// <summary>
        /// Gets or sets the message formatter, used to construct the final form of log messages.
        /// </summary>
        Func<LogLevel, EventId, string, string>? MessageFormatter { get; set; }
        /// <summary>
        /// Gets a list of output buffers to which log messages are written.
        /// </summary>
        List<IOutput> Outputs { get; }
        /// <summary>
        /// Gets or sets an external scope provider.
        /// </summary>
        IExternalScopeProvider? ScopeProvider { get; set; }
    }
}