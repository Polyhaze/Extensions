using System;
using Ultz.Extensions.Commands.Built;

namespace Ultz.Extensions.Commands.Attributes.Modules
{
    /// <summary>
    /// Prevents <see cref="CommandService.AddModules" />
    /// from automatically adding the marked class as a <see cref="Module" />.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DoNotAddAttribute : Attribute
    {
    }
}