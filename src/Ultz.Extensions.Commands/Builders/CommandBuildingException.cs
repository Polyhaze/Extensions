using System;
using Ultz.Extensions.Commands.Built;

namespace Ultz.Extensions.Commands.Builders
{
    /// <summary>
    /// Represents errors that occur during building <see cref="Command" />s.
    /// </summary>
    public sealed class CommandBuildingException : Exception
    {
        internal CommandBuildingException(CommandBuilder commandBuilder, string message) : base(message)
        {
            CommandBuilder = commandBuilder;
        }

        /// <summary>
        /// Gets the <see cref="Builders.CommandBuilder" /> that failed to build.
        /// </summary>
        public CommandBuilder CommandBuilder { get; }
    }
}