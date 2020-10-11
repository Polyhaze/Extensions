using System.Collections.Generic;
using Ultz.Extensions.Commands.Built;

namespace Ultz.Extensions.Commands.Results.Failed
{
    /// <summary>
    /// Represents multiple failed overloads.
    /// </summary>
    public sealed class OverloadsFailedResult : FailedResult
    {
        internal OverloadsFailedResult(IReadOnlyDictionary<Command, FailedResult> failedOverloads)
        {
            FailedOverloads = failedOverloads;
        }

        /// <summary>
        /// Gets the reason of this failed result.
        /// </summary>
        /// s
        public override string Reason => "Failed to find a matching overload.";

        /// <summary>
        /// Gets the failed overloads with their respective failed results.
        /// </summary>
        public IReadOnlyDictionary<Command, FailedResult> FailedOverloads { get; }
    }
}