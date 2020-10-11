#region

using System;
using System.Diagnostics;

#endregion

namespace Ultz.Extensions.PrivacyEnhancedMail
{
    public class RsaParameterTraits
    {
        public readonly int SizeD = -1;
        public readonly int SizeDp = -1;
        public readonly int SizeDq = -1;
        public readonly int SizeExp = -1;
        public readonly int SizeInvQ = -1;

        public readonly int SizeMod = -1;
        public readonly int SizeP = -1;
        public readonly int SizeQ = -1;

        public RsaParameterTraits(int modulusLengthInBits)
        {
            // The modulus length is supposed to be one of the common lengths, which is the commonly referred to strength of the key,
            // like 1024 bit, 2048 bit, etc.  It might be a few bits off though, since if the modulus has leading zeros it could show
            // up as 1016 bits or something like that.
            int assumedLength;
            var logbase = Math.Log(modulusLengthInBits, 2);
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (logbase == (int) logbase)
            {
                // It's already an even power of 2
                assumedLength = modulusLengthInBits;
            }
            else
            {
                // It's not an even power of 2, so round it up to the nearest power of 2.
                assumedLength = (int) (logbase + 1.0);
                assumedLength = (int) Math.Pow(2, assumedLength);
                Debug.Assert(false); // Can this really happen in the field?  I've never seen it, so if it happens
                // you should verify that this really does the 'right' thing!
            }

            switch (assumedLength)
            {
                case 1024:
                    SizeMod = 0x80;
                    SizeExp = -1;
                    SizeD = 0x80;
                    SizeP = 0x40;
                    SizeQ = 0x40;
                    SizeDp = 0x40;
                    SizeDq = 0x40;
                    SizeInvQ = 0x40;
                    break;
                case 2048:
                    SizeMod = 0x100;
                    SizeExp = -1;
                    SizeD = 0x100;
                    SizeP = 0x80;
                    SizeQ = 0x80;
                    SizeDp = 0x80;
                    SizeDq = 0x80;
                    SizeInvQ = 0x80;
                    break;
                case 4096:
                    SizeMod = 0x200;
                    SizeExp = -1;
                    SizeD = 0x200;
                    SizeP = 0x100;
                    SizeQ = 0x100;
                    SizeDp = 0x100;
                    SizeDq = 0x100;
                    SizeInvQ = 0x100;
                    break;
                default:
                    Debug.Assert(false); // Unknown key size?
                    break;
            }
        }
    }
}