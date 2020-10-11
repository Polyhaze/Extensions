using System.Threading.Tasks;
using Ultz.Extensions.Commands.Attributes.Checks;

namespace Ultz.Extensions.Commands.Results.User
{
    /// <summary>
    /// Represents a <see cref="CheckAttribute" />'s result.
    /// </summary>
    public class CheckResult : IResult
    {
        /// <summary>
        /// Initialises a new successful <see cref="CheckResult" />.
        /// </summary>
        public CheckResult()
        {
        }

        /// <summary>
        /// Initialises a new failed <see cref="CheckResult" /> with the specified error reason.
        /// </summary>
        /// <param name="reason"> The error reason. </param>
        public CheckResult(string reason)
        {
            Reason = reason;
        }

        /// <summary>
        /// Gets the error reason.
        /// </summary>
        public virtual string Reason { get; }

        /// <summary>
        /// Gets a successful <see cref="CheckResult" />.
        /// </summary>
        public static CheckResult Successful
            => new CheckResult();

        /// <summary>
        /// Gets whether the result was successful or not.
        /// </summary>
        public virtual bool IsSuccessful => Reason == null;

        /// <summary>
        /// Initialises a new unsuccessful <see cref="CheckResult" />.
        /// </summary>
        /// <param name="reason"> The error reason. </param>
        /// <returns>
        /// An unsuccessful <see cref="CheckResult" />.
        /// </returns>
        public static CheckResult Unsuccessful(string reason)
        {
            return new CheckResult(reason);
        }

        /// <summary>
        /// Returns <see cref="Reason" />.
        /// </summary>
        /// <returns>
        /// The <see cref="Reason" />.
        /// </returns>
        public override string ToString()
        {
            return Reason;
        }

        /// <summary>
        /// Implicitly wraps the provided <see cref="CheckResult" /> in a <see cref="ValueTask{TResult}" />.
        /// </summary>
        /// <param name="result"> The result to wrap. </param>
        public static implicit operator ValueTask<CheckResult>(CheckResult result)
        {
            return new ValueTask<CheckResult>(result);
        }
    }
}