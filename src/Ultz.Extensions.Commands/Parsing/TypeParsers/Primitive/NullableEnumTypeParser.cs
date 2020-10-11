using System.Linq;
using Ultz.Extensions.Commands.Built;

namespace Ultz.Extensions.Commands.Parsing.TypeParsers.Primitive
{
    // T is the underlying type of the enum, not typeof(enum)
    internal sealed class NullableEnumTypeParser<T> : IPrimitiveTypeParser
        where T : struct
    {
        private readonly EnumTypeParser<T> _enumTypeParser;

        public NullableEnumTypeParser(EnumTypeParser<T> enumTypeParser)
        {
            _enumTypeParser = enumTypeParser;
        }

        public bool TryParse(Parameter parameter, string value, out object result)
        {
            if (parameter.Service.NullableNouns.Any(x => value.Equals(x, parameter.Service.StringComparison)))
            {
                result = null;
                return true;
            }

            if (!_enumTypeParser.TryParse(parameter, value, out var enumResult))
            {
                result = null;
                return false;
            }

            result = enumResult;
            return true;
        }
    }
}