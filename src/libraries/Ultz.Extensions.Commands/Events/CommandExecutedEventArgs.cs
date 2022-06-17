using System;
using Ultz.Extensions.Commands.Built;
using Ultz.Extensions.Commands.Context;
using Ultz.Extensions.Commands.Results.User;

namespace Ultz.Extensions.Commands.Events
{
    /// <summary>
    /// Represents the event data for when a command was executed.
    /// </summary>
    public sealed class CommandExecutedEventArgs : EventArgs
    {
        /// <summary>
        /// Initialises a new <see cref="CommandExecutedEventArgs" />.
        /// </summary>
        /// <param name="result"> The <see cref="CommandResult" /> from the <see cref="Command" />. </param>
        /// <param name="context"> The <see cref="CommandContext" /> used for execution. </param>
        public CommandExecutedEventArgs(CommandResult result, CommandContext context)
        {
            Result = result;
            Context = context;
        }

        /// <summary>
        /// Gets the <see cref="CommandResult" /> of the <see cref="Command" />.
        /// <see langword="null" /> if the <see cref="Command" /> did not return anything.
        /// </summary>
        public CommandResult Result { get; }

        /// <summary>
        /// Gets the <see cref="CommandContext" /> used for execution.
        /// </summary>
        public CommandContext Context { get; }
    }
}