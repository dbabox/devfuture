/*
 * ��INT64ת����26�����ַ�����
 * ֧���Զ����ַ���ӳ�䡣
 * 
 * ת���İ취�����֣�
 * 1����INT64�ֽ��2��UINT32����ת������ʹ��ModelUInt32Encode��ModeUInt32Decode������
 * 2����INT64ֱ�Ӵ�������ȡ����ֵ������˴���INT64ʱ���ǱȽϾ���ֵ��������ֱ�ӱȽ�ֵ�Ƿ���ȡ�
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
        /// �14��ASCII�ַ����Ƽ���ʽxxxx-xxxx-xxxx-xx        
        /// </summary>
        internal const int Bit64Base26MaxLength = 14;
      
        /// <summary>
        /// UInt32תBase26ʱ��ַ���Ϊ7
        /// </summary>
        internal const int Bit32Base26MaxLength = 7;

        internal static readonly Base26 Instance = new Base26(BASE26);
      

        /// <summary>
        /// ˫UINT32ģʽ��8�ֽ��������ֵ����õ��Ľ����14���ַ�
        /// </summary>
        internal const string UInt32ModeMaxValue = "NXMRLXVNXMRLXV";
        /// <summary>
        /// Int64ֱ��ģʽ�£����ֵ�õ��ı���,14���ַ�
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
            //�ַ���������Ч
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
            //�ַ���������Ч
            if (input.Length == Int64ModeMaxValue.Length)
                return String.Compare(Int64ModeMaxValue, input, StringComparison.Ordinal) >= 0;
            return true;
        }



        #region UInt32ģʽ�������
        /// <summary>
        /// �����޷���32λ�������ܷ���7���ַ���������7λ����λ������0���ַ���
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
        /// ����2���޷���32λ����������Ϊ14���ַ���ģʽ
        /// </summary>
        /// <param name="valueHeight"></param>
        /// <param name="valueLow"></param>
        /// <returns></returns>
        internal string EncodeUInt32Couple(UInt32 valueHeight, UInt32 valueLow)
        {
            //Note:�ַ����Ƿ��ŵģ���Ϊ123,3�Ǹ�λ�����ַ�����"123"�����е��ַ�3��Ӧ������2.
            return ModelUInt32Encode(valueHeight) + ModelUInt32Encode(valueLow);//��λ�ӵ�λ             
        }

        /// <summary>
        /// ������UInt64��ʽ�洢��2���޷���32λ����������Ϊ14���ַ���
        /// </summary>
        /// <param name="valueHeight"></param>
        /// <param name="valueLow"></param>
        /// <returns></returns>
        internal string EncodeUInt32CoupleAsUInt64(UInt64 value)
        {
            return ModelUInt32Encode((UInt32)(value>>32)) + ModelUInt32Encode((UInt32)(value&0x00000000FFFFFFFFUL));//��λ�ӵ�λ             
        }
 

        /// <summary>
        /// ����8�ֽ�����
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        internal string Encode8BytesArrayAsUInt32Couple(byte[] arr)
        {
            if (arr.Length != 8) throw new ArgumentOutOfRangeException("arr", "�ֽ����鳤�ȱ���Ϊ8.");

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
        /// ����32λ�޷�������
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
        /// �������ַ�������ΪInt64����
        /// </summary>
        /// <param name="valueHeight"></param>
        /// <param name="valueLow"></param>
        /// <returns></returns>
        internal Int64 DecodeUInt32CoupleAsInt64(string valueHeight, string valueLow)
        {
            UInt32 vh = ModeUInt32Decode(valueHeight);//���ֽ�
            UInt64 v = (UInt64)vh;
            v <<= 32;
            UInt32 vl = ModeUInt32Decode(valueLow);           
            v += vl;
            return (Int64)v;
        }

        /// <summary>
        /// �������һ���2��UInt32�ı����ַ������һ��Int64.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal Int64 DecodeUInt32CoupleOneStringAsInt64(string value)
        {
            if (value.Length != (Bit32Base26MaxLength * 2)) throw new ArgumentOutOfRangeException("value");

            return DecodeUInt32CoupleAsInt64(value.Substring(0, 7),value.Substring(7, 7));
        }
               


        #endregion
        
        #region Int64ģʽ�ı���
        /// <summary>
        /// ��Int64����ֱ�ӱ����һ���ַ���������Int64����
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
        /// ��������ΪInt64ת���������ַ���������ΪInt64������        
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
                returnValue += Convert.ToInt64(valueindex * Math.Pow(__base__, i));//�������������쳣   
            }
            
            return returnValue;
        }
        #endregion

    }
}
