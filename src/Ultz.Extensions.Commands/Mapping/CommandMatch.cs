using System.Collections.Generic;
using System.Collections.Immutable;
using Ultz.Extensions.Commands.Built;

namespace Ultz.Extensions.Commands.Mapping
{
    /// <summary>
    /// Represents a found <see cref="Built.Command" />, the path to it, and raw arguments.
    /// </summary>
    public sealed class CommandMatch
    {
        internal CommandMatch(Command command, string alias, IReadOnlyList<string> path, string rawArguments)
        {
            Command = command;
            Alias = alias;
            Path = path.ToImmutableArray();
            RawArguments = rawArguments;
        }

        /// <summary>
        /// Gets the found <see cref="Built.Command" />.
        /// </summary>
        public Command Command { get; }

        /// <summary>
        /// Gets the matched alias.
        /// </summary>
        public string Alias { get; }

        /// <summary>
        /// Gets the alias path to the found <see cref="Built.Command" /> in the order they were matched.
        /// </summary>
        public IReadOnlyList<string> Path { get; }

        /// <summary>
        /// Gets the extracted raw arguments after the matched aliases.
        /// </summary>
        public string RawArguments { get; }
    }
}