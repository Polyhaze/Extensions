using System;
using Ultz.Extensions.Commands.Built;

namespace Ultz.Extensions.Commands.Builders
{
    /// <summary>
    /// Represents errors that occur during building <see cref="Parameter" />s.
    /// </summary>
    public sealed class ParameterBuildingException : Exception
    {
        internal ParameterBuildingException(ParameterBuilder parameterBuilder, string message) : base(message)
        {
            ParameterBuilder = parameterBuilder;
        }

        /// <summary>
        /// Gets the <see cref="Builders.ParameterBuilder" /> that failed to build.
        /// </summary>
        public ParameterBuilder ParameterBuilder { get; }
    }
}