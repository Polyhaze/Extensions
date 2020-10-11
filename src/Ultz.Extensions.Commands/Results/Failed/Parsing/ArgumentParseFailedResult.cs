using Ultz.Extensions.Commands.Built;
using Ultz.Extensions.Commands.Context;
using Ultz.Extensions.Commands.Parsing.ArgumentParsers;
using Ultz.Extensions.Commands.Results.User;

namespace Ultz.Extensions.Commands.Results.Failed.Parsing
{
    /// <summary>
    /// Represents an argument parse failure.
    /// </summary>
    public sealed class ArgumentParseFailedResult : FailedResult
    {
        internal ArgumentParseFailedResult(CommandContext context, ArgumentParserResult parserResult)
        {
            Command = context.Command;
            RawArguments = context.RawArguments;
            ParserResult = parserResult;
        }

        /// <summary>
        /// Gets the reason of this failed result.
        /// </summary>
        public override string Reason => ParserResult.Reason;

        /// <summary>
        /// Gets the <see cref="Built.Command" /> the parse failed for.
        /// </summary>
        public Command Command { get; }

        /// <summary>
        /// Gets the raw arguments.
        /// </summary>
        public string RawArguments { get; }

        /// <summary>
        /// Gets the result returned from <see cref="IArgumentParser.ParseAsync" />.
        /// </summary>
        public ArgumentParserResult ParserResult { get; }
    }
}