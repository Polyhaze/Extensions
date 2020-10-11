using System.Threading.Tasks;
using Ultz.Extensions.Commands.Built;
using Ultz.Extensions.Commands.Context;
using Ultz.Extensions.Commands.Results.User;

namespace Ultz.Extensions.Commands.Parsing.TypeParsers
{
    internal interface ITypeParser
    {
        ValueTask<TypeParserResult<object>> ParseAsync(Parameter parameter, string value, CommandContext context);
    }
}