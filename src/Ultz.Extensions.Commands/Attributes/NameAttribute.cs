using System;
using Ultz.Extensions.Commands.Built;

namespace Ultz.Extensions.Commands.Attributes
{
    /// <summary>
    /// Sets a name for the <see cref="Module" />, <see cref="Command" />, or <see cref="Parameter" />.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Parameter)]
    public sealed class NameAttribute : Attribute
    {
        /// <summary>
        /// Initialises a new <see cref="NameAttribute" /> with the specified <paramref name="name" />.
        /// </summary>
        /// <param name="name"> The value to set. </param>
        public NameAttribute(string name)
        {
            Value = name;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Value { get; }
    }
}