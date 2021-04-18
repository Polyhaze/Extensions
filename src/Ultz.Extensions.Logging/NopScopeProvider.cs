using System;
using Microsoft.Extensions.Logging;

namespace Ultz.Extensions.Logging
{
    /// <summary>
    /// An external scope provider that does nothing.
    /// </summary>
    public class NopScopeProvider : IExternalScopeProvider
    {
        private NopScopeProvider()
        {
        }

        /// <summary>
        /// Returns a cached instance of <see cref="NopScopeProvider" />.
        /// </summary>
        public static IExternalScopeProvider Instance { get; } = new NopScopeProvider();

        /// <inheritdoc />
        void IExternalScopeProvider.ForEachScope<TState>(Action<object, TState> callback, TState state)
        {
        }

        /// <inheritdoc />
        IDisposable IExternalScopeProvider.Push(object state)
        {
            return NopScope.Instance;
        }

        internal class NopScope : IDisposable
        {
            private NopScope()
            {
            }

            public static NopScope Instance { get; } = new();

            /// <inheritdoc />
            public void Dispose()
            {
            }
        }
    }
}