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
    internal sealed class UltzLogger : ILogger
    {
        private readonly string _name;
        private readonly UltzLoggerProvider _parent;

        internal UltzLogger(string categoryName, UltzLoggerProvider parent)
        {
            _name = categoryName;
            _parent = parent;
        }

        /// <inheritdoc />
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter) =>
            _parent.Log(_name, logLevel, eventId, state, exception, formatter);

        /// <inheritdoc />
        public bool IsEnabled(LogLevel logLevel) => _parent.LogLevels.Contains(logLevel);

        /// <inheritdoc />
        public IDisposable BeginScope<TState>(TState state)
            => _parent.ScopeProvider?.Push(state) ?? NopScopeProvider.NopScope.Instance;
    }
}