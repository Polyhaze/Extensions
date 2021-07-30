using System;
using System.Collections.Concurrent;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace Ultz.Extensions.Logging
{
    /// <summary>
    /// A static wrapper over the creation and usage of <see cref="ILogger"/>s using
    /// <see cref="UltzLoggerProvider"/> by default - can be overridden - and uses the
    /// filename (without the extension) as a category name.
    /// </summary>
    public static class Log
    {
        private static ILoggerProvider? _provider;
        private static bool _providerSetup;

        private static ConcurrentDictionary<string, ILogger> _loggers = new();

        /// <summary>
        /// The logger provider used by the static wrapper. May be null.
        /// </summary>
        public static ILoggerProvider? LoggerProvider
        {
            get
            {
                if (_providerSetup)
                {
                    return _provider;
                }

                return LoggerProvider = new UltzLoggerProvider();
            }

            set
            {
                _provider = value;
                _providerSetup = true;
                _loggers.Clear();
            }
        }

        /// <summary>
        /// Gets or creates a <see cref="ILogger"/> instance with the given category name, or returns
        /// <c>null</c> if the <see cref="LoggerProvider"/> has been explicitly configured to be <c>null</c>.
        /// </summary>
        /// <param name="categoryName">The category name.</param>
        /// <returns>A logger.</returns>
        public static ILogger? GetOrCreateLogger(string categoryName) => LoggerProvider is not null
            ? _loggers.GetOrAdd(categoryName, k => LoggerProvider.CreateLogger(k))
            : null;

        /// <summary>
        /// Writes a debug log message.
        /// </summary>
        /// <param name="eventId">The event id associated with the log.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message in message template format.</param>
        /// <param name="category">The caller file path. Do not specify manually, compiler inferred.</param>
        public static void Debug(EventId eventId,
            Exception exception,
            string message,
            [CallerFilePath] string category = "")
            => GetOrCreateLogger(Path.GetFileNameWithoutExtension(category)).LogDebug(eventId, exception, message);

        /// <summary>
        /// Writes a debug log message.
        /// </summary>
        /// <param name="eventId">The event id associated with the log.</param>
        /// <param name="message">Format string of the log message in message template format.</param>
        /// <param name="category">The caller file path. Do not specify manually, compiler inferred.</param>
        public static void Debug(EventId eventId,
            string message,
            [CallerFilePath] string category = "")
            => GetOrCreateLogger(Path.GetFileNameWithoutExtension(category)).LogDebug(eventId, message);

        /// <summary>
        /// Writes a debug log message.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message in message template format.</param>
        /// <param name="category">The caller file path. Do not specify manually, compiler inferred.</param>
        public static void Debug(Exception exception,
            string message,
            [CallerFilePath] string category = "")
            => GetOrCreateLogger(Path.GetFileNameWithoutExtension(category)).LogDebug(exception, message);

        /// <summary>
        /// Writes a debug log message.
        /// </summary>
        /// <param name="message">Format string of the log message in message template format.</param>
        /// <param name="category">The caller file path. Do not specify manually, compiler inferred.</param>
        public static void Debug(string message,
            [CallerFilePath] string category = "")
            => GetOrCreateLogger(Path.GetFileNameWithoutExtension(category)).LogDebug(message);

        /// <summary>
        /// Writes a trace log message.
        /// </summary>
        /// <param name="eventId">The event id associated with the log.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message in message template format.</param>
        /// <param name="category">The caller file path. Do not specify manually, compiler inferred.</param>
        public static void Trace(EventId eventId,
            Exception exception,
            string message,
            [CallerFilePath] string category = "")
            => GetOrCreateLogger(Path.GetFileNameWithoutExtension(category)).LogTrace(eventId, exception, message);

        /// <summary>
        /// Writes a trace log message.
        /// </summary>
        /// <param name="eventId">The event id associated with the log.</param>
        /// <param name="message">Format string of the log message in message template format.</param>
        /// <param name="category">The caller file path. Do not specify manually, compiler inferred.</param>
        public static void Trace(EventId eventId,
            string message,
            [CallerFilePath] string category = "")
            => GetOrCreateLogger(Path.GetFileNameWithoutExtension(category)).LogTrace(eventId, message);

        /// <summary>
        /// Writes a trace log message.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message in message template format.</param>
        /// <param name="category">The caller file path. Do not specify manually, compiler inferred.</param>
        public static void Trace(Exception exception,
            string message,
            [CallerFilePath] string category = "")
            => GetOrCreateLogger(Path.GetFileNameWithoutExtension(category)).LogTrace(exception, message);

        /// <summary>
        /// Writes a trace log message.
        /// </summary>
        /// <param name="message">Format string of the log message in message template format.</param>
        /// <param name="category">The caller file path. Do not specify manually, compiler inferred.</param>
        public static void Trace(string message,
            [CallerFilePath] string category = "")
            => GetOrCreateLogger(Path.GetFileNameWithoutExtension(category)).LogTrace(message);

        /// <summary>
        /// Writes a information log message.
        /// </summary>
        /// <param name="eventId">The event id associated with the log.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message in message template format.</param>
        /// <param name="category">The caller file path. Do not specify manually, compiler inferred.</param>
        public static void Information(EventId eventId,
            Exception exception,
            string message,
            [CallerFilePath] string category = "")
            => GetOrCreateLogger(Path.GetFileNameWithoutExtension(category)).LogInformation(eventId, exception, message);

        /// <summary>
        /// Writes a information log message.
        /// </summary>
        /// <param name="eventId">The event id associated with the log.</param>
        /// <param name="message">Format string of the log message in message template format.</param>
        /// <param name="category">The caller file path. Do not specify manually, compiler inferred.</param>
        public static void Information(EventId eventId,
            string message,
            [CallerFilePath] string category = "")
            => GetOrCreateLogger(Path.GetFileNameWithoutExtension(category)).LogInformation(eventId, message);

        /// <summary>
        /// Writes a information log message.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message in message template format.</param>
        /// <param name="category">The caller file path. Do not specify manually, compiler inferred.</param>
        public static void Information(Exception exception,
            string message,
            [CallerFilePath] string category = "")
            => GetOrCreateLogger(Path.GetFileNameWithoutExtension(category)).LogInformation(exception, message);

        /// <summary>
        /// Writes a information log message.
        /// </summary>
        /// <param name="message">Format string of the log message in message template format.</param>
        /// <param name="category">The caller file path. Do not specify manually, compiler inferred.</param>
        public static void Information(string message,
            [CallerFilePath] string category = "")
            => GetOrCreateLogger(Path.GetFileNameWithoutExtension(category)).LogInformation(message);

        /// <summary>
        /// Writes a warning log message.
        /// </summary>
        /// <param name="eventId">The event id associated with the log.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message in message template format.</param>
        /// <param name="category">The caller file path. Do not specify manually, compiler inferred.</param>
        public static void Warning(EventId eventId,
            Exception exception,
            string message,
            [CallerFilePath] string category = "")
            => GetOrCreateLogger(Path.GetFileNameWithoutExtension(category)).LogWarning(eventId, exception, message);

        /// <summary>
        /// Writes a warning log message.
        /// </summary>
        /// <param name="eventId">The event id associated with the log.</param>
        /// <param name="message">Format string of the log message in message template format.</param>
        /// <param name="category">The caller file path. Do not specify manually, compiler inferred.</param>
        public static void Warning(EventId eventId,
            string message,
            [CallerFilePath] string category = "")
            => GetOrCreateLogger(Path.GetFileNameWithoutExtension(category)).LogWarning(eventId, message);

        /// <summary>
        /// Writes a warning log message.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message in message template format.</param>
        /// <param name="category">The caller file path. Do not specify manually, compiler inferred.</param>
        public static void Warning(Exception exception,
            string message,
            [CallerFilePath] string category = "")
            => GetOrCreateLogger(Path.GetFileNameWithoutExtension(category)).LogWarning(exception, message);

        /// <summary>
        /// Writes a warning log message.
        /// </summary>
        /// <param name="message">Format string of the log message in message template format.</param>
        /// <param name="category">The caller file path. Do not specify manually, compiler inferred.</param>
        public static void Warning(string message,
            [CallerFilePath] string category = "")
            => GetOrCreateLogger(Path.GetFileNameWithoutExtension(category)).LogWarning(message);

        /// <summary>
        /// Writes a error log message.
        /// </summary>
        /// <param name="eventId">The event id associated with the log.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message in message template format.</param>
        /// <param name="category">The caller file path. Do not specify manually, compiler inferred.</param>
        public static void Error(EventId eventId,
            Exception exception,
            string message,
            [CallerFilePath] string category = "")
            => GetOrCreateLogger(Path.GetFileNameWithoutExtension(category)).LogError(eventId, exception, message);

        /// <summary>
        /// Writes a error log message.
        /// </summary>
        /// <param name="eventId">The event id associated with the log.</param>
        /// <param name="message">Format string of the log message in message template format.</param>
        /// <param name="category">The caller file path. Do not specify manually, compiler inferred.</param>
        public static void Error(EventId eventId,
            string message,
            [CallerFilePath] string category = "")
            => GetOrCreateLogger(Path.GetFileNameWithoutExtension(category)).LogError(eventId, message);

        /// <summary>
        /// Writes a error log message.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message in message template format.</param>
        /// <param name="category">The caller file path. Do not specify manually, compiler inferred.</param>
        public static void Error(Exception exception,
            string message,
            [CallerFilePath] string category = "")
            => GetOrCreateLogger(Path.GetFileNameWithoutExtension(category)).LogError(exception, message);

        /// <summary>
        /// Writes a error log message.
        /// </summary>
        /// <param name="message">Format string of the log message in message template format.</param>
        /// <param name="category">The caller file path. Do not specify manually, compiler inferred.</param>
        public static void Error(string message,
            [CallerFilePath] string category = "")
            => GetOrCreateLogger(Path.GetFileNameWithoutExtension(category)).LogError(message);


        /// <summary>
        /// Writes a critical log message.
        /// </summary>
        /// <param name="eventId">The event id associated with the log.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message in message template format.</param>
        /// <param name="category">The caller file path. Do not specify manually, compiler inferred.</param>
        public static void Critical(EventId eventId,
            Exception exception,
            string message,
            [CallerFilePath] string category = "")
            => GetOrCreateLogger(Path.GetFileNameWithoutExtension(category)).LogCritical(eventId, exception, message);

        /// <summary>
        /// Writes a critical log message.
        /// </summary>
        /// <param name="eventId">The event id associated with the log.</param>
        /// <param name="message">Format string of the log message in message template format.</param>
        /// <param name="category">The caller file path. Do not specify manually, compiler inferred.</param>
        public static void Critical(EventId eventId,
            string message,
            [CallerFilePath] string category = "")
            => GetOrCreateLogger(Path.GetFileNameWithoutExtension(category)).LogCritical(eventId, message);

        /// <summary>
        /// Writes a critical log message.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message in message template format.</param>
        /// <param name="category">The caller file path. Do not specify manually, compiler inferred.</param>
        public static void Critical(Exception exception,
            string message,
            [CallerFilePath] string category = "")
            => GetOrCreateLogger(Path.GetFileNameWithoutExtension(category)).LogCritical(exception, message);

        /// <summary>
        /// Writes a critical log message.
        /// </summary>
        /// <param name="message">Format string of the log message in message template format.</param>
        /// <param name="category">The caller file path. Do not specify manually, compiler inferred.</param>
        public static void Critical(string message,
            [CallerFilePath] string category = "")
            => GetOrCreateLogger(Path.GetFileNameWithoutExtension(category)).LogCritical(message);

        /// <summary>
        /// Writes a log message at the specified log level.
        /// </summary>
        /// <param name="logLevel">Entry will be written on this level.</param>
        /// <param name="message">Format string of the log message.</param>
        /// <param name="category">The caller file path. Do not specify manually, compiler inferred.</param>
        public static void Write(LogLevel logLevel,
            string message,
            [CallerFilePath] string category = "")
            => GetOrCreateLogger(Path.GetFileNameWithoutExtension(category)).Log(logLevel, message);

        /// <summary>
        /// Writes a log message at the specified log level.
        /// </summary>
        /// <param name="logLevel">Entry will be written on this level.</param>
        /// <param name="eventId">The event id associated with the log.</param>
        /// <param name="message">Format string of the log message.</param>
        /// <param name="category">The caller file path. Do not specify manually, compiler inferred.</param>
        public static void Write(LogLevel logLevel,
            EventId eventId,
            string message,
            [CallerFilePath] string category = "")
            => GetOrCreateLogger(Path.GetFileNameWithoutExtension(category)).Log(logLevel, eventId, message);

        /// <summary>
        /// Writes a log message at the specified log level.
        /// </summary>
        /// <param name="logLevel">Entry will be written on this level.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">The log message.</param>
        /// <param name="category">The caller file path. Do not specify manually, compiler inferred.</param>
        public static void Write(LogLevel logLevel,
            Exception exception,
            string message,
            [CallerFilePath] string category = "")
            => GetOrCreateLogger(Path.GetFileNameWithoutExtension(category)).Log(logLevel, exception, message);

        /// <summary>
        /// Writes a log message at the specified log level.
        /// </summary>
        /// <param name="logLevel">Entry will be written on this level.</param>
        /// <param name="eventId">The event id associated with the log.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">The log message.</param>
        /// <param name="category">The caller file path. Do not specify manually, compiler inferred.</param>
        public static void Write(LogLevel logLevel,
            EventId eventId,
            Exception exception,
            string message,
            [CallerFilePath] string category = "")
            => GetOrCreateLogger(Path.GetFileNameWithoutExtension(category)).Log(logLevel, eventId, exception, message);

        /// <summary>
        /// Waits and shuts down the logger.
        /// </summary>
        public static void Shutdown() => (LoggerProvider as UltzLoggerProvider)?.WaitAndShutdown();
    }
}