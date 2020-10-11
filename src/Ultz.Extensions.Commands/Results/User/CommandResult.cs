using System.Threading.Tasks;
using Ultz.Extensions.Commands.Built;

namespace Ultz.Extensions.Commands.Results.User
{
    /// <summary>
    /// The abstract class to use for implementing results that can be returned from <see cref="Built.Command" />s.
    /// </summary>
    public abstract class CommandResult : IResult
    {
        /// <summary>
        /// The <see cref="Built.Command" /> this result was returned by.
        /// </summary>
        public Command Command { get; internal set; }

        /// <summary>
        /// Gets whether the result was successful or not.
        /// </summary>
        public abstract bool IsSuccessful { get; }

        /// <summary>
        /// Implicitly wraps the provided <see cref="CommandResult" /> in a <see cref="ValueTask{TResult}" />.
        /// </summary>
        /// <param name="result"> The result to wrap. </param>
        public static implicit operator ValueTask<CommandResult>(CommandResult result)
        {
            return new ValueTask<CommandResult>(result);
        }
    }
}