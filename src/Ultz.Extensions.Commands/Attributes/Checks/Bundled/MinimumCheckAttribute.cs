using System;
using System.Threading.Tasks;
using Ultz.Extensions.Commands.Context;
using Ultz.Extensions.Commands.Results.User;

namespace Ultz.Extensions.Commands.Attributes.Checks.Bundled
{
    /// <summary>
    /// Represents a parameter check that ensures the provided numeric/string argument has the minimum value/length.
    /// </summary>
    public sealed class MinimumAttribute : ParameterCheckAttribute
    {
        /// <summary>
        /// Initialises a new <see cref="MinimumAttribute" /> with the specified minimum value.
        /// </summary>
        /// <param name="minimum"> The minimum value. </param>
        public MinimumAttribute(double minimum)
            : base(Utilities.IsNumericOrStringType)
        {
            Minimum = minimum;
        }

        /// <summary>
        /// Gets the minimum value required.
        /// </summary>
        public double Minimum { get; }

        /// <inheritdoc />
        public override ValueTask<CheckResult> CheckAsync(object argument, CommandContext context)
        {
            var isString = argument is string;
            var value = isString
                ? (argument as string).Length
                : Convert.ToDouble(argument);
            return value >= Minimum
                ? CheckResult.Successful
                : CheckResult.Unsuccessful(
                    $"The provided argument must have a minimum {(isString ? "length" : "value")} of {Minimum}.");
        }
    }
}