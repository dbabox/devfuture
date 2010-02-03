/*
 * 将INT64转换成26进制字符串。
 * 支持自定义字符串映射。
 * 
 * 转换的办法有两种：
 * 1、将INT64分解成2个UINT32进行转换，请使用ModelUInt32Encode，ModeUInt32Decode来处理。
 * 2、将INT64直接处理，负数取绝对值处理。因此处理INT64时，是比较绝对值，而不能直接比较值是否相等。
 * 
 * */
using System;
using System.Collections.Generic;
using System.Text;

namespace DevFuture.Common.Security
{
    internal class Base26
    {
        internal const string BASE26 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
         
               
        readonly string baseChars ;
        readonly char[] baseCharsBytes ;
        readonly int __base__;
        readonly char zeroChar;

        internal int BaseValue
        {
            get { return __base__; }
        }

        /// <summary>
        /// 最长14个ASCII字符，推荐格式xxxx-xxxx-xxxx-xx        
        /// </summary>
        internal const int Bit64Base26MaxLength = 14;
      
        /// <summary>
        /// UInt32转Base26时最长字符串为7
        /// </summary>
        internal const int Bit32Base26MaxLength = 7;

        internal static readonly Base26 Instance = new Base26(BASE26);
      

        /// <summary>
        /// 双UINT32模式，8字节数组最大值编码得到的结果，14个字符
        /// </summary>
        internal const string UInt32ModeMaxValue = "NXMRLXVNXMRLXV";
        /// <summary>
        /// Int64直接模式下，最大值得到的编码,14个字符
        /// </summary>
        internal const string Int64ModeMaxValue = "DSQYOMTLWMKGIH";
        


        internal Base26(string baseChars_)
        {
            baseChars = baseChars_;
            baseCharsBytes = baseChars.ToCharArray();
            __base__ = baseCharsBytes.Length;
            zeroChar = baseChars[0];
        }


        internal static bool CanParseInModeUInt32(string input)
        {
            if (input.Length > UInt32ModeMaxValue.Length) return false;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] >= 'A' && input[i] <= 'Z')  
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            //字符串内容有效
            if (input.Length == UInt32ModeMaxValue.Length)
                return String.Compare(UInt32ModeMaxValue, input, StringComparison.Ordinal) >= 0;
            return true;
        }

        internal static bool CanParseInModeInt64(string input)
        {

            if (input.Length > Int64ModeMaxValue.Length) return false;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] >= 'A' && input[i] <= 'Z')
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            //字符串内容有效
            if (input.Length == Int64ModeMaxValue.Length)
                return String.Compare(Int64ModeMaxValue, input, StringComparison.Ordinal) >= 0;
            return true;
        }



        #region UInt32模式编码解码
        /// <summary>
        /// 编码无符号32位整数，总返回7个字符。若不足7位，高位补代表0的字符。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string ModelUInt32Encode(UInt32 value)
        {
            Int64 v64 = Convert.ToInt64(value);
            string rc= ModeInt64BaseEncode(v64);

            System.Diagnostics.Debug.Assert(rc.Length <= Bit32Base26MaxLength);
            if (rc.Length < Bit32Base26MaxLength)
            {
                return new string(zeroChar, Bit32Base26MaxLength - rc.Length) + rc;
            }
            return rc;
            
        }

        /// <summary>
        /// 编码2个无符号32位整数，编码为14个字符的模式
        /// </summary>
        /// <param name="valueHeight"></param>
        /// <param name="valueLow"></param>
        /// <returns></returns>
        internal string EncodeUInt32Couple(UInt32 valueHeight, UInt32 valueLow)
        {
            //Note:字符串是反着的，因为123,3是高位，而字符串中"123"，其中的字符3对应着索引2.
            return ModelUInt32Encode(valueHeight) + ModelUInt32Encode(valueLow);//高位加低位             
        }

        /// <summary>
        /// 编码以UInt64形式存储的2个无符号32位整数，编码为14个字符。
        /// </summary>
        /// <param name="valueHeight"></param>
        /// <param name="valueLow"></param>
        /// <returns></returns>
        internal string EncodeUInt32CoupleAsUInt64(UInt64 value)
        {
            return ModelUInt32Encode((UInt32)(value>>32)) + ModelUInt32Encode((UInt32)(value&0x00000000FFFFFFFFUL));//高位加低位             
        }
 

        /// <summary>
        /// 编码8字节数组
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        internal string Encode8BytesArrayAsUInt32Couple(byte[] arr)
        {
            if (arr.Length != 8) throw new ArgumentOutOfRangeException("arr", "字节数组长度必须为8.");

            byte[] arrLow = new byte[4];
            for (int i = 0; i < arrLow.Length; i++)
            {
                arrLow[i] = arr[i];
            }
            byte[] arrHeight = new byte[4];
            for (int i = 0; i < arrHeight.Length; i++)
            {
                arrHeight[i] = arr[i+4];
            }          
            UInt32 valueLow = BitConverter.ToUInt32(arrLow, 0);
            UInt32 valueHeight = BitConverter.ToUInt32(arrHeight, 0);
            return EncodeUInt32Couple(valueHeight, valueLow);

        }


        /// <summary>
        /// 解码32位无符号整数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private UInt32 ModeUInt32Decode(string value)
        {
            if (value.Length > Bit32Base26MaxLength) throw new ArgumentOutOfRangeException("value");
            long rc= ModeInt64BaseDecode(value);
            if (rc > UInt32.MaxValue) throw new ArgumentOutOfRangeException("value");
            return Convert.ToUInt32(rc);

        }

        /// <summary>
        /// 将两个字符串解码为Int64整数
        /// </summary>
        /// <param name="valueHeight"></param>
        /// <param name="valueLow"></param>
        /// <returns></returns>
        internal Int64 DecodeUInt32CoupleAsInt64(string valueHeight, string valueLow)
        {
            UInt32 vh = ModeUInt32Decode(valueHeight);//高字节
            UInt64 v = (UInt64)vh;
            v <<= 32;
            UInt32 vl = ModeUInt32Decode(valueLow);           
            v += vl;
            return (Int64)v;
        }

        /// <summary>
        /// 将组合在一起的2个UInt32的编码字符串变成一个Int64.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal Int64 DecodeUInt32CoupleOneStringAsInt64(string value)
        {
            if (value.Length != (Bit32Base26MaxLength * 2)) throw new ArgumentOutOfRangeException("value");

            return DecodeUInt32CoupleAsInt64(value.Substring(0, 7),value.Substring(7, 7));
        }
               


        #endregion
        
        #region Int64模式的编码
        /// <summary>
        /// 将Int64整数直接编码成一个字符串。编码Int64整数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal string ModeInt64BaseEncode(Int64 value)
        {
            string returnValue = "";
            if (value < 0)
            {
                value *= -1;
            }

            do
            {
                returnValue = baseCharsBytes[value % baseChars.Length] + returnValue;
                value /= __base__;
            } while (value != 0);
            return returnValue;
        }

      
        /// <summary>
        /// 将输入视为Int64转换而来的字符串。解码为Int64整数。        
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        internal Int64 ModeInt64BaseDecode(string input)
        {
            char[] arrInput = input.ToCharArray();
            Array.Reverse(arrInput);
            Int64 returnValue = 0;          
            for (int i = 0; i < arrInput.Length; i++)
            {
                int valueindex = baseChars.IndexOf(arrInput[i]);
                returnValue += Convert.ToInt64(valueindex * Math.Pow(__base__, i));//可能引发算数异常   
            }
            
            return returnValue;
        }
        #endregion

    }
}
