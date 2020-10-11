using System;
using System.IO;

namespace Ultz.Extensions.Logging
{
    /// <summary>
    /// An implementation of <see cref="IOutput"/> using a <see cref="TextWriter"/> for output.
    /// </summary>
    /// <remarks>
    /// Does not support coloured messages.
    /// </remarks>
    public class TextWriterOutput : IOutput
    {
        private readonly TextWriter _base;

        /// <summary>
        /// Constructs a new instance of <see cref="TextWriterOutput"/> using the given <see cref="TextWriter"/> as the
        /// output.
        /// </summary>
        /// <param name="base">The <see cref="TextWriter"/> to use.</param>
        public TextWriterOutput(TextWriter @base)
        {
            _base = @base;
        }

        /// <inheritdoc />
        public void Write(string msg, ConsoleColor? color)
        {
            _base.Write(msg);
        }
    }
}