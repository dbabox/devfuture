using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
 

namespace Rtp.Driver
{
    public static class Utility
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
       /// <summary>
       /// ��������ģʽ�����ַ���ת����Byte����.
        /// �ɹ�����ת���ɹ���byte����ĳ��ȣ�ʧ�ܷ���0��
       /// </summary>
       /// <param name="str"></param>
       /// <param name="resultBuff"></param>
       /// <returns></returns>
        public static int HexStrToByteArray(string str, ref byte[] resultBuff)
        {
            return HexStrToByteArray(str, ref resultBuff, true);
        }

        /// <summary>
        /// ��16�����ַ���ת����byte���顣�ɹ�����ת���ɹ���byte����ĳ��ȣ�ʧ�ܷ���-1��
        /// ע�⣺�ֽ������ַ��� 00 01����ʾ����ʮ����1����ת���ֽ��������Ϊbyte[0]=0x01,byte[1]=0x00.
        /// </summary>
        /// <param name="str">��ת�����ַ�������"001A0BC0"</param>
        /// <param name="resultBuff">������������ڴ��ת����Ľ��</param>
        /// <param name="checkInput">true��ʾ������룬false��ʾ����顣�����ʱ���ܺá�</param>
        /// <returns>����ת���ɹ���byte����ĳ���.</returns>
        public static int HexStrToByteArray(string str, ref byte[] resultBuff, bool checkInput)
        {
            Array.Clear(resultBuff, 0, resultBuff.Length);
            string trim = System.Text.RegularExpressions.Regex.Replace(str, @"\s", "");
            if (checkInput)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(trim, "^[A-Fa-f0-9]+$"))
                {
                    logger.WarnFormat("��ת���ַ��� {0} ���ֲ���16�����ַ�����", trim);
                    return 0;
                }
                if (trim.Length % 2 != 0)
                {
                    logger.WarnFormat("��ת���ַ��� {0} ���ֵĸ�����Ϊż�����޷�ת��16���ƣ�", trim);
                    return 0;
                }
            }
            int rc = trim.Length / 2;
            if (resultBuff.Length < rc) throw new ArgumentException("���buffer����̫С��", "resultBuff");
            for (int i = 0; i < rc; i++)
                resultBuff[i] = Convert.ToByte(trim.Substring(i * 2, 2), 16);
            return rc;
        }

        /// <summary>
        /// Byte����ת16�����ַ���
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ByteArrayToHexStr(byte[] bytes)
        {
            return ByteArrayToHexStr(bytes, bytes.Length);
        }
        

        /// <summary>
        /// Byte����ת16�����ַ���
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ByteArrayToHexStr(byte[] bytes,int len)
        {
            StringBuilder sb = new StringBuilder(len*2);
            if (bytes != null)
            {
                for (int i = 0; i < len; i++)
                {
                    sb.AppendFormat("{0,2:X2}", bytes[i]);
                }
            }
            return sb.ToString();
        }

        public static string ByteArrayToHexStr(byte[] bytes, string spchar)
        {
            return ByteArrayToHexStr(bytes, bytes.Length, spchar);
        }

        /// <summary>
        /// ��ʽ���ֽ����飬��spcharΪ�ָ����
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="len"></param>
        /// <param name="spchar"></param>
        /// <returns></returns>
        public static string ByteArrayToHexStr(byte[] bytes, int len, string spchar)
        {
            return ByteArrayToHexStr(bytes, 0, len, spchar);
        }

        public static string ByteArrayToHexStr(byte[] bytes,int offset, int len, string spchar)
        {
            if ((offset + len) > bytes.Length) throw new ArgumentException("offset+len�������鳤�ȣ�");
            StringBuilder sb = new StringBuilder(len * 2);
            if (bytes != null)
            {
                for (int i = offset; i < (len + offset); i++)
                {
                    sb.AppendFormat("{0,2:X2}", bytes[i]);
                    if (i < (len + offset) - 1) sb.Append(spchar);
                }
            }
            return sb.ToString();
        }


        public static IList<string>  GetCmdFromFile(string file)
        {
            IList<string> strList = new List<string>();
            using (System.IO.StreamReader sr = new System.IO.StreamReader(file,Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {             
                    if(line.StartsWith("--")|| line.StartsWith("//") || line.StartsWith("#") || line.StartsWith("=="))
                    {
                        logger.InfoFormat(line);
                        continue;
                    }
                    line = line.Trim();
                    if (line.Length == 0) continue;
                    if (line.Length > 2)
                    {
                        strList.Add(line);
                    }
                    else
                    {
                        logger.WarnFormat("��Ч�������ַ�����{0}", line);
                    }

                }
            }
            return strList;
        }


        #region 3DES���

        /// <summary>
        /// 3DES����1��16�ֽڿ� Ҫ��KEY>=16�ֽڣ���Ϊ8�ı���������ת���ɹ��Ľ���ֽ�����
        /// ����16�ֽ���Կ��8�ֽ�����������ⲿ��֤����ʱ����ʹ��ECBģʽ���ú���ͨ����֤��
        /// ����inputValue������8�ı�������.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="inputValue"></param>
        /// <param name="rbuff"></param>
        /// <param name="CipherMode">ECB����CBC����CPU�����׹��������ⲿ��֤ʱ��ʹ�õ���ECBģʽ��</param>
        /// <returns></returns>
        public static int TripDesBlockEncrypt(byte[] key, byte[] inputValue,CipherMode cm,ref byte[] rbuff)
        {
            System.Diagnostics.Trace.Assert(key.Length >= 16 && key.Length % 8 == 0 &&
                inputValue.Length > 0 && rbuff.Length >= inputValue.Length
                );

            Array.Clear(rbuff, 0, rbuff.Length);
#if USE_MS_CRYPTO
            TripleDESCryptoServiceProvider tripdes = new TripleDESCryptoServiceProvider();
            tripdes.Mode = cm;
#else
            SharpPrivacy.Cipher.TripleDES tripdes = SharpPrivacy.Cipher.TripleDES.Create();
            tripdes.Mode = (SharpPrivacy.Cipher.CipherMode)cm;
#endif
            
            tripdes.BlockSize = 64;          //��8λΪ�����
            byte[] iv = new byte[tripdes.BlockSize/8]; //8�ֽ�������ȫ0
            Array.Clear(iv, 0, iv.Length);
            tripdes.KeySize = 128;          //16�ֽ���Կ
            tripdes.Key = key;
#if USE_MS_CRYPTO
            ICryptoTransform  trans= tripdes.CreateEncryptor();
            int rc= trans.TransformBlock(inputValue, 0, inputValue.Length, rbuff, 0);
#else
            SharpPrivacy.Cipher.ICryptoTransform trans = tripdes.CreateEncryptor();
            int rc = trans.TransformBlock(inputValue, 0, inputValue.Length, ref rbuff, 0);
#endif
            
            logger.InfoFormat("TripDes:key={0},inputValue={1},CipherMode={4}, outputValue={2},rc={3}",
                ByteArrayToHexStr(key,key.Length), ByteArrayToHexStr(inputValue, inputValue.Length), ByteArrayToHexStr(rbuff, inputValue.Length), rc, cm);
           
            return rc;
        }




        /// <summary>
        /// 3DES����
        /// </summary>
        public static byte[] TripleDESEncrypt(byte[] bySource, CipherMode mCipherMode, PaddingMode mPaddingMode, byte[] mbyKey, byte[] mbyIV)
        {
          
            TripleDESCryptoServiceProvider MyServiceProvider = new TripleDESCryptoServiceProvider();

            //����des���������õ��㷨

            MyServiceProvider.Mode = mCipherMode;

            //�����������
            MyServiceProvider.Padding = mPaddingMode;

            //TripleDESCryptoServiceProvider
            //֧�ִ� 128 λ�� 192 λ���� 64 λ����������Կ����
            //IV��Ҫ8���ֽ�
            //����KEYʱҪע����ǿ�������CryptographicException�쳣����Ҫ����Ϊ�����õ�KEYΪWeakKey
            ICryptoTransform MyTransform = MyServiceProvider.CreateEncryptor(mbyKey, mbyIV);
            //CryptoStream����������ǽ����������ӵ�����ת������
            MemoryStream ms = new MemoryStream();
            CryptoStream MyCryptoStream = new CryptoStream(ms, MyTransform, CryptoStreamMode.Write);
            //���ֽ������е�����д�뵽��������
            MyCryptoStream.Write(bySource, 0, bySource.Length);
            MyCryptoStream.FlushFinalBlock();
            MyCryptoStream.Close();
            byte[] byEncRet = ms.ToArray();
            ms.Close();
            return byEncRet;
            
        }

        /// <summary>
        /// 3DES����
        /// </summary>
        public static int TripleDESDecrypt(byte[] data,byte[] key16,CipherMode mCipherMode, PaddingMode mPaddingMode,ref byte[] rbuff)
        {
            byte[] mbyIV = new byte[8];//��ʼ���� ȫ0

            using (TripleDESCryptoServiceProvider MyServiceProvider = new TripleDESCryptoServiceProvider())
            {

                //����des���������õ��㷨

                MyServiceProvider.Mode = mCipherMode;

                //�����������
                MyServiceProvider.Padding = mPaddingMode;

                //TripleDESCryptoServiceProvider
                //֧�ִ� 128 λ�� 192 λ���� 64 λ����������Կ����
                //IV��Ҫ8���ֽ�
                //����KEYʱҪע����ǿ�������CryptographicException�쳣����Ҫ����Ϊ�����õ�KEYΪWeakKey
                ICryptoTransform MyTransform = MyServiceProvider.CreateDecryptor(key16, mbyIV);
                //CryptoStream����������ǽ����������ӵ�����ת������
                MemoryStream ms = new MemoryStream();
                CryptoStream MyCryptoStream = new CryptoStream(ms, MyTransform, CryptoStreamMode.Write);
                //���ֽ������е�����д�뵽��������
                MyCryptoStream.Write(data, 0, data.Length);
                MyCryptoStream.FlushFinalBlock();
                MyCryptoStream.Close();

                byte[] byEncRet = ms.ToArray();
                ms.Close();
                Array.Copy(byEncRet, rbuff, byEncRet.Length);
                return byEncRet.Length;
            }
           
        }

        /// <summary>
        /// ʹ��MACTripleDES������Ϣ��MAC
        /// </summary>
        /// <param name="key">���32�ֽڵ�KEY</param>
        /// <param name="iv">���8�ֽڵĳ�ʼ����</param>
        /// <param name="inputValue">������MAC�����ݣ�����Ϊ2�ı�����</param>
        /// <param name="rbuff">MAC���������Ϊ8�ֽ�</param>
        /// <returns></returns>
        //public static int TripDesMac(byte[] key, byte[] iv, byte[] inputValue, ref byte[] rbuff)
        //{
        //    MACTripleDES mac = new MACTripleDES();
        //    mac.Key = key;



        //}
        #endregion

        #region DES�㷨���

        /// <summary>
        /// DES����һ��8�ֽڿ顣key����Ϊ8�ֽڡ�����ת���ɹ��Ľ���ֽ�����
        /// </summary>
        /// <param name="key">8�ֽ���Կ</param>
        /// <param name="inputValue">����������8�ֽڡ�Ҳ�ǳ�ʼ����ֵ��</param>
        /// <param name="cm">����ģʽ��һ��Ӧ����ECB.</param>
        /// <param name="rbuff">���ܺ�Ľ��</param>
        /// <returns></returns>
        public static int DesBlockEncrypt(byte[] key,byte[] inputValue,CipherMode cm, ref byte[] rbuff)
        {
            System.Diagnostics.Trace.Assert(key.Length ==8 &&  
                inputValue.Length > 0 && rbuff.Length >= 8 
                );
#if USE_MS_CRYPTO
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Mode = cm; //CipherMode.ECB;
#else
            SharpPrivacy.Cipher.DES des = SharpPrivacy.Cipher.DES.Create();
            des.Clear();           
            des.Mode = (SharpPrivacy.Cipher.CipherMode)cm;  
#endif
            
            des.BlockSize = 64;
        
            des.IV = inputValue;
            des.KeySize = 64;          //8�ֽ���Կ
            des.Key = key;
#if USE_MS_CRYPTO
            ICryptoTransform trans = des.CreateEncryptor();
            int rc = trans.TransformBlock(inputValue, 0, inputValue.Length, rbuff, 0);
#else   

            SharpPrivacy.Cipher.ICryptoTransform trans = des.CreateEncryptor();
            int rc = trans.TransformBlock(inputValue, 0, 8, ref rbuff, 0);
         
#endif
            logger.InfoFormat("DesBlockEncrypt:key={0},inputValue={1},CipherMode={4}, outputValue={2},rc={3}",
                ByteArrayToHexStr(key), ByteArrayToHexStr(inputValue, inputValue.Length), ByteArrayToHexStr(rbuff, inputValue.Length), rc, cm);
            return 0;  
        }

        /// <summary>
        /// DES����
        /// </summary>
        /// <param name="bySource"></param>
        /// <param name="mCipherMode"></param>
        /// <param name="mPaddingMode"></param>
        /// <param name="mbyKey"></param>
        /// <param name="mbyIV"></param>
        /// <returns></returns>
        public static int DesBlockDecrypt(byte[] bySource, CipherMode cm, PaddingMode mPaddingMode, byte[] mbyKey,ref byte[] rbuff)
        {
            System.Diagnostics.Trace.Assert(mbyKey.Length == 8);
            byte[] buff=new byte[bySource.Length];
            SharpPrivacy.Cipher.DES des = SharpPrivacy.Cipher.DES.Create();
            des.Mode = (SharpPrivacy.Cipher.CipherMode)cm;
            des.Padding = (SharpPrivacy.Cipher.PaddingMode)mPaddingMode;
            des.KeySize = 64;
            des.Key = mbyKey;
            SharpPrivacy.Cipher.ICryptoTransform c = des.CreateDecryptor(mbyKey, bySource);
            return c.TransformBlock(bySource, 0, 8, ref rbuff, 0);         
        }


     
        /// <summary>
        /// DES�������ⳤ������
        /// </summary>
        /// <param name="bySource"></param>
        /// <param name="mCipherMode"></param>
        /// <param name="mPaddingMode"></param>
        /// <param name="mbyKey"></param>
        /// <param name="mbyIV"></param>
        /// <returns></returns>
        public static byte[] DESEncrypt(byte[] bySource, CipherMode mCipherMode, PaddingMode mPaddingMode, byte[] mbyKey, byte[] mbyIV)
        {
            try
            {
                DESCryptoServiceProvider MyServiceProvider = new DESCryptoServiceProvider();

                //����des���������õ��㷨

                MyServiceProvider.Mode = mCipherMode;

                //�����������
                MyServiceProvider.Padding = mPaddingMode;

                ICryptoTransform MyTransform = MyServiceProvider.CreateEncryptor(mbyKey, mbyIV);
                //CryptoStream����������ǽ����������ӵ�����ת������
                MemoryStream ms = new MemoryStream();
                CryptoStream MyCryptoStream = new CryptoStream(ms, MyTransform, CryptoStreamMode.Write);
                //���ֽ������е�����д�뵽��������
                MyCryptoStream.Write(bySource, 0, bySource.Length);
                MyCryptoStream.FlushFinalBlock();
                MyCryptoStream.Close();
                byte[] byEncRet = ms.ToArray();
                ms.Close();
                return byEncRet;
            }
            catch (Exception ex)
            {
                
                logger.ErrorFormat("Exception caught: {0}", ex.Message);
                return new byte[8];
            }

        }

     
        /// <summary>
        /// DES ��������
        /// </summary>
        /// <param name="bySource">Ҫ���ܵ�����</param>
        /// <param name="mCipherMode">DES���������õ��㷨ģʽ��Ĭ��CBC</param>
        /// <param name="mPaddingMode">ָ������Ϣ���ݿ�ȼ��ܲ��������ȫ���ֽ�����ʱӦ�õ�������ͣ�Ĭ�ϲ����None.</param>
        /// <param name="mbyKey">8�ֽ���Կ</param>
        /// <param name="mbyIV">8�ֽڳ�ʼ��������Ĭ��8��0</param>
        /// <returns></returns>
        public static byte[] DESDecrypt(byte[] bySource, CipherMode mCipherMode, PaddingMode mPaddingMode, byte[] mbyKey, byte[] mbyIV)
        {
            try
            {
                DESCryptoServiceProvider MyServiceProvider = new DESCryptoServiceProvider();
                //����des���������õ��㷨
                MyServiceProvider.Mode = mCipherMode;
                //�����������
                MyServiceProvider.Padding = mPaddingMode;
                ICryptoTransform MyTransform = MyServiceProvider.CreateDecryptor(mbyKey, mbyIV);
                //CryptoStream����������ǽ����������ӵ�����ת������
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                CryptoStream MyCryptoStream = new CryptoStream(ms, MyTransform, CryptoStreamMode.Write);
                //���ֽ������е�����д�뵽��������
                MyCryptoStream.Write(bySource, 0, bySource.Length);
                MyCryptoStream.FlushFinalBlock();
                MyCryptoStream.Close();
                byte[] byEncRet = ms.ToArray();
                ms.Close();
                return byEncRet;
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("DESDecrypt Error:{0}", ex);
                return new byte[8];
            }
        }

        public static byte[] CalcDesMac(byte[] key, byte[] data)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Key = key;
            des.IV = new byte[8];
            des.Padding = PaddingMode.Zeros;
            MemoryStream ms = new MemoryStream();
            using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(data, 0, data.Length);
            }
            byte[] encryption = ms.ToArray();
            byte[] mac = new byte[8];
            Array.Copy(encryption, encryption.Length - 8, mac, 0, 8);
            logger.InfoFormat("encryption={0}", Utility.ByteArrayToHexStr(encryption));
            return mac;
        }



        /// <summary>
        /// MAC������Ҫ���õ�CBC DES�㷨ʵ��
        /// </summary>
        public static byte[] HCDES(byte[] Key, byte[] Data)
        {
            try
            {
                //����һ��DES�㷨�ļ�����
                DESCryptoServiceProvider MyServiceProvider = new DESCryptoServiceProvider();
                MyServiceProvider.Mode = CipherMode.CBC;
                MyServiceProvider.Padding = PaddingMode.None;
                //��DES�㷨�ļ���������CreateEncryptor����,����һ������ת���ӿڶ���
                //��һ�������ĺ����ǣ��Գ��㷨�Ļ�����Կ(����Ϊ64λ,Ҳ����8���ֽ�)
                // �����˹�����,Ҳ����������ɷ����ǣ�MyServiceProvider.GenerateKey();
                //�ڶ��������ĺ����ǣ��Գ��㷨�ĳ�ʼ������(����Ϊ64λ,Ҳ����8���ֽ�)
                // �����˹�����,Ҳ����������ɷ����ǣ�MyServiceProvider.GenerateIV()
                //
                //Ĭ�ϳ�ʼ������8��0
                ICryptoTransform MyTransform = MyServiceProvider.CreateEncryptor(Key, new byte[8]);

                //CryptoStream����������ǽ����������ӵ�����ת������
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                CryptoStream MyCryptoStream = new CryptoStream(ms, MyTransform, CryptoStreamMode.Write);
                //���ֽ������е�����д�뵽��������


                MyCryptoStream.Write(Data, 0, Data.Length);
                //�رռ���������
                byte[] bEncRet = new byte[8];
                // Array.Copy(ms.GetBuffer(), bEncRet, ms.Length);
                bEncRet = ms.ToArray(); // MyCryptoStream�ر�֮ǰms.Length Ϊ8�� �ر�֮��Ϊ16

                MyCryptoStream.FlushFinalBlock();
                MyCryptoStream.Close();
                byte[] bTmp = ms.ToArray();
                ms.Close();


                // return bEncRet;
                return bTmp;//
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("HCDES Exception Caught, Exception = {0}", ex.Message);
                return new byte[8];
            }

        }

       
        /// <summary>
        /// ����MacData�����ݵ�MACֵ��
        /// </summary>
        /// <param name="MacData">Ҫ����MAC�����ݡ�</param>
        /// <param name="mbyKey">��Կֵ</param>
        /// <param name="mbyIV">��ʼ��������Ĭ��Ӧ��8��0��</param>
        /// <returns></returns>
        public static byte[] MAC_CBC(byte[] MacData, byte[] mbyKey, byte[] mbyIV)
        {
            try
            {
                int iGroup = 0;
                byte[] bKey = mbyKey;

                byte[] bIV = mbyIV;

                byte[] bTmpBuf1 = new byte[8];
                byte[] bTmpBuf2 = new byte[8];

                // init

                Array.Copy(bIV, bTmpBuf1, 8);

                if ((MacData.Length % 8 == 0))
                    iGroup = MacData.Length / 8;
                else
                    iGroup = MacData.Length / 8 + 1;

                int i = 0;
                int j = 0;

                for (i = 0; i < iGroup; i++)
                {
                    Array.Copy(MacData, 8 * i, bTmpBuf2, 0, 8);
                    for (j = 0; j < 8; j++)
                        bTmpBuf1[j] = (byte)(bTmpBuf1[j] ^ bTmpBuf2[j]);
                    bTmpBuf2 = HCDES(bKey, bTmpBuf1);
                    Array.Copy(bTmpBuf2, bTmpBuf1, 8);
                }

                return bTmpBuf2;
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("MAC_CBC() Exception caught, exception = {0}", ex.Message);
                return new byte[8];
            }

        }
        #endregion

        /// <summary>
        /// Ϊĳ���������MAC����������+MAC����Щ���
        /// ʹ��DES�㷨,KeyΪ8�ֽڼ���MAC.
        /// 
        /// </summary>
        /// <param name="rn4">4�ֽ������</param>
        /// <param name="cipp4">CLA,INC,P1,P2</param>
        /// <param name="lc">Lc</param>
        /// <param name="data">������</param>
        /// <returns></returns>
        public static int DesKey8MAC(byte[] rn4,byte[] cipp4,byte lc,byte[] data,byte[] key8 ,ref byte[] mac)
        {            

            System.Diagnostics.Trace.Assert(rn4.Length == 4);
            System.Diagnostics.Trace.Assert(cipp4.Length == 4);
            System.Diagnostics.Trace.Assert(key8.Length == 8);
            System.Diagnostics.Trace.Assert(mac.Length == 8);

            int block_len = 5 + lc;
            int zeroPad = 0;
            if (block_len % 8 != 0)
            {
                zeroPad = 8 - (block_len % 8);
                block_len += zeroPad;
            }
            else
            {
                block_len += 8;
            }
            byte[] buff = new byte[block_len];
            Array.Clear(buff, 0, block_len);
            if (zeroPad == 0) 
                buff[block_len - 8] = 0x80;
            else 
                buff[block_len - zeroPad] = 0x80;

            Array.Copy(cipp4, 0, buff, 0, 4);
            buff[0] &= 0xF4;
            buff[4] = (byte)(lc + 4);
            Array.Copy(data, 0, buff, 5, lc);
            //======����׼������Ҫ����MAC������ buff========
            byte[] init = new byte[8];
            Array.Clear(init, 0, 8);
            Array.Copy(rn4, init, 4);            
            int rc = 0;
            for (int i = 0; i < (block_len / 8); ++i)
            {
                init[0] ^= buff[0+i*8];
                init[1] ^= buff[1 + i * 8];
                init[2] ^= buff[2 + i * 8];
                init[3] ^= buff[3 + i * 8];
                init[4] ^= buff[4 + i * 8];
                init[5] ^= buff[5 + i * 8];
                init[6] ^= buff[6 + i * 8];
                init[7] ^= buff[7 + i * 8];

                rc = DesBlockEncrypt(key8, init, CipherMode.ECB, ref mac);
                logger.InfoFormat("i={0},mac={1}", i, Utility.ByteArrayToHexStr(mac,init.Length));                
                Array.Copy(mac, init, 8);
              
            }
            //init��������ֵ,mac��֮���       
            return 0;
        }

     
        /// <summary>
        /// ��8�ֽ���Կkey8��������data��mac��ע�⣺MAC����������8�ֽ�,ʵ��ֻȡ���ս�������4�ֽ�ʹ�á�       
        /// </summary>
        /// <param name="data">������MAC�����ݡ�</param>
        /// <param name="key8">8�ֽ���Կ����������MAC��</param>
        /// <param name="init">8�ֽ��������</param>
        /// <param name="mac">����ó���MAC���ݣ�����Ϊ8�ֽڡ�PBOC�涨���յõ��ǴӼ��������ȡ�õ�4�ֽڳ��ȵ�MAC��</param>
        /// <returns>�ɹ�����0�����󷵻ط�0�����롣</returns>
        public static int PBOC_GetKey8MAC(byte[] data, byte[] key8,byte[] init, ref byte[] mac)
        {
            System.Diagnostics.Trace.Assert(key8.Length == 8, "KEY���ȴ���Ӧ����8�ֽ�");
            System.Diagnostics.Trace.Assert(init.Length == 8, "IV���ȴ���Ӧ����8�ֽ�");

            logger.InfoFormat("data={0} key8={1} IV={2}", 
                Utility.ByteArrayToHexStr(data), 
                Utility.ByteArrayToHexStr(key8), 
                Utility.ByteArrayToHexStr(init));
            int block_len = data.Length;
            int zeroPad = 0;

            //1������8�ı�����ĩβ��0x80,0x00....ֱ����8�ı���
            if (block_len % 8 != 0)
            {
                zeroPad = 8 - (block_len % 8);
                block_len += zeroPad;
            }
            else
            {
                block_len += 8;
            }
            byte[] buff = new byte[block_len];
            Array.Clear(buff, 0, block_len);
            Array.Copy(data, buff, data.Length);

            if (zeroPad == 0)
                buff[block_len - 8] = 0x80;
            else
                buff[block_len - zeroPad] = 0x80;

            //======����׼������Ҫ����MAC������ buff========
            //byte[] init = new byte[8];
            //Array.Clear(init, 0, 8);
          
            int rc = 0;
            for (int i = 0; i < (block_len / 8); ++i)
            {
                init[0] ^= buff[0 + i * 8];
                init[1] ^= buff[1 + i * 8];
                init[2] ^= buff[2 + i * 8];
                init[3] ^= buff[3 + i * 8];
                init[4] ^= buff[4 + i * 8];
                init[5] ^= buff[5 + i * 8];
                init[6] ^= buff[6 + i * 8];
                init[7] ^= buff[7 + i * 8];

                rc = DesBlockEncrypt(key8, init, CipherMode.ECB, ref mac);
                logger.InfoFormat("i={0},mac={1}", i, Utility.ByteArrayToHexStr(mac,init.Length));
                Array.Copy(mac, init, 8);

            }
            //init��������ֵ,mac��֮���               
            return 0;
        }

        #region ����FM1208 COS�ĵ��������ļ��ܽ����㷨
        /// <summary>
        /// �ɹ��������ĳ��ȣ����򷵻�0.
        /// </summary>
        /// <param name="data">����������</param>
        /// <param name="key8">8�ֽ���Կ</param>
        /// <param name="enc">����</param>
        /// <returns></returns>
        public static int PBOC_DesEnc_Key8(byte[] data, byte[] key8, ref byte[] enc)
        {
            if (key8.Length != 8) throw new ArgumentException("��Կ���ȱ�����8�ֽ�.", "key8");
            int srcLen = data.Length + 1;
            int padLen = srcLen % 8==0?0:(8-(srcLen % 8));
            byte[] src = new byte[srcLen + padLen];
            Array.Clear(src, 0, 0);
            if (padLen > 0) src[srcLen] = 0x80;
            src[0] = (byte)data.Length;
            Array.Copy(data, 0, src, 1, data.Length);
            byte[] buff8 = new byte[8];
            byte[] rc8=new byte[8];
            int rc = 0;
            for (int i = 0; i < src.Length / 8; i++)
            {
                Array.Copy(src, i * 8, buff8, 0, 8);
                rc=DesBlockEncrypt(key8, buff8, CipherMode.ECB, ref rc8);
                logger.InfoFormat(Utility.ByteArrayToHexStr(rc8));
                Array.Copy(rc8, 0, enc, i * 8, 8);                
            }
            return src.Length;
            
        }


        public static int PBOC_DesDec_Key8(byte[] enc, byte[] key8, ref byte[] dec)
        {
            if (enc.Length % 8 != 0) throw new ArgumentException("���ı�����8�ı�������.", "enc");
            if (key8.Length != 8) throw new ArgumentException("��Կ���ȱ�����8�ֽ�.", "key8");
            if (dec.Length < enc.Length) throw new ArgumentException("���������dec�ĳ��ȱ���������ĳ���.", "dec");

            byte[] buff8 = new byte[8];
            byte[] rc8 = new byte[8];
            int rc = 0;

            for (int i = 0; i < enc.Length / 8; i++)
            {
                Array.Copy(enc, i * 8, buff8, 0, 8);
                rc = DesBlockDecrypt(buff8, CipherMode.ECB, PaddingMode.None, key8, ref rc8);
                System.Diagnostics.Trace.Assert(rc == 8);
                Array.Copy(rc8, 0, dec, i * 8, 8);
            }
            return enc.Length;
        }


        public static int PBOC_DesEnc_Key16(byte[] data, byte[] key16, ref byte[] enc)
        {
            if (key16.Length !=16) throw new ArgumentException("��Կ���ȱ�����16�ֽ�.", "key16");
            byte[] lkey8 = new byte[8];
            byte[] rkey8 = new byte[8];
            Array.Copy(key16, lkey8, 8);
            Array.Copy(key16, 8, rkey8, 0, 8);

            int srcLen = data.Length + 1;
            int padLen = srcLen % 8 == 0 ? 0 : (8 - (srcLen % 8));
            byte[] src = new byte[srcLen + padLen];
            Array.Clear(src, 0, 0);
            if (padLen > 0) src[srcLen] = 0x80;
            src[0] = (byte)data.Length;
            Array.Copy(data, 0, src, 1, data.Length);
            byte[] buff8 = new byte[8];
            byte[] rc8 = new byte[8];
            int rc = 0;

            for (int i = 0; i < src.Length / 8; i++)
            {
                Array.Copy(src, i * 8, buff8, 0, 8);
                rc = DesBlockEncrypt(lkey8, buff8, CipherMode.ECB, ref rc8);
                rc = DesBlockDecrypt(rc8, CipherMode.ECB, PaddingMode.None, rkey8, ref buff8);
                rc = DesBlockEncrypt(lkey8, buff8, CipherMode.ECB, ref rc8);
                
                Array.Copy(rc8, 0, enc, i * 8, 8);
            }
            return src.Length;


            
        }


        public static int PBOC_DesDec_Key16(byte[] enc, byte[] key16, ref byte[] data)
        {
            if (enc.Length % 8 != 0) throw new ArgumentException("���ı�����8�ı�������.", "enc");
            if (key16.Length != 16) throw new ArgumentException("��Կ���ȱ�����16�ֽ�.", "key16");
            if (data.Length < enc.Length) throw new ArgumentException("���������data�ĳ��ȱ���������ĳ���.", "data");
            byte[] lkey8 = new byte[8];
            byte[] rkey8 = new byte[8];
            Array.Copy(key16, lkey8, 8);
            Array.Copy(key16, 8, rkey8, 0, 8);

            byte[] buff8 = new byte[8];
            byte[] rc8 = new byte[8];
            int rc = 0;

            for (int i = 0; i < enc.Length /8; i++)
            {
                Array.Copy(enc, i * 8, buff8, 0, 8);

                rc = DesBlockDecrypt(buff8, CipherMode.ECB, PaddingMode.None, lkey8, ref rc8);
                rc = DesBlockEncrypt(rkey8, rc8, CipherMode.ECB, ref buff8);
                rc = DesBlockDecrypt(buff8, CipherMode.ECB, PaddingMode.None, lkey8, ref rc8);
                
                System.Diagnostics.Trace.Assert(rc == 8);
                Array.Copy(rc8, 0, data, i * 8, 8);
            }
            return enc.Length;


        }

        #endregion


        //=============================



        /// <summary>
        /// ʹ��16�ֽڵ���Կkey16��������data��MAC����ʹ��3DES�㷨��ע�⣺MAC����������8�ֽ�,ʵ��ֻȡ���ս�������4�ֽ�ʹ�á�
        /// �ú���ͨ����֤��
        /// </summary>
        /// <param name="data">������MAC�����ݡ�</param>
        /// <param name="key16">16�ֽ���Կ��</param>
        /// <param name="init">8�ֽ��������</param>
        /// <param name="mac">����ó���MAC���ݣ�����Ϊ8�ֽڡ�PBOC�涨���յõ��ǴӼ��������ȡ�õ�4�ֽڳ��ȵ�MAC��</param>
        /// <returns>�ɹ�����0�����󷵻ط�0�����롣</returns>
        public static int PBOC_GetKey16MAC(byte[] data, byte[] key16, byte[] init, ref byte[] mac)
        {
            System.Diagnostics.Trace.Assert(key16.Length == 16, "KEY���ȴ���Ӧ����16�ֽ�");
            System.Diagnostics.Trace.Assert(init.Length == 8, "IV���ȴ���Ӧ����8�ֽ�");

            logger.InfoFormat("data={0} key={1} IV={2}",
                Utility.ByteArrayToHexStr(data),
                Utility.ByteArrayToHexStr(key16),
                Utility.ByteArrayToHexStr(init));
            int block_len = data.Length;
            int zeroPad = 0;
            if (block_len % 8 != 0)
            {
                zeroPad = 8 - (block_len % 8);
                block_len += zeroPad;
            }
            else
            {
                block_len += 8;
            }
            byte[] buff = new byte[block_len];
            Array.Clear(buff, 0, block_len);
            Array.Copy(data, buff, data.Length);

            if (zeroPad == 0)
                buff[block_len - 8] = 0x80;
            else
                buff[block_len - zeroPad] = 0x80;


            //======����׼������Ҫ����MAC������ buff========
            logger.InfoFormat("������MAC������:{0}", Utility.ByteArrayToHexStr(buff));

            //byte[] init = new byte[8];
            //Array.Clear(init, 0, 8);
            int rc = 0;
            byte[] key8left = new byte[8];
            Array.Copy(key16, 0, key8left, 0, 8);//����벿��copy��key8   

            byte[] result = new byte[8];

            for (int i = 0; i < (block_len / 8); ++i)
            {
                init[0] ^= buff[0 + i * 8];
                init[1] ^= buff[1 + i * 8];
                init[2] ^= buff[2 + i * 8];
                init[3] ^= buff[3 + i * 8];
                init[4] ^= buff[4 + i * 8];
                init[5] ^= buff[5 + i * 8];
                init[6] ^= buff[6 + i * 8];
                init[7] ^= buff[7 + i * 8];
                rc = DesBlockEncrypt(key8left, init, CipherMode.ECB, ref result);
                Array.Copy(result, init, 8);
                Array.Clear(result, 0, 8);
            }

            byte[] key8right = new byte[8];
            Array.Copy(key16, 8, key8right, 0, 8);
            //
            Array.Clear(result, 0, 8);
            DesBlockDecrypt(init, CipherMode.ECB, PaddingMode.None, key8right, ref result);
            Array.Copy(result, init, 8);
            DesBlockEncrypt(key8left, init, CipherMode.ECB, ref result);
            Array.Copy(result, mac, 8);
            return 0;

        }
        

       
        /// <summary>
        /// PBOC�涨��ȡ����
        /// </summary>
        /// <param name="In">��������</param>
        /// <param name="in_len">�������ݴ�С</param>
        /// <param name="Out">�������</param>
        /// <returns>�ɹ�����0���쳣���ط�0��</returns>
        public static int PBOC_Anti(byte[] In, int in_len,ref byte[] Out)   
        {

            if (In == null || Out.Length < in_len ) return -1;   
            for (int i=0; i<in_len; i++)   
            {   
                Out[i] = (byte)(~In[i]&0xFF);   
            }   
            return 0;   
        }

 
        /// <summary>
        /// PBOC��Կ��ɢ�㷨 .��Ч��PBOC_Diversify128������
        /// ZL := ALG(IMK)[Y] 
        /// ZR := ALG(IMK)[Y��(��FF�� || ��FF�� || ��FF�� || ��FF�� || ��FF�� || ��FF�� || ��FF�� || ��FF��)] 
        /// Z := (ZL || ZR) 
        /// </summary>
        /// <param name="Km">������Կ</param>
        /// <param name="Seed">��ɢ����.�����ɢ����С��8���ֽڣ���F��ֱ����8�ֽڣ�
        /// Ҳ��16�����֣�ÿ2������1���ֽڡ������ɢ���Ӵ��ڵ���8�ֽڣ���ȡ�Ҷ�8�ֽڡ�
        /// </param>
        /// <param name="Out">�������</param>
        /// <returns>�ɹ�����0���쳣���ط�0��.</returns>
        public static int PBOC_Diversify64(byte[] Km, byte[] Seed, ref byte[] Out)   
        {   
            if (Km.Length != 16)
            {
                logger.ErrorFormat("����Կ���Ȳ���16�ֽڡ�");
                return -1;
            }
            if (Out.Length < 16)
            {
                logger.ErrorFormat("��ɢ��Կ������̫С��");
                return -1;
            }
          
            byte[] AntiSeed = new byte[8];
            Array.Clear(AntiSeed, 0, AntiSeed.Length);
         
            byte[] Seed8 = new byte[8];
            for (int i = 0; i < Seed8.Length; ++i)
            {
                Seed8[i] = 0xFF;
            }

            if (Seed.Length <= 8)
            {
                Array.Copy(Seed, Seed8, Seed.Length);
            }
            else
            {
                Array.Copy(Seed, 0, Seed8, 0, 8);
            }
            if (PBOC_Anti(Seed8, Seed8.Length,ref AntiSeed) != 0) return -1;
            //=======��ɢ����׼����========      
            
            byte[] iv = new byte[8];
            Array.Clear(iv, 0, iv.Length);
            //3DES_ECB����
            byte[] ZL = TripleDESEncrypt(Seed8,
               System.Security.Cryptography.CipherMode.ECB,
               System.Security.Cryptography.PaddingMode.None,
               Km, iv);
            Array.Clear(iv, 0, iv.Length);
            byte[] ZR = TripleDESEncrypt(AntiSeed,
              System.Security.Cryptography.CipherMode.ECB,
              System.Security.Cryptography.PaddingMode.None,
              Km, iv);

            Array.Copy(ZL, Out, 8);
            Array.Copy(ZR, 0, Out, 8, 8);       
            return 0;   
        }

        /// <summary>
        /// ����128λ��������㷨�ļ��㷽��.��Ч��PBOC_Diversify64������
        /// Z := ALG(IMK)[Y || (Y��(��FF�� || ��FF�� || ��FF�� || ��FF�� || ��FF�� || ��FF�� || ��FF�� || ��FF��))] 
        /// </summary>
        /// <param name="Km">������Կ</param>
        /// <param name="Seed">��ɢ����</param>
        /// <param name="Out">���������</param>
        /// <returns>�ɹ�����0���쳣���ط�0��</returns>
        public static int PBOC_Diversify128(byte[] Km, byte[] Seed, ref byte[] Out)
        {
            if (Km.Length != 16)
            {
                logger.ErrorFormat("����Կ���Ȳ���16�ֽڡ�");
                return -1;
            }
            if (Out.Length < 16)
            {
                logger.ErrorFormat("��ɢ��Կ������̫С��");
                return -1;
            }
            byte[] AntiSeed = new byte[8];
            Array.Clear(AntiSeed, 0, AntiSeed.Length);

            byte[] Seed8 = new byte[8];
            for (int i = 0; i < Seed8.Length; ++i)
            {
                Seed8[i] = 0xFF;
            }

            if (Seed.Length <= 8)
            {
                Array.Copy(Seed, Seed8, Seed.Length);
            }
            else
            {
                Array.Copy(Seed, 0, Seed8, 0, 8);
            }
            if (PBOC_Anti(Seed8, Seed8.Length, ref AntiSeed) != 0) return -1;
            byte[] Seed16 = new byte[16];
            Array.Copy(Seed8, Seed16, 8);
            Array.Copy(AntiSeed,0, Seed16, 8,8);
            TripDesBlockEncrypt(Km, Seed16,  CipherMode.ECB,ref Out);
            return 0;
        }


       



        /// <summary>
        /// ������ 9000|ִ�гɹ� ���ַ����������ֵ���Ŀ�����SW������X�����������֣����滻��0��
        /// </summary>
        /// <param name="line"></param>
        /// <param name="kvp"></param>
        /// <returns></returns>
        public static bool ReadCosSWItem(string line, out KeyValuePair<UInt16, string> kvp)
        {
            string[] strs= line.Split('|');
            if (strs != null && strs.Length == 2)
            {
              
                strs[0] = strs[0].Replace('x', '0');
                strs[0] = strs[0].Replace('X', '0');
                UInt16 sw=0;
                if (UInt16.TryParse(strs[0].Trim(), System.Globalization.NumberStyles.AllowHexSpecifier, null, out sw))
                {
                    kvp = new KeyValuePair<ushort, string>(sw, strs[1].Trim());
                    return true;
                }
            }
            else
            {
                logger.WarnFormat("��Ч��SW Item���壺{0}", line);               
            }
            kvp = new KeyValuePair<ushort, string>(0, String.Empty);
            return false;
        }

        public static bool ReadCosIOItem(string line, out KeyValuePair<UInt16, string> kvp)
        {
            string[] strs = line.Split('|');
            if (strs != null && strs.Length == 2)
            {
                string code = strs[0].Trim().Replace('x','0').Replace('X','0');

                UInt16 cmd = 0;
                if (UInt16.TryParse(code, System.Globalization.NumberStyles.HexNumber, null, out cmd))
                {
                    kvp = new KeyValuePair<UInt16, string>(cmd, strs[1].Trim());
                    return true;
                }
                else
                {
                    logger.ErrorFormat("{0}�޷�������COS״̬�ֻ�������", line);
                }
            }
            else
            {
                logger.WarnFormat("��Ч��Cmd/SW Item���壺{0}", line);
            }
            kvp = new KeyValuePair<UInt16, string>(0, String.Empty);
            return false;
        }

        /// <summary>
        /// �ӻ������л�ȡ״̬�֡�״̬����ĩβ2�ֽ����ݣ���9000,6130���������֡�
        /// </summary>
        /// <param name="rlen">���ճ���</param>
        /// <param name="rbuff">���ջ�����</param>
        /// <returns>��ȡ��״̬��</returns>
        public static ushort GetSW(byte rlen, byte[] rbuff)
        {
            if (rlen < 2) return 0;
            ushort rc = rbuff[rlen - 2];
            rc <<= 8;
            rc += rbuff[rlen - 1];
            return rc;
        }

        /// <summary>
        /// ����λ���ȵ��ֽ�����ת����������
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public static int ConvertByteArrayToInt32(byte[] buff)
        {
            return ConvertByteArrayToInt32(buff, 0);
        }

        public static int ConvertByteArrayToInt32(byte[] buff, int offset)
        {
            if (buff == null) throw new ArgumentNullException("buff");
            if ((buff.Length - offset) > 4)
            {
                logger.WarnFormat("�ֽ����鳤�ȴ���4��ֻת����4�ֽڡ�");
            }
            int rc = 0;
            int v = 0;
            for (int i = offset; i < (offset+4); ++i)
            {
                v = buff[i];
                v <<= (8 * (3-(i-offset)));
                rc |= v;
            }
            return rc;
        }

        /// <summary>
        /// ������Int32ת�ɸ�λ���ȵ�byte���飻
        /// </summary>
        /// <param name="iValue"></param>
        /// <param name="buff"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static int ConvertInt32ToByteArray(int iValue, byte[] buff, int offset)
        {
            if (buff == null || buff.Length < 4) return -1;
            buff[offset] = (byte)(iValue >> 24);
            buff[offset+1] = (byte)((iValue >> 16) & 0xFF);
            buff[offset + 2] = (byte)((iValue >> 8) & 0xFF);
            buff[offset + 3] = (byte)(iValue & 0xFF);
            return 0;
        }

        /// <summary>
        /// ������UInt32ת�ɸ�λ���ȵ�byte���飻
        /// </summary>
        /// <param name="iValue"></param>
        /// <param name="buff"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static int ConvertUInt32ToByteArray(UInt32 iValue, byte[] buff, int offset)
        {
            if (buff == null || buff.Length < 4) return -1;
            buff[offset] = (byte)(iValue >> 24);
            buff[offset + 1] = (byte)((iValue >> 16) & 0xFF);
            buff[offset + 2] = (byte)((iValue >> 8) & 0xFF);
            buff[offset + 3] = (byte)(iValue & 0xFF);
            return 0;
        }


        /// <summary>
        /// ������UInt16ת�ɸ�λ���ȵ�byte���飻
        /// </summary>
        /// <param name="iValue"></param>
        /// <param name="buff"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static int ConvertUInt16ToByteArray(UInt16 iValue, byte[] buff, int offset)
        {
            if (buff == null || buff.Length <2) return -1;            
            buff[offset] = (byte)((iValue >> 8) & 0xFF);
            buff[offset + 1] = (byte)(iValue & 0xFF);
            return 0;
        }

        public static UInt16 ConvertByteArrayToUInt16(byte[] buff,int offset)
        {
            UInt16 rc = 0;
            UInt16 v=0;
            for (int i = offset; i < (2 + offset); ++i)
            {
                v= buff[i];
                v <<= (8 * (1 - (i - offset)));
                rc |= v;
            }
            return rc;
        }
       


         
        public static string ResovleATS(byte rlen, byte[] rbuff)
        {
            if (rlen >=13)
            {
                StringBuilder s = new StringBuilder();
                s.AppendFormat("TL={0,2:X2} �����ֽ� ", rbuff[0]);
                s.AppendFormat(" / ");
                s.AppendFormat("T0={0,2:X2} TA1 ����={1},TB1 ����={2},TC1 ����={3} ,FSCI={4}bit",
                    rbuff[1], ((rbuff[1] & 0x40) != 0), ((rbuff[1] & 0x20) != 0), ((rbuff[1] & 0x10) != 0), rbuff[1] & 0x0F);
                s.AppendFormat(" / ");
                if ((rbuff[1] & 0x40) != 0)
                {
                    s.AppendFormat("TA1={0,2:X2} ", rbuff[2]);
                    s.AppendFormat(" / ");
                }
                if ((rbuff[1] & 0x20) != 0)
                {
                    s.AppendFormat("TB1={0,2:X2} FWI={1,1:X1},SFGI={2,1:X1}", rbuff[3], (rbuff[3] & 0xF0) >> 4, (rbuff[3] & 0x0F));
                    s.AppendFormat(" / ");
                }
                if ((rbuff[1] & 0x10) != 0)
                {
                    s.AppendFormat("TC1={0,2:X2}", rbuff[4]);
                    s.AppendFormat(" / ");
                }
                s.AppendFormat("T1={0,2:X2} COS�汾", rbuff[rlen - 11]);
                s.AppendFormat(" / ");
                s.AppendFormat("T2={0,2:X2} COS���̴���", rbuff[rlen - 10]);
                s.AppendFormat(" / ");
                s.AppendFormat("T3={0,2:X2} �����ֽ�", rbuff[rlen - 9]);
                s.AppendFormat(" / ");
                s.AppendFormat("Card SN={0}", ByteArrayToHexStr(rbuff, rlen - 8, 8, ""));
                
                return s.ToString();
            }
            return String.Format("ATS={0}", ByteArrayToHexStr(rbuff, rlen, " "));
        }
        /// <summary>
        /// ��Byte����ת����ASCII�ַ���������ת�����ַ�����.��ʾ��
        /// </summary>
        /// <param name="rlen"></param>
        /// <param name="rbuff"></param>
        /// <returns></returns>
        public static string ByteArrayToAsciiString(ushort rlen, byte[] rbuff)
        {
            StringBuilder s = new StringBuilder(rlen);
            for (int i = 0; i < rlen; i++)
            {
                if (rbuff[i] < 127 && rbuff[i] > 31)
                {
                    s.Append((char)rbuff[i]);
                }
                else if (rbuff[i] > 127 && rbuff[i] < 255)
                {
                    s.Append((char)rbuff[i]);
                }
                else
                {
                    s.Append('.');
                }
            }
            return s.ToString();
        }

        /// <summary>
        /// ��byte����ת����GB18030�ַ���
        /// </summary>
        /// <param name="rlen"></param>
        /// <param name="rbuff"></param>
        /// <returns></returns>
        public static string ByteArrayToGB18030String(int rlen, byte[] rbuff)
        {
            Encoding encd=System.Text.Encoding.GetEncoding(54936);
            return encd.GetString(rbuff, 0, rlen);            
        }

        /// <summary>
        /// COS��������9000����ʾִ�гɹ�.
        /// </summary>
        /// <param name="rlen"></param>
        /// <param name="rbuff"></param>
        /// <returns></returns>
        public static bool IsSwSuccess(byte rlen, byte[] rbuff)
        {
            return
                (rlen >= 2) && (
                (rbuff[rlen - 2] == 0x90 && rbuff[rlen - 1] == 0x00)
                || (rbuff[rlen - 2] == 0x61 && rbuff[rlen - 1] >0)
                );

        }

        public static string GetSubStringBetweenChars(string src,char leftChar,char rightChar)
        {
            int lkh = src.LastIndexOf(leftChar);
            int rkh = src.IndexOf(rightChar);
            if (lkh < 0 || rkh < 0)
            {
        
                return src;
            }
            return src.Substring(lkh + 1, rkh - lkh - 1);        
        }

        /// <summary>
        /// ���������ַ�֮������ַ���������2���ַ������ڣ�����ֻ���ڲ��֣��򷵻�ԭ�ַ���.
        /// </summary>
        /// <param name="src"></param>
        /// <param name="leftChar"></param>
        /// <param name="rightChar"></param>
        /// <returns></returns>
        public static string GetSubStringBetweenChars(string src, string leftChar, string rightChar)
        {
            int lkh = src.LastIndexOf(leftChar);
            int rkh = src.IndexOf(rightChar);
            if (lkh < 0 || rkh < 0)
            {
          
                return src;
            }
            return src.Substring(lkh + 1, rkh - lkh - 1);
        }


        public static string GetSubStringBetweenCharsInclude(string src, char leftChar, char rightChar)
        {
            int lkh = src.LastIndexOf(leftChar);
            int rkh = src.IndexOf(rightChar);
            if (lkh < 0 || rkh < 0)
            {

                throw new ArgumentOutOfRangeException(String.Format("��ȡ�����ַ�(��)����ַ���ֵʧ�ܣ�ԭ�򣺸�ʽ���� src={0},leftChar={1},rightChar={2}",
                    src, leftChar, rightChar));               
            }
            return src.Substring(lkh , rkh - lkh + 1);    
        }

        public static bool ConvertDateTo745b(DateTime dateVal,ref byte[] result)
        {
            return ConvertDateTo745b((byte)(dateVal.Year % 100), (byte)dateVal.Month, (byte)dateVal.Day, ref result);
        }

        /// <summary>
        /// ����-��-��ת����7bit��-4bit��-5bit�ո�ʽ����ռ2�ֽڣ�
        /// �����ɱ��0~127���ʿɸ���1�����ͣ�100�꣩;��λ��ǰ(��)(���������÷�ʽ,����ʱ�����ֽڸ�λ)
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool ConvertDateTo745b(byte year,byte month,byte day, ref byte[] result)
        {
            if (result.Length < 2)
            {
                logger.ErrorFormat("�������������2�ֽڡ�");
                return false;
            }         
            System.Diagnostics.Debug.Assert(year < 127);
            System.Diagnostics.Debug.Assert(month <= 12);
            System.Diagnostics.Debug.Assert(day <= 31);

            result[0] = (byte)((year << 1) + (month >> 3));
            result[1] = (byte)(((month & 0x07)<<5) + day);

            return true;
        }

        


        public static bool Convert745bToDate(byte[] b745,out byte year,out byte month,out byte day)
        {
            year = 0;
            month = 0; 
            day = 0;

            if (b745 == null || b745.Length != 2)
            {
                logger.ErrorFormat("b745���ڱ�ʾ������2�ֽڡ�");
                return false;
            }
            //===================================
            year = (byte)(b745[0] >> 1);
            month = (byte)(((b745[0] & 0x01) << 3) + (b745[1] >> 5));
            day = (byte)(b745[1] & 0x01F);
            return true;
        }

        /// <summary>
        /// �������ձ��ΪUINT16��7-4-5�����ʽ
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static UInt16 ConvertDateTo745bU16(byte year, byte month, byte day)
        {
            UInt16 u16 = 0;
            byte[] rc = new byte[2];
            if (ConvertDateTo745b(year, month, day, ref rc))
            {
                u16 += rc[0];
                u16 <<= 8;
                u16 += rc[1];                
            }
            return u16;
        }


        /// <summary>
        /// ����-��-��ת����6bit��-4bit��-5bit�ո�ʽ����ռ2�ֽڣ���ĩλԤ����
        /// �����ɱ��0~63����λ��ǰ(��)(���������÷�ʽ,����ʱ�����ֽڸ�λ)
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool ConvertDateTo645b(byte year, byte month, byte day, ref byte[] result)
        {
            if (result.Length < 2)
            {
                logger.ErrorFormat("�������������2�ֽڡ�");
                return false;
            }
            System.Diagnostics.Debug.Assert(year < 64);
            System.Diagnostics.Debug.Assert(month <= 12);
            System.Diagnostics.Debug.Assert(day <= 31);            
            //year����ֽڷ����ƶ�2λ��month�ĸ�2λ���ճ�����2λ��
            result[0] = (byte)((year << 2) + (month >> 2));
            result[1] = (byte)(((month & 0x03) << 6) + (day<<1));
            return true;
        }

        public static bool Convert645bToDate(byte[] b645, out byte year, out byte month, out byte day)
        {
            year = 0;
            month = 0;
            day = 0;
            if (b645 == null || b645.Length != 2)
            {
                logger.ErrorFormat("b645�ṹ��ʾ������2�ֽڡ�");
                return false;
            }

            year = (byte)(b645[0] >> 2);
            month = (byte)(((b645[0] & 0x03) << 2) + (b645[1] >> 6));
            day = (byte)((b645[1] & 0x3E) >> 1);
            return true;
        }


        /// <summary>
        /// �������ձ��ΪUINT16��6-4-5��ʽ
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static UInt16 ConvertDateTo645bU16(byte year, byte month, byte day)
        {
            UInt16 u16 = 0;
            byte[] rc = new byte[2];
            if (ConvertDateTo645b(year, month, day, ref rc))
            {
                u16 += rc[0];
                u16 <<= 8;
                u16 += rc[1];
            }
            return u16;
        }

        #region ����У���
        /// <summary>
        /// ���������ֽڵ�У���
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte CheckSumXor(byte[] input)
        {
            return CheckSumXor(input, 0, input.Length);
        }
        /// <summary>
        /// ����buff�д�0��ʼ��length�ֽڵ����ݵ�У���
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte CheckSumXor(byte[] buff, int length)
        {
            return CheckSumXor(buff, 0, length);
        }


        /// <summary>
        /// ����BUFF�д�startIdx��ʼLength�ֽڵ����ݵ�У��͡�
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="startIdx"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte CheckSumXor(byte[] buff, int startIdx, int length)
        {
            byte cs = 0;
            for (int i = 0; i < length; i++)
            {
                cs ^= buff[startIdx+i];
            }
            return cs;
        }

        #endregion
    }
}
