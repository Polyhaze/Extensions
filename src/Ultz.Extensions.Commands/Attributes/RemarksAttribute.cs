using System;
using Ultz.Extensions.Commands.Built;

namespace Ultz.Extensions.Commands.Attributes
{
    /// <summary>
    /// Sets remarks for the <see cref="Module" />, <see cref="Command" />, or <see cref="Parameter" />.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Parameter)]
    public sealed class RemarksAttribute : Attribute
    {
        /// <summary>
        /// Initialises a new <see cref="RemarksAttribute" /> with the specified <paramref name="remarks" />.
        /// </summary>
        /// <param name="remarks"> The value to set. </param>
        public RemarksAttribute(string remarks)
        {
            Value = remarks;
        }

        /// <summary>
        /// Gets the remarks.
        /// </summary>
        public string Value { get; }
    }
}