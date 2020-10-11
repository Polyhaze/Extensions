using System;
using Ultz.Extensions.Commands.Built;

namespace Ultz.Extensions.Commands.Parsing.TypeParsers.Primitive
{
    internal class PrimitiveTypeParser<T> : IPrimitiveTypeParser
        where T : struct
    {
        private readonly TryParseDelegate<T> _tryParse;

        public PrimitiveTypeParser()
        {
            _tryParse = (TryParseDelegate<T>) Utilities.TryParseDelegates[typeof(T)];
        }

        bool IPrimitiveTypeParser.TryParse(Parameter parameter, string value, out object result)
        {
            if (!TryParse(value, out var genericResult))
            {
                result = null;
                return false;
            }

            result = genericResult;
            return true;
        }

        public bool TryParse(string value, out T result)
        {
            return _tryParse(value.AsSpan(), out result);
        }
    }
}