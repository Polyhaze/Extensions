using System;
using System.Threading.Tasks;
using Ultz.Extensions.Commands.Built;
using Ultz.Extensions.Commands.Context;
using Ultz.Extensions.Commands.Results.User;

namespace Ultz.Extensions.Commands.Attributes.Checks
{
    /// <summary>
    /// Represents a <see cref="Built.Parameter" /> check that has to succeed before the <see cref="Command" /> can be
    /// executed.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
    public abstract class ParameterCheckAttribute : Attribute
    {
        /// <summary>
        /// Initialises a new <see cref="ParameterCheckAttribute" /> with the predicate that determines what <see cref="Type" />s
        /// are supported.
        /// </summary>
        /// <param name="predicate"> The optional <see cref="Predicate{T}" /> that determines what types are supported. </param>
        protected ParameterCheckAttribute(Predicate<Type> predicate = null)
        {
            Predicate = predicate;
        }

        /// <summary>
        /// Gets the <see cref="Predicate{T}" /> that determines which types are supported by this
        /// <see cref="ParameterCheckAttribute" />.
        /// If <see langword="null" />, any <see cref="Type" /> of parameters are accepted.
        /// </summary>
        public Predicate<Type> Predicate { get; }

        /// <summary>
        /// Gets the <see cref="Built.Parameter" /> this <see cref="ParameterCheckAttribute" /> is for.
        /// </summary>
        public Parameter Parameter { get; internal set; }

        /// <summary>
        /// Gets or sets the group for this check.
        /// </summary>
        /// <remarks>
        /// Grouped checks act as if they were put side by side with the logical OR operator (||) in between.
        /// </remarks>
        public string Group { get; set; }

        /// <summary>
        /// Determines whether the <paramref name="argument" /> is valid for the <see cref="Built.Parameter" /> in given
        /// circumstances.
        /// </summary>
        /// <param name="argument"> The value given to this <see cref="Built.Parameter" />. </param>
        /// <param name="context"> The <see cref="CommandContext" /> used during execution. </param>
        /// <returns>
        /// A <see cref="CheckResult" /> which determines whether this <see cref="ParameterCheckAttribute" /> succeeded or not.
        /// </returns>
        public abstract ValueTask<CheckResult> CheckAsync(object argument, CommandContext context);
    }
}