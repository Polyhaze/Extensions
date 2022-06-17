using System;
using Ultz.Extensions.Commands.Built;

namespace Ultz.Extensions.Commands.Builders
{
    /// <summary>
    /// Represents errors that occur during building <see cref="Module" />s.
    /// </summary>
    public sealed class ModuleBuildingException : Exception
    {
        internal ModuleBuildingException(ModuleBuilder moduleBuilder, string message) : base(message)
        {
            ModuleBuilder = moduleBuilder;
        }

        /// <summary>
        /// Gets the <see cref="Builders.ModuleBuilder" /> that failed to build.
        /// </summary>
        public ModuleBuilder ModuleBuilder { get; }
    }
}