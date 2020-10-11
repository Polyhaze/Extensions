using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Ultz.Extensions.Commands.Built;
using Ultz.Extensions.Commands.Parsing.ArgumentParsers;

namespace Ultz.Extensions.Commands.Results.User
{
    /// <summary>
    /// The base interface for <see cref="IArgumentParser.ParseAsync" /> results.
    /// </summary>
    public abstract class ArgumentParserResult : IResult
    {
        private static readonly IReadOnlyDictionary<Parameter, object> _emptyParameterDictionary =
            new ReadOnlyDictionary<Parameter, object>(new Dictionary<Parameter, object>(0));

        /// <summary>
        /// Initialises a new <see cref="ArgumentParserResult" />.
        /// </summary>
        /// <param name="arguments"> The successfully parsed arguments. </param>
        protected ArgumentParserResult(IReadOnlyDictionary<Parameter, object> arguments)
        {
            Arguments = arguments ?? _emptyParameterDictionary;
        }

        /// <summary>
        /// Gets the failure reason of this <see cref="ArgumentParserResult" />.
        /// </summary>
        public abstract string Reason { get; }

        /// <summary>
        /// Gets the successfully parsed arguments.
        /// </summary>
        public IReadOnlyDictionary<Parameter, object> Arguments { get; }

        /// <summary>
        /// Gets whether the result was successful or not.
        /// </summary>
        public abstract bool IsSuccessful { get; }

        /// <summary>
        /// Implicitly wraps the provided <see cref="ArgumentParserResult" /> in a <see cref="ValueTask" />.
        /// </summary>
        /// <param name="result"> The result to wrap. </param>
        public static implicit operator ValueTask<ArgumentParserResult>(ArgumentParserResult result)
        {
            return new ValueTask<ArgumentParserResult>(result);
        }
    }
}