using System;
using Ultz.Extensions.Commands.Built;

namespace Ultz.Extensions.Commands.Mapping
{
    /// <summary>
    /// Represents errors that occur during mapping <see cref="Built.Command" />s.
    /// </summary>
    public sealed class CommandMappingException : Exception
    {
        internal CommandMappingException(Command command, string segment, string message) : base(message)
        {
            Command = command;
            Segment = segment;
        }

        /// <summary>
        /// Gets the <see cref="Built.Command" /> this exception occurred for.
        /// </summary>
        public Command Command { get; }

        /// <summary>
        /// Gets the segment to the map this exception occurred at.
        /// <see langword="null" /> if there were no segments (an attempt was made to map an ungrouped <see cref="Built.Command" />
        /// without any aliases)
        /// </summary>
        public string Segment { get; }
    }
}