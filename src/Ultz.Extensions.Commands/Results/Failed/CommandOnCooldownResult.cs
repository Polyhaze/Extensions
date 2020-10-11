using System;
using System.Collections.Generic;
using System.Linq;
using Ultz.Extensions.Commands.Built;
using Ultz.Extensions.Commands.Cooldown;

namespace Ultz.Extensions.Commands.Results.Failed
{
    /// <summary>
    /// Represents the command being on a cooldown.
    /// </summary>
    public sealed class CommandOnCooldownResult : FailedResult
    {
        private readonly Lazy<string> _lazyReason;

        internal CommandOnCooldownResult(Command command,
            IReadOnlyList<(Cooldown.Cooldown Cooldown, TimeSpan RetryAfter)> cooldowns)
        {
            Command = command;
            Cooldowns = cooldowns;

            _lazyReason = new Lazy<string>(() => cooldowns.Count == 1
                    ? $"Command {command} is on a '{cooldowns[0].Cooldown.BucketType}' cooldown. Retry after {cooldowns[0].RetryAfter}."
                    : $"Command {command} is on multiple cooldowns: {string.Join(", ", cooldowns.Select(x => $"'{x.Cooldown.BucketType}' - retry after {x.RetryAfter}"))}",
                true);
        }

        /// <summary>
        /// Gets the reason of this failed result.
        /// </summary>
        public override string Reason => _lazyReason.Value;

        /// <summary>
        /// Gets the <see cref="Built.Command" /> that is on cooldown.
        /// </summary>
        public Command Command { get; }

        /// <summary>
        /// Gets the <see cref="Cooldown" />s and <see cref="TimeSpan" />s after which it is safe to retry.
        /// </summary>
        public IReadOnlyList<(Cooldown.Cooldown Cooldown, TimeSpan RetryAfter)> Cooldowns { get; }
    }
}