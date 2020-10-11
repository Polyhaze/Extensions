// 
// CertHelpers.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using System;
using System.IO;

#endregion

namespace Ultz.Extensions.PrivacyEnhancedMail
{
    public class CertHelpers
    {
        /// <summary>
        ///     This helper function parses an integer size from the reader using the ASN.1 format
        /// </summary>
        /// <param name="rd"></param>
        /// <returns></returns>
        public static int DecodeIntegerSize(BinaryReader rd)
        {
            byte byteValue;
            int count;

            byteValue = rd.ReadByte();
            if (byteValue != 0x02) // indicates an ASN.1 integer value follows
                return 0;

            byteValue = rd.ReadByte();
            if (byteValue == 0x81)
            {
                count = rd.ReadByte(); // data size is the following byte
            }
            else if (byteValue == 0x82)
            {
                var hi = rd.ReadByte(); // data size in next 2 bytes
                var lo = rd.ReadByte();
                count = BitConverter.ToUInt16(new[] {lo, hi}, 0);
            }
            else
            {
                count = byteValue; // we already have the data size
            }

            //remove high order zeros in data
            while (rd.ReadByte() == 0x00) count -= 1;

            rd.BaseStream.Seek(-1, SeekOrigin.Current);

            return count;
        }

        /// <summary>
        /// </summary>
        /// <param name="inputBytes"></param>
        /// <param name="alignSize"></param>
        /// <returns></returns>
        public static byte[] AlignBytes(byte[] inputBytes, int alignSize)
        {
            var inputBytesSize = inputBytes.Length;

            if (alignSize != -1 && inputBytesSize < alignSize)
            {
                var buf = new byte[alignSize];
                for (var i = 0; i < inputBytesSize; ++i) buf[i + (alignSize - inputBytesSize)] = inputBytes[i];

                return buf;
            }

            return inputBytes; // Already aligned, or doesn't need alignment
        }
    }
}
