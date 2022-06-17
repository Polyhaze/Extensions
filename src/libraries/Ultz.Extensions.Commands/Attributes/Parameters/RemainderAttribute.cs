using System;
using Ultz.Extensions.Commands.Built;
using Ultz.Extensions.Commands.Parsing.ArgumentParsers.Default;

namespace Ultz.Extensions.Commands.Attributes.Parameters
{
    /// <summary>
    /// Marks the <see cref="Parameter" /> as a remainder parameter.
    /// </summary>
    /// <remarks>
    /// Remainder parameters are the last parameters of <see cref="Command" />s.
    /// Using the <see cref="DefaultArgumentParser" /> remainder parameters can consist
    /// of multiple words without the need of using quotation marks."/>.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class RemainderAttribute : Attribute
    {
    }
}