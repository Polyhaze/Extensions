namespace Ultz.Extensions.Commands.Results.Failed
{
    /// <summary>
    /// Represents no commands matching the provided input.
    /// </summary>
    public sealed class CommandNotFoundResult : FailedResult
    {
        internal CommandNotFoundResult()
        {
        }

        /// <summary>
        /// Gets the reason of this failed result.
        /// </summary>
        public override string Reason => "No command found matching the provided input.";
    }
}