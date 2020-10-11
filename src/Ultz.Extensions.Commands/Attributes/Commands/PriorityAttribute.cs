using System;
using Ultz.Extensions.Commands.Built;

namespace Ultz.Extensions.Commands.Attributes.Commands
{
    /// <summary>
    /// Sets a priority for the <see cref="Command" />.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class PriorityAttribute : Attribute
    {
        /// <summary>
        /// Initialises a new <see cref="PriorityAttribute" /> with the specified priority.
        /// </summary>
        /// <param name="priority"> The priority to set. </param>
        /// <remarks>
        /// The <see cref="CommandService" /> will try to execute higher priority <see cref="Command" />s first.
        /// </remarks>
        public PriorityAttribute(int priority)
        {
            Value = priority;
        }

        /// <summary>
        /// Gets the priority.
        /// </summary>
        public int Value { get; }
    }
}