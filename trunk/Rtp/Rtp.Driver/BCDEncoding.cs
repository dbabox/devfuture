using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver
{
    public class BCDEncoding
    {
        #region Str2BCD
        /// <summary>
        /// Converts a string containing decimal digits to a BCD encoded number
        /// padding it with zeros on the left.
        /// The method will now verify that the string contains only digits. If a wrong
        /// parameter is passed, the result will be unpredictable
        /// </summary>
        /// <param name="s">The string containing the number to be BCD encoded</param>
        /// <returns></returns>
        public static byte[] Str2BCD(string s)
        {
            return Str2BCD(s, true, false);
        }

        /// <summary>
        /// Converts a string containing decimal digits to a BCD encoded number
        /// padding it with zeros on the left or right, based on the padLeft parameter.
        /// The method will not verify that the string contains only digits. If a wrong
        /// parameter is passed, the result will be unpredictable
        /// </summary>
        /// <param name="s">The string containing the number to be BCD encoded</param>
        /// <returns></returns>
        public static byte[] Str2BCD(string s, bool padLeft)
        {
            return Str2BCD(s, padLeft, false);
        }

        /// <summary>
        /// Converts a string containing decimal digits to a BCD encoded number
        /// padding it with zeros or 0xFs on the left or right, based on the padWithF and padLeft parameters.
        /// The method will not verify that the string contains only digits. If a wrong
        /// parameter is passed, the result will be unpredictable
        /// </summary>
        /// <param name="s">The string containing the number to be BCD encoded</param>
        /// <param name="padLeft">Shows if the result should be left or right padded (for numbers with odd number of digits)</param>
        /// <param name="padWithF">Shows if the result should be padded with 0xF (when true) or 0x0 (when false)</param>
        /// <returns></returns>
        public static byte[] Str2BCD(string s, bool padLeft, bool padWithF)
        {
            int len = s.Length;
            byte[] buf = new byte[(len + 1) / 2];
            // Set the padding with an F element as specified in the [ISO7813]

            if (padWithF && len % 2 == 1)
                if (padLeft)
                    buf[0] = 0xF0;
                else
                    buf[buf.Length - 1] = 0x0F;

            int start = (len % 2 == 1) && padLeft ? 1 : 0;
            for (int i = start; i < len + start; i++)
                buf[i / 2] = (byte)(buf[i / 2] | ((s[i - start] - '0') * (i % 2 == 1 ? 1 : 0x10)));
            return buf;
        }
        #endregion

        #region BCD2Str
        /// <summary>
        /// Converts a BCD encoded number to a string containing decimal digits or
        /// the symbol '=' encoded as the hex digit D (13).
        /// The method will now verify that the buffer contains only correct digits.
        /// If a wrong parameter is passed, the result will be unpredictable.
        /// The buffer is considered to be left padded
        /// </summary>
        /// <param name="buf">The buffer containing the BCD encoded number</param>
        /// <param name="offset"></param>
        /// <param name="len">The length of the number in decimal digits (not in bytes)</param>
        /// <returns></returns>
        public static string BCD2Str(byte[] buf, int offset, int len)
        {
            return BCD2Str(buf, offset, len, true);
        }

        /// <summary>
        /// Converts a BCD encoded number to a string containing decimal digits or
        /// the symbol '=' encoded as the hex digit D (13).
        /// The method will now verify that the buffer contains only correct digits.
        /// If a wrong parameter is passed, the result will be unpredictable
        /// </summary>
        /// <param name="buf">The buffer containing the BCD encoded number</param>
        /// <param name="offset"></param>
        /// <param name="len">The length of the number in decimal digits (not in bytes)</param>
        /// <param name="padLeft">Shows if the buffer was left or right padded (for numbers with odd number of digits)</param>
        /// <returns></returns>
        public static string BCD2Str(byte[] buf, int offset, int len, bool padLeft)
        {
            StringBuilder result = new StringBuilder(len);

            int start = ((len % 2 == 1) && padLeft) ? 1 : 0;
            for (int i = start; i < len + start; i++)
            {
                int shift = (i % 2 == 1) ? 0 : 4;
                int curChar = (buf[offset + (i / 2)] >> shift) & 0x0F;
                if (curChar == 13)
                    result.Append('=');
                else if (curChar >= 0 && curChar <= 9)
                    result.Append(curChar);
                else
                    result.Append((char)('A' + curChar - 10));
            }
            // Remove filling F's at the end
            return result.ToString().TrimEnd('F');
        }
        #endregion

    }
}
