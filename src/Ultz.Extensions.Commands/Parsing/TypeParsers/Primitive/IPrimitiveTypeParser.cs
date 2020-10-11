using Ultz.Extensions.Commands.Built;

namespace Ultz.Extensions.Commands.Parsing.TypeParsers.Primitive
{
    internal interface IPrimitiveTypeParser
    {
        bool TryParse(Parameter parameter, string value, out object result);
    }
}