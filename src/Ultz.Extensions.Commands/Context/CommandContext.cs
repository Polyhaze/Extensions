using System;
using System.Collections.Generic;
using Ultz.Extensions.Commands.Built;

namespace Ultz.Extensions.Commands.Context
{
    /// <summary>
    /// The base class for custom command contexts.
    /// </summary>
    /// <remarks>
    /// The properties this class exposes may not always be present, depending on how the <see cref="Built.Command" /> was
    /// executed.
    /// </remarks>
    public abstract class CommandContext
    {
        private List<object> _arguments;

        internal object[] InternalArguments;

        /// <summary>
        /// Initialises a new instance of the <see cref="CommandContext" />.
        /// </summary>
        /// <param name="serviceProvider">
        /// The <see cref="IServiceProvider" /> to use for execution. Passing <see langword="null" /> will make it default to a
        /// <see cref="DummyServiceProvider" />.
        /// </param>
        protected CommandContext(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider ?? DummyServiceProvider.Instance;
        }

        /// <summary>
        /// Gets the currently executed <see cref="Built.Command" />.
        /// </summary>
        public Command Command { get; internal set; }

        /// <summary>
        /// Gets the alias used.
        /// <see langword="null" /> if the <see cref="Built.Command" /> was invoked without searching.
        /// </summary>
        public string Alias { get; internal set; }

        /// <summary>
        /// Gets the alias path used.
        /// <see langword="null" /> if the <see cref="Built.Command" /> was invoked without searching.
        /// </summary>
        public IReadOnlyList<string> Path { get; internal set; }

        /// <summary>
        /// Gets the raw arguments.
        /// <see langword="null" /> if the <see cref="Built.Command" /> was invoked with already parsed arguments.
        /// </summary>
        public string RawArguments { get; internal set; }

        /// <summary>
        /// Gets the parsed arguments.
        /// </summary>
        public IReadOnlyList<object> Arguments
        {
            get
            {
                var arguments = _arguments;
                return arguments ?? (_arguments = new List<object>(InternalArguments ?? Array.Empty<object>()));
            }
        }

        /// <summary>
        /// Gets the <see cref="IServiceProvider" /> used for execution.
        /// </summary>
        public IServiceProvider ServiceProvider { get; }
    }
}