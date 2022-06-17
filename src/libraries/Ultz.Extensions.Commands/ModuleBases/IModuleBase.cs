using System.Threading.Tasks;
using Ultz.Extensions.Commands.Context;

namespace Ultz.Extensions.Commands.ModuleBases
{
    internal interface IModuleBase
    {
        ValueTask BeforeExecutedAsync();

        ValueTask AfterExecutedAsync();

        void Prepare(CommandContext context);
    }
}