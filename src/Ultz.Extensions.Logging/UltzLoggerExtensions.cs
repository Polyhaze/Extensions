using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace Ultz.Extensions.Logging
{
    /// <summary>
    /// Contains useful extension methods for working with the Ultz Logger.
    /// </summary>
    public static class UltzLoggerExtensions
    {
        /// <summary>
        /// Adds the <see cref="UltzLoggerProvider"/> to the <see cref="ILoggingBuilder"/>'s <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="builder">The <see cref="ILoggingBuilder"/> to add the <see cref="UltzLoggerProvider"/> to.</param>
        /// <returns>The instance passed in, for method chaining.</returns>
        public static ILoggingBuilder AddUltzLogger(this ILoggingBuilder builder)
        {
            builder.AddConfiguration();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, UltzLoggerProvider>());
            return builder;
        }

        /// <summary>
        /// Adds the <see cref="UltzLoggerProvider"/> to the <see cref="ILoggingBuilder"/>'s <see cref="IServiceCollection"/> with custom configuration.
        /// </summary>
        /// <param name="builder">The <see cref="ILoggingBuilder"/> to add the <see cref="UltzLoggerProvider"/> to.</param>
        /// <param name="configure">A delegate called when constructing the <see cref="UltzLoggerProvider"/>. This is where the custom configuration should be added.</param>
        /// <returns>The instance passed in, for method chaining.</returns>
        public static ILoggingBuilder AddUltzLogger(this ILoggingBuilder builder, Action<IUltzLoggerObject> configure)
        {
            if (configure is null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            builder.AddConfiguration();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, UltzLoggerProvider>(impl =>
            {
                var ret = new UltzLoggerProvider();
                configure(ret);
                return ret;
            }));

            return builder;
        }

        /// <summary>
        /// Adds the given <see cref="LogLevel"/>s to the logger object's <see cref="UltzLogger.LogLevels" /> list.
        /// </summary>
        /// <param name="opts">The logger object to configure.</param>
        /// <param name="logLevels">The <see cref="LogLevel"/>s to add.</param>
        /// <returns>The instance passed in, for method chaining.</returns>
        public static IUltzLoggerObject WithLogLevels(this IUltzLoggerObject opts, params LogLevel[] logLevels)
        {
            opts.LogLevels.AddRange(logLevels);
            return opts;
        }

        /// <summary>
        /// Adds the given <see cref="LogLevel"/> string representations to the logger object's <see cref="UltzLogger.LogLevelStrings" />.
        /// </summary>
        /// <param name="opts">The logger object to configure.</param>
        /// <param name="logLevelStrings">The <see cref="LogLevel"/> string representations to add.</param>
        /// <returns>The instance passed in, for method chaining.</returns>
        public static IUltzLoggerObject WithLogLevelStrings(this IUltzLoggerObject opts,
            params (LogLevel, string)[] logLevelStrings)
        {
            foreach (var (logLevel, @string) in logLevelStrings)
            {
                opts.LogLevelStrings[logLevel] = @string;
            }

            return opts;
        }

        /// <summary>
        /// Adds the given <see cref="LogLevel"/> string representations to the logger object's <see cref="UltzLogger.LogLevelStrings" />.
        /// </summary>
        /// <param name="opts">The logger object to configure.</param>
        /// <param name="logLevelStrings">The <see cref="LogLevel"/> string representations to add.</param>
        /// <returns>The instance passed in, for method chaining.</returns>
        public static IUltzLoggerObject WithLogLevelStrings(this IUltzLoggerObject opts,
            params KeyValuePair<LogLevel, string>[] logLevelStrings)
        {
            foreach (var logLevelString in logLevelStrings)
            {
                opts.LogLevelStrings[logLevelString.Key] = logLevelString.Value;
            }

            return opts;
        }

        /// <summary>
        /// Configures the logger object to use the specified <see cref="UltzLogger.MessageFormat"/>.
        /// </summary>
        /// <param name="opts">The logger object to configure.</param>
        /// <param name="format">The format to use.</param>
        /// <returns>The instance passed in, for method chaining.</returns>
        /// <remarks>
        /// Likely only used if the <see cref="UltzLogger.MessageFormat"/> is unchanged.
        /// </remarks>
        public static IUltzLoggerObject WithMessageFormat(this IUltzLoggerObject opts, string format)
        {
            opts.MessageFormat = format;
            return opts;
        }

        /// <summary>
        /// Configures the logger object to use the specified <see cref="UltzLogger.MessageFormatter"/>.
        /// </summary>
        /// <param name="opts">The logger object to configure.</param>
        /// <param name="formatter">The formatter to use.</param>
        /// <returns>The instance passed in, for method chaining.</returns>
        public static IUltzLoggerObject WithMessageFormatter(this IUltzLoggerObject opts,
            Func<LogLevel, EventId, string, string>? formatter)
        {
            opts.MessageFormatter = formatter;
            return opts;
        }

        /// <summary>
        /// Adds the given <see cref="IOutput"/>s to the logger object's <see cref="UltzLogger.Outputs" />.
        /// </summary>
        /// <param name="opts">The logger object to configure.</param>
        /// <param name="outputs">The outputs to add.</param>
        /// <returns>The instance passed in, for method chaining.</returns>
        public static IUltzLoggerObject WithOutputs(this IUltzLoggerObject opts, params IOutput[] outputs)
        {
            opts.Outputs.AddRange(outputs);
            return opts;
        }

        /// <summary>
        /// Configures the logger object to use the specified <see cref="UltzLogger.ScopeProvider"/>.
        /// </summary>
        /// <param name="opts">The logger object to configure.</param>
        /// <param name="provider">The <see cref="IExternalScopeProvider"/> to use.</param>
        /// <returns>The instance passed in, for method chaining.</returns>
        public static IUltzLoggerObject WithScopeProvider(this IUltzLoggerObject opts,
            IExternalScopeProvider? provider)
        {
            opts.ScopeProvider = provider;
            return opts;
        }
    }
}