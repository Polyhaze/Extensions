using Ultz.Extensions.Commands.Built;

namespace Ultz.Extensions.Commands.Results.Failed
{
    /// <summary>
    /// Represents the command being disabled.
    /// </summary>
    public sealed class CommandDisabledResult : FailedResult
    {
        internal CommandDisabledResult(Command command)
        {
            Command = command;
            Reason = $"Command {command} is disabled.";
        }

        /// <summary>
        /// Gets the reason of this failed result.
        /// </summary>
        public override string Reason { get; }

        /// <summary>
        /// Gets the <see cref="Built.Command" /> that is disabled.
        /// </summary>
        public Command Command { get; }
    }
}