using System;
using System.Collections.Generic;
using System.Linq;
using Ultz.Extensions.Commands.Built;
using Ultz.Extensions.Commands.Results.User;

namespace Ultz.Extensions.Commands.Parsing.ArgumentParsers.Default
{
    /// <summary>
    /// Represents a <see cref="DefaultArgumentParser.ParseAsync" /> result.
    /// </summary>
    public sealed class DefaultArgumentParserResult : ArgumentParserResult
    {
        private readonly Lazy<string> _lazyReason;

        /// <summary>
        /// Initialises a new unsuccessful <see cref="DefaultArgumentParserResult" />.
        /// </summary>
        /// <param name="command"> The <see cref="Built.Command" /> the parse failed for. </param>
        /// <param name="parameter"> The <see cref="Built.Parameter" /> the parse failed for. </param>
        /// <param name="arguments"> The successfully parsed arguments. </param>
        /// <param name="failure"> The <see cref="DefaultArgumentParserFailure" />. </param>
        /// <param name="failurePosition"> The failure position. </param>
        public DefaultArgumentParserResult(Command command, Parameter parameter,
            IReadOnlyDictionary<Parameter, object> arguments, DefaultArgumentParserFailure failure,
            int? failurePosition) : this(command, arguments)
        {
            if (!Enum.IsDefined(typeof(DefaultArgumentParserFailure), failure))
            {
                throw new ArgumentOutOfRangeException(nameof(failure));
            }

            Parameter = parameter;
            Failure = failure;
            FailurePosition = failurePosition;
        }

        /// <summary>
        /// Initialises a new successful <see cref="DefaultArgumentParserResult" />.
        /// </summary>
        /// <param name="command"> The <see cref="Built.Command" /> the parse failed for. </param>
        /// <param name="arguments"> The successfully parsed arguments. </param>
        public DefaultArgumentParserResult(Command command, IReadOnlyDictionary<Parameter, object> arguments) :
            base(arguments)
        {
            Command = command;
            _lazyReason = new Lazy<string>(() =>
            {
                switch (Failure)
                {
                    case DefaultArgumentParserFailure.UnclosedQuote:
                        return "A quotation mark was left unclosed.";

                    case DefaultArgumentParserFailure.UnexpectedQuote:
                        return "Encountered an unexpected quotation mark.";

                    case DefaultArgumentParserFailure.NoWhitespaceBetweenArguments:
                        return "Whitespace is required between arguments.";

                    case DefaultArgumentParserFailure.TooFewArguments:
                        var missingParameters = EnumerateMissingParameters().Select(x => $"'{x}'").ToArray();
                        return $"Required {(missingParameters.Length == 1 ? "parameter" : "parameters")} " +
                               $"{string.Join(", ", missingParameters)} {(missingParameters.Length == 1 ? "is" : "are")} missing.";

                    case DefaultArgumentParserFailure.TooManyArguments:
                        return "Too many arguments provided.";

                    default:
                        throw new InvalidOperationException("Invalid argument parser failure.");
                }
            }, true);
        }

        /// <summary>
        /// Gets whether the result was successful or not.
        /// </summary>
        public override bool IsSuccessful => Failure == null;

        /// <summary>
        /// Gets the failure reason of this <see cref="DefaultArgumentParserResult" />.
        /// </summary>
        public override string Reason => _lazyReason.Value;

        /// <summary>
        /// Gets the <see cref="Built.Command" />.
        /// </summary>
        public Command Command { get; }

        /// <summary>
        /// Gets the <see cref="Built.Parameter" /> the parse failed on.
        /// Can be <see langword="null" /> depending on the <see cref="Failure" />.
        /// </summary>
        public Parameter Parameter { get; }

        /// <summary>
        /// Gets the <see cref="DefaultArgumentParserFailure" />.
        /// </summary>
        public DefaultArgumentParserFailure? Failure { get; }

        /// <summary>
        /// Gets the position (index) at which the parsing failed, can be <see langword="null" /> depending on the
        /// <see cref="DefaultArgumentParserFailure" />.
        /// </summary>
        public int? FailurePosition { get; }

        /// <summary>
        /// Enumerates missing <see cref="Built.Parameter" />s.
        /// </summary>
        public IEnumerable<Parameter> EnumerateMissingParameters()
        {
            if (Parameter == null)
            {
                return Enumerable.Empty<Parameter>();
            }

            return Command.Parameters.SkipWhile(x => x != Parameter).Where(x => !x.IsOptional);
        }
    }
}