using System;
using System.Collections.Generic;
using System.Globalization;

namespace Ultz.Extensions.Logging
{
    /// <summary>
    /// Utility class, used to split a message with colour formatting up into "sub-messages" of one solid colour.
    /// </summary>
    public static class Output
    {
        /// <summary>
        /// The character used to denote a colour change.
        /// </summary>
        /// <remarks>
        /// The character that follows this character in a message denotes what colour the next part of the message
        /// will be. Possible values are:
        /// <list type="bullet">
        /// <item>
        /// <term>0</term><description>Black</description>
        /// </item>
        /// <item>
        /// <term>1</term><description>Dark Blue</description>
        /// </item>
        /// <item>
        /// <term>2</term><description>Dark Green</description>
        /// </item>
        /// <item>
        /// <term>3</term><description>Dark Aqua</description>
        /// </item>
        /// <item>
        /// <term>4</term><description>Dark Red</description>
        /// </item>
        /// <item>
        /// <term>5</term><description>Dark Purple</description>
        /// </item>
        /// <item>
        /// <term>6</term><description>Gold</description>
        /// </item>
        /// <item>
        /// <term>7</term><description>Grey</description>
        /// </item>
        /// <item>
        /// <term>8</term><description>Dark Grey</description>
        /// </item>
        /// <item>
        /// <term>9</term><description>Blue</description>
        /// </item>
        /// <item>
        /// <term>a</term><description>Green</description>
        /// </item>
        /// <item>
        /// <term>b</term><description>Aqua</description>
        /// </item>
        /// <item>
        /// <term>c</term><description>Red</description>
        /// </item>
        /// <item>
        /// <term>d</term><description>Light Purple</description>
        /// </item>
        /// <item>
        /// <term>e</term><description>Yellow</description>
        /// </item>
        /// <item>
        /// <term>f</term><description>White</description>
        /// </item>
        /// </list>
        /// If the colour character appears once again, the colour character is considered "escaped" and, as a result,
        /// no colour change will occur and the colour character will be written to the buffer.<br />
        /// If the character that follows the colour character is not one of the possible values denoted above and is
        /// not the colour character once again (to escape it), then the character is treated as a "reset character" and
        /// will reset the output buffer colour back to its original state. You will often see <c>§r</c> used to reset
        /// the colour, however any character that is not a valid value can be used.
        /// </remarks>
        public const char ColourCharacter = '§';

        /// <summary>
        /// Gets all coloured sub-messages in the given colour formatted message.
        /// </summary>
        /// <param name="msg">The message to split into sub-messages.</param>
        /// <param name="colourChar">The colour character to use. By default, this is <see cref="ColourCharacter" /></param>
        /// <returns>All sub-messages found within this message.</returns>
        public static IEnumerable<(string, ConsoleColor?)> EnumerateSubMessages(string msg,
            char colourChar = ColourCharacter)
        {
            var subMsg = string.Empty;
            var gotColourChar = false;
            ConsoleColor? colour = null;
            for (var i = 0; i < msg.Length; i++)
            {
                var c = msg[i];
                if (c == colourChar)
                {
                    if (gotColourChar)
                    {
                        // escaped
                        subMsg += colourChar;
                        gotColourChar = false;
                        continue;
                    }

                    gotColourChar = true;
                }
                else if (gotColourChar)
                {
                    if (subMsg != string.Empty)
                    {
                        yield return (subMsg, colour);
                        subMsg = string.Empty;
                        gotColourChar = false;
                    }

                    colour = byte.TryParse(c.ToString(), NumberStyles.HexNumber, null, out var val)
                        ? (ConsoleColor?) val
                        : null;
                    gotColourChar = false;
                }
                else
                {
                    gotColourChar = false;
                    subMsg += c;
                }
            }

            if (subMsg != string.Empty)
            {
                yield return (subMsg, colour);
            }
        }
    }
}