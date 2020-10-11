using System;

namespace Ultz.Extensions.Logging
{
    /// <summary>
    /// Represents an output for log messages.
    /// </summary>
    public interface IOutput
    {
        /// <summary>
        /// Writes a message to the output buffer, optionally with a foreground colour.
        /// </summary>
        /// <param name="msg">The message to write.</param>
        /// <param name="color">The text colour of the message.</param>
        void Write(string msg, ConsoleColor? color);
    }
}