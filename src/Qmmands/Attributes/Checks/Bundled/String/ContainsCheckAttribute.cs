﻿using System;
using System.Threading.Tasks;

namespace Qmmands
{
    /// <summary>
    ///     Represents a parameter check that ensures the provided string argument contains the provided string value.
    /// </summary>
    public sealed class ContainsAttribute : ParameterCheckAttribute
    {
        /// <summary>
        ///     Gets the required string value.
        /// </summary>
        public string Value { get; }

        /// <summary>
        ///     Gets the <see cref="System.StringComparison"/> used for comparison.
        /// </summary>
        public StringComparison StringComparison { get; }

        /// <summary>
        ///     Initialises a new <see cref="ContainsAttribute"/> with the specified string value and <see cref="StringComparison.OrdinalIgnoreCase"/>.
        /// </summary>
        /// <param name="value"> The string value. </param>
        public ContainsAttribute(string value)
            : this(value, StringComparison.OrdinalIgnoreCase)
        { }

        /// <summary>
        ///     Initialises a new <see cref="ContainsAttribute"/> with the specified string value and <see cref="System.StringComparison"/>.
        /// </summary>
        /// <param name="value"> The string value. </param>
        /// <param name="comparison"> The <see cref="System.StringComparison"/> used for comparison. </param>
        public ContainsAttribute(string value, StringComparison comparison)
            : base(Utilities.IsStringType)
        {
            Value = value;
            StringComparison = comparison;
        }

        /// <inheritdoc />
        public override
#if NETCOREAPP
            ValueTask<CheckResult>
#else
            Task<CheckResult>
#endif
            CheckAsync(object argument, CommandContext context, IServiceProvider provider)
        {
            var result = (argument as string).IndexOf(Value, StringComparison) != -1
                ? CheckResult.Successful
                : CheckResult.Unsuccessful(
                    $"The provided argument must contain the {(StringComparison.IsCaseSensitive() ? "case-sensitive" : "case-insensitive")} value: {Value}.");
#if NETCOREAPP
            return result;
#else
            return Task.FromResult(result);
#endif
        }
    }
}