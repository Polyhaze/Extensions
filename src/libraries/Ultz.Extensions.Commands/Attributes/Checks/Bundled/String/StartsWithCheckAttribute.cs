using System;
using System.Threading.Tasks;
using Ultz.Extensions.Commands.Context;
using Ultz.Extensions.Commands.Results.User;

namespace Ultz.Extensions.Commands.Attributes.Checks.Bundled.String
{
    /// <summary>
    /// Represents a parameter check that ensures the provided string argument starts with the provided string value.
    /// </summary>
    public sealed class StartsWithAttribute : ParameterCheckAttribute
    {
        /// <summary>
        /// Initialises a new <see cref="StartsWithAttribute" /> with the specified string value and
        /// <see cref="StringComparison.OrdinalIgnoreCase" />.
        /// </summary>
        /// <param name="value"> The string value. </param>
        public StartsWithAttribute(string value)
            : this(value, StringComparison.OrdinalIgnoreCase)
        {
        }

        /// <summary>
        /// Initialises a new <see cref="StartsWithAttribute" /> with the specified string value and
        /// <see cref="System.StringComparison" />.
        /// </summary>
        /// <param name="value"> The string value. </param>
        /// <param name="comparison"> The <see cref="System.StringComparison" /> used for comparison. </param>
        public StartsWithAttribute(string value, StringComparison comparison)
            : base(Utilities.IsStringType)
        {
            Value = value;
            StringComparison = comparison;
        }

        /// <summary>
        /// Gets the required string value.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Gets the <see cref="System.StringComparison" /> used for comparison.
        /// </summary>
        public StringComparison StringComparison { get; }

        /// <inheritdoc />
        public override ValueTask<CheckResult> CheckAsync(object argument, CommandContext context)
        {
            return (argument as string).StartsWith(Value, StringComparison)
                ? CheckResult.Successful
                : CheckResult.Unsuccessful(
                    $"The provided argument must start with the {(StringComparison.IsCaseSensitive() ? "case-sensitive" : "case-insensitive")} value: {Value}.");
        }
    }
}