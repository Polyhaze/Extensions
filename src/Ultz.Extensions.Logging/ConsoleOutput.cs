using System;

namespace Ultz.Extensions.Logging
{
    /// <summary>
    /// An implementation of <see cref="IOutput"/> using <see cref="Console"/> for output.
    /// </summary>
    public class ConsoleOutput : IOutput
    {
        /// <summary>
        /// Gets a cached instance of <see cref="ConsoleOutput"/>
        /// </summary>
        public static ConsoleOutput Instance { get; } = new ConsoleOutput();

        /// <inheritdoc />
        public void Write(string msg, ConsoleColor? color)
        {
            if (!(color is null))
            {
                Console.ForegroundColor = color.Value;
            }
            else
            {
                Console.ResetColor();
            }

            Console.Write(msg);
        }
    }
}