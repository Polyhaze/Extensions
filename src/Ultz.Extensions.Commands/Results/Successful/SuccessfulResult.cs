namespace Ultz.Extensions.Commands.Results.Successful
{
    /// <summary>
    /// Represents a successful result.
    /// </summary>
    public sealed class SuccessfulResult : IResult
    {
        /// <summary>
        /// Gets <see langword="true" />.
        /// </summary>
        public bool IsSuccessful => true;
    }
}