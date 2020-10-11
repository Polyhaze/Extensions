using System;
using Ultz.Extensions.Commands.Built;

namespace Ultz.Extensions.Commands.Attributes
{
    /// <summary>
    /// Sets a description for the <see cref="Module" />, <see cref="Command" />, or <see cref="Parameter" />.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Parameter)]
    public sealed class DescriptionAttribute : Attribute
    {
        /// <summary>
        /// Initialises a new <see cref="DescriptionAttribute" /> with the specified <paramref name="description" />.
        /// </summary>
        /// <param name="description"> The value to set. </param>
        public DescriptionAttribute(string description)
        {
            Value = description;
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public string Value { get; }
    }
}