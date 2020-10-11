using System;
using System.Collections.Generic;
using Ultz.Extensions.Commands.Attributes.Checks;
using Ultz.Extensions.Commands.Built;
using Ultz.Extensions.Commands.Results.User;

namespace Ultz.Extensions.Commands.Results.Failed.Checks
{
    /// <summary>
    /// Represents a <see cref="Built.Command" />'s or a <see cref="Built.Module" />'s checks failure.
    /// </summary>
    public sealed class ChecksFailedResult : FailedResult
    {
        private readonly Lazy<string> _lazyReason;

        internal ChecksFailedResult(Command command,
            IReadOnlyList<(CheckAttribute Check, CheckResult Result)> failedChecks)
        {
            Command = command;
            FailedChecks = failedChecks;
            _lazyReason =
                new Lazy<string>(
                    () =>
                        $"{(FailedChecks.Count == 1 ? "One check" : "Multiple checks")} failed for the command {Command}.",
                    true);
        }

        internal ChecksFailedResult(Module module,
            IReadOnlyList<(CheckAttribute Check, CheckResult Result)> failedChecks)
        {
            Module = module;
            FailedChecks = failedChecks;
            _lazyReason =
                new Lazy<string>(
                    () =>
                        $"{(FailedChecks.Count == 1 ? "One check" : "Multiple checks")} failed for the module {Module}.",
                    true);
        }

        /// <summary>
        /// Gets the reason of this failed result.
        /// </summary>
        public override string Reason => _lazyReason.Value;

        /// <summary>
        /// Gets the <see cref="Built.Module" /> the checks failed on, <see langword="null" /> if <see cref="Command" /> has a
        /// value.
        /// </summary>
        public Module Module { get; }

        /// <summary>
        /// Gets the <see cref="Built.Command" /> the checks failed on, <see langword="null" /> if <see cref="Module" /> has a
        /// value.
        /// </summary>
        public Command Command { get; }

        /// <summary>
        /// Gets the checks that failed with their respective results.
        /// </summary>
        public IReadOnlyList<(CheckAttribute Check, CheckResult Result)> FailedChecks { get; }
    }
}