using System;
using System.Collections.Generic;
using Ultz.Extensions.Commands.Attributes.Checks;
using Ultz.Extensions.Commands.Built;
using Ultz.Extensions.Commands.Results.User;

namespace Ultz.Extensions.Commands.Results.Failed.Checks
{
    /// <summary>
    /// Represents a <see cref="Built.Parameter" />'s checks failure.
    /// </summary>
    public sealed class ParameterChecksFailedResult : FailedResult
    {
        private readonly Lazy<string> _lazyReason;

        internal ParameterChecksFailedResult(Parameter parameter, object argument,
            IReadOnlyList<(ParameterCheckAttribute Check, CheckResult Result)> failedChecks)
        {
            Parameter = parameter;
            Argument = argument;
            FailedChecks = failedChecks;
            _lazyReason = new Lazy<string>(
                () =>
                    $"{(FailedChecks.Count == 1 ? "One check" : "Multiple checks")} failed for the parameter {Parameter.Name} in the command {Parameter.Command}.",
                true);
        }

        /// <summary>
        /// Gets the reason of this failed result.
        /// </summary>
        public override string Reason => _lazyReason.Value;

        /// <summary>
        /// Gets the <see cref="Built.Parameter" /> the checks failed on.
        /// </summary>
        public Parameter Parameter { get; }

        /// <summary>
        /// Gets the argument the checks failed on.
        /// </summary>
        public object Argument { get; }

        /// <summary>
        /// Gets the checks that failed with their error reasons.
        /// </summary>
        public IReadOnlyList<(ParameterCheckAttribute Check, CheckResult Result)> FailedChecks { get; }
    }
}