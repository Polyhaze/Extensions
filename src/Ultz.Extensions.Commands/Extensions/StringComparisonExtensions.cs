using System;

namespace Ultz.Extensions.Commands.Extensions
{
    public static class StringComparisonExtensions
    {
        /// <summary>
        ///     Converts a <see cref="StringComparison"/> value to its corresponding <see cref="StringComparer"/>.
        /// </summary>
        /// <param name="comparison">The comparision.</param>
        /// <returns>The comparer.</returns>
        public static StringComparer ToComparer(this StringComparison comparison)
        {
            return comparison switch
            {
                StringComparison.CurrentCulture => StringComparer.CurrentCulture,
                StringComparison.CurrentCultureIgnoreCase => StringComparer.CurrentCultureIgnoreCase,
                StringComparison.InvariantCulture => StringComparer.InvariantCulture,
                StringComparison.InvariantCultureIgnoreCase => StringComparer.InvariantCultureIgnoreCase,
                StringComparison.Ordinal => StringComparer.Ordinal,
                StringComparison.OrdinalIgnoreCase => StringComparer.OrdinalIgnoreCase,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}