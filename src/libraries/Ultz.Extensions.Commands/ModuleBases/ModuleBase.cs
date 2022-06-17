using System;
using System.Threading.Tasks;
using Ultz.Extensions.Commands.Built;
using Ultz.Extensions.Commands.Context;

namespace Ultz.Extensions.Commands.ModuleBases
{
    /// <summary>
    /// Makes the inheriting class a <see cref="Module" /> that can be added to the <see cref="CommandService" />.
    /// </summary>
    /// <typeparam name="TContext"> The <see cref="CommandContext" /> this <see cref="Module" /> will use. </typeparam>
    public abstract class ModuleBase<TContext> : IModuleBase
        where TContext : CommandContext
    {
        /// <summary>
        /// The execution context.
        /// </summary>
        protected TContext Context { get; private set; }

        ValueTask IModuleBase.BeforeExecutedAsync()
        {
            return BeforeExecutedAsync();
        }

        ValueTask IModuleBase.AfterExecutedAsync()
        {
            return AfterExecutedAsync();
        }

        void IModuleBase.Prepare(CommandContext context)
        {
            Prepare(context);
        }

        /// <summary>
        /// Fires before a <see cref="Command" /> in this <see cref="Module" /> is executed.
        /// </summary>
        protected virtual ValueTask BeforeExecutedAsync()
        {
            return default;
        }

        /// <summary>
        /// Fires after a <see cref="Command" /> in this <see cref="Module" /> is executed.
        /// </summary>
        protected virtual ValueTask AfterExecutedAsync()
        {
            return default;
        }

        internal void Prepare(CommandContext context)
        {
            Context = context as TContext ??
                      throw new InvalidOperationException(
                          $"Unable to set the context. Expected {typeof(TContext)}, got {context.GetType()}.");
        }
    }
}