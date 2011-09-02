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
       /// 检查输入的模式，将字符串转换成Byte数组.
        /// 成功返回转换成功的byte数组的长度，失败返回0。
       /// </summary>
       /// <param name="str"></param>
       /// <param name="resultBuff"></param>
       /// <returns></returns>
        public static int HexStrToByteArray(string str, ref byte[] resultBuff)
        {
            return HexStrToByteArray(str, ref resultBuff, true);
        }

        /// <summary>
        /// 将16进制字符串转换成byte数组。成功返回转换成功的byte数组的长度，失败返回-1。
        /// 注意：字节数组字符串 00 01，表示的是十进制1。故转成字节数组则成为byte[0]=0x01,byte[1]=0x00.
        /// </summary>
        /// <param name="str">待转换的字符串，如"001A0BC0"</param>
        /// <param name="resultBuff">输出参数，用于存放转换后的结果</param>
        /// <param name="checkInput">true表示检查输入，false表示不检查。不检查时性能好。</param>
        /// <returns>返回转换成功的byte数组的长度.</returns>
        public static int HexStrToByteArray(string str, ref byte[] resultBuff, bool checkInput)
        {
            Array.Clear(resultBuff, 0, resultBuff.Length);
            string trim = System.Text.RegularExpressions.Regex.Replace(str, @"\s", "");
            if (checkInput)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(trim, "^[A-Fa-f0-9]+$"))
                {
                    logger.WarnFormat("待转换字符串 {0} 数字不是16进制字符串！", trim);
                    return 0;
                }
                if (trim.Length % 2 != 0)
                {
                    logger.WarnFormat("待转换字符串 {0} 数字的个数不为偶数，无法转成16进制！", trim);
                    return 0;
                }
            }
            int rc = trim.Length / 2;
            if (resultBuff.Length < rc) throw new ArgumentException("结果buffer长度太小！", "resultBuff");
            for (int i = 0; i < rc; i++)
                resultBuff[i] = Convert.ToByte(trim.Substring(i * 2, 2), 16);
            return rc;
        }

        /// <summary>
        /// Byte数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ByteArrayToHexStr(byte[] bytes)
        {
            return ByteArrayToHexStr(bytes, bytes.Length);
        }
        

        /// <summary>
        /// Byte数组转16进制字符串
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
        /// 格式化字节数组，以spchar为分割符。
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
            if ((offset + len) > bytes.Length) throw new ArgumentException("offset+len大于数组长度！");
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
                        logger.WarnFormat("无效的命令字符串：{0}", line);
                    }

                }
            }
            return strList;
        }


        #region 3DES相关

        /// <summary>
        /// 3DES加密1个16字节快 要求KEY>=16字节，且为8的倍数。返回转换成功的结果字节数。
        /// 当用16字节密钥，8字节随机数，做外部认证加密时，需使用ECB模式。该函数通过认证。
        /// 明文inputValue必须是8的倍数长度.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="inputValue"></param>
        /// <param name="rbuff"></param>
        /// <param name="CipherMode">ECB或者CBC。在CPU卡交易过程中作外部认证时，使用的是ECB模式。</param>
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
            
            tripdes.BlockSize = 64;          //以8位为块加密
            byte[] iv = new byte[tripdes.BlockSize/8]; //8字节向量，全0
            Array.Clear(iv, 0, iv.Length);
            tripdes.KeySize = 128;          //16字节密钥
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
        /// 3DES加密
        /// </summary>
        public static byte[] TripleDESEncrypt(byte[] bySource, CipherMode mCipherMode, PaddingMode mPaddingMode, byte[] mbyKey, byte[] mbyIV)
        {
          
            TripleDESCryptoServiceProvider MyServiceProvider = new TripleDESCryptoServiceProvider();

            //计算des加密所采用的算法

            MyServiceProvider.Mode = mCipherMode;

            //计算填充类型
            MyServiceProvider.Padding = mPaddingMode;

            //TripleDESCryptoServiceProvider
            //支持从 128 位到 192 位（以 64 位递增）的密钥长度
            //IV需要8个字节
            //设置KEY时要注意的是可能引发CryptographicException异常，主要是因为所设置的KEY为WeakKey
            ICryptoTransform MyTransform = MyServiceProvider.CreateEncryptor(mbyKey, mbyIV);
            //CryptoStream对象的作用是将数据流连接到加密转换的流
            MemoryStream ms = new MemoryStream();
            CryptoStream MyCryptoStream = new CryptoStream(ms, MyTransform, CryptoStreamMode.Write);
            //将字节数组中的数据写入到加密流中
            MyCryptoStream.Write(bySource, 0, bySource.Length);
            MyCryptoStream.FlushFinalBlock();
            MyCryptoStream.Close();
            byte[] byEncRet = ms.ToArray();
            ms.Close();
            return byEncRet;
            
        }

        /// <summary>
        /// 3DES解密
        /// </summary>
        public static int TripleDESDecrypt(byte[] data,byte[] key16,CipherMode mCipherMode, PaddingMode mPaddingMode,ref byte[] rbuff)
        {
            byte[] mbyIV = new byte[8];//初始向量 全0

            using (TripleDESCryptoServiceProvider MyServiceProvider = new TripleDESCryptoServiceProvider())
            {

                //计算des加密所采用的算法

                MyServiceProvider.Mode = mCipherMode;

                //计算填充类型
                MyServiceProvider.Padding = mPaddingMode;

                //TripleDESCryptoServiceProvider
                //支持从 128 位到 192 位（以 64 位递增）的密钥长度
                //IV需要8个字节
                //设置KEY时要注意的是可能引发CryptographicException异常，主要是因为所设置的KEY为WeakKey
                ICryptoTransform MyTransform = MyServiceProvider.CreateDecryptor(key16, mbyIV);
                //CryptoStream对象的作用是将数据流连接到加密转换的流
                MemoryStream ms = new MemoryStream();
                CryptoStream MyCryptoStream = new CryptoStream(ms, MyTransform, CryptoStreamMode.Write);
                //将字节数组中的数据写入到加密流中
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
        /// 使用MACTripleDES计算消息的MAC
        /// </summary>
        /// <param name="key">最大32字节的KEY</param>
        /// <param name="iv">最大8字节的初始向量</param>
        /// <param name="inputValue">待计算MAC的数据，长度为2的倍数。</param>
        /// <param name="rbuff">MAC输出，长度为8字节</param>
        /// <returns></returns>
        //public static int TripDesMac(byte[] key, byte[] iv, byte[] inputValue, ref byte[] rbuff)
        //{
        //    MACTripleDES mac = new MACTripleDES();
        //    mac.Key = key;



        //}
        #endregion

        #region DES算法相关

        /// <summary>
        /// DES加密一个8字节块。key必须为8字节。返回转换成功的结果字节数。
        /// </summary>
        /// <param name="key">8字节密钥</param>
        /// <param name="inputValue">待加密数据8字节。也是初始向量值。</param>
        /// <param name="cm">加密模式，一般应该是ECB.</param>
        /// <param name="rbuff">加密后的结果</param>
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
            des.KeySize = 64;          //8字节密钥
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
        /// DES解密
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
        /// DES加密任意长度数据
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

                //计算des加密所采用的算法

                MyServiceProvider.Mode = mCipherMode;

                //计算填充类型
                MyServiceProvider.Padding = mPaddingMode;

                ICryptoTransform MyTransform = MyServiceProvider.CreateEncryptor(mbyKey, mbyIV);
                //CryptoStream对象的作用是将数据流连接到加密转换的流
                MemoryStream ms = new MemoryStream();
                CryptoStream MyCryptoStream = new CryptoStream(ms, MyTransform, CryptoStreamMode.Write);
                //将字节数组中的数据写入到加密流中
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
        /// DES 解密数据
        /// </summary>
        /// <param name="bySource">要解密的数据</param>
        /// <param name="mCipherMode">DES加密所采用的算法模式，默认CBC</param>
        /// <param name="mPaddingMode">指定在消息数据块比加密操作所需的全部字节数短时应用的填充类型，默认不填充None.</param>
        /// <param name="mbyKey">8字节密钥</param>
        /// <param name="mbyIV">8字节初始化向量，默认8个0</param>
        /// <returns></returns>
        public static byte[] DESDecrypt(byte[] bySource, CipherMode mCipherMode, PaddingMode mPaddingMode, byte[] mbyKey, byte[] mbyIV)
        {
            try
            {
                DESCryptoServiceProvider MyServiceProvider = new DESCryptoServiceProvider();
                //计算des加密所采用的算法
                MyServiceProvider.Mode = mCipherMode;
                //计算填充类型
                MyServiceProvider.Padding = mPaddingMode;
                ICryptoTransform MyTransform = MyServiceProvider.CreateDecryptor(mbyKey, mbyIV);
                //CryptoStream对象的作用是将数据流连接到加密转换的流
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                CryptoStream MyCryptoStream = new CryptoStream(ms, MyTransform, CryptoStreamMode.Write);
                //将字节数组中的数据写入到加密流中
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
        /// MAC计算所要采用的CBC DES算法实现
        /// </summary>
        public static byte[] HCDES(byte[] Key, byte[] Data)
        {
            try
            {
                //创建一个DES算法的加密类
                DESCryptoServiceProvider MyServiceProvider = new DESCryptoServiceProvider();
                MyServiceProvider.Mode = CipherMode.CBC;
                MyServiceProvider.Padding = PaddingMode.None;
                //从DES算法的加密类对象的CreateEncryptor方法,创建一个加密转换接口对象
                //第一个参数的含义是：对称算法的机密密钥(长度为64位,也就是8个字节)
                // 可以人工输入,也可以随机生成方法是：MyServiceProvider.GenerateKey();
                //第二个参数的含义是：对称算法的初始化向量(长度为64位,也就是8个字节)
                // 可以人工输入,也可以随机生成方法是：MyServiceProvider.GenerateIV()
                //
                //默认初始化向量8个0
                ICryptoTransform MyTransform = MyServiceProvider.CreateEncryptor(Key, new byte[8]);

                //CryptoStream对象的作用是将数据流连接到加密转换的流
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                CryptoStream MyCryptoStream = new CryptoStream(ms, MyTransform, CryptoStreamMode.Write);
                //将字节数组中的数据写入到加密流中


                MyCryptoStream.Write(Data, 0, Data.Length);
                //关闭加密流对象
                byte[] bEncRet = new byte[8];
                // Array.Copy(ms.GetBuffer(), bEncRet, ms.Length);
                bEncRet = ms.ToArray(); // MyCryptoStream关闭之前ms.Length 为8， 关闭之后为16

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
        /// 计算MacData的数据的MAC值。
        /// </summary>
        /// <param name="MacData">要计算MAC的数据。</param>
        /// <param name="mbyKey">密钥值</param>
        /// <param name="mbyIV">初始化向量，默认应是8个0。</param>
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
        /// 为某个命令计算MAC。用于明文+MAC的那些命令。
        /// 使用DES算法,Key为8字节计算MAC.
        /// 
        /// </summary>
        /// <param name="rn4">4字节随机数</param>
        /// <param name="cipp4">CLA,INC,P1,P2</param>
        /// <param name="lc">Lc</param>
        /// <param name="data">数据域</param>
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
            //======以上准备好了要计算MAC的数据 buff========
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
            //init就是最终值,mac与之相等       
            return 0;
        }

     
        /// <summary>
        /// 用8字节密钥key8计算数据data的mac。注意：MAC计算结果总是8字节,实际只取最终结果的左侧4字节使用。       
        /// </summary>
        /// <param name="data">待计算MAC的数据。</param>
        /// <param name="key8">8字节密钥，用来计算MAC。</param>
        /// <param name="init">8字节随机数。</param>
        /// <param name="mac">计算得出的MAC数据，长度为8字节。PBOC规定最终得到是从计算结果左侧取得的4字节长度的MAC。</param>
        /// <returns>成功返回0，错误返回非0错误码。</returns>
        public static int PBOC_GetKey8MAC(byte[] data, byte[] key8,byte[] init, ref byte[] mac)
        {
            System.Diagnostics.Trace.Assert(key8.Length == 8, "KEY长度错误，应该是8字节");
            System.Diagnostics.Trace.Assert(init.Length == 8, "IV长度错误，应该是8字节");

            logger.InfoFormat("data={0} key8={1} IV={2}", 
                Utility.ByteArrayToHexStr(data), 
                Utility.ByteArrayToHexStr(key8), 
                Utility.ByteArrayToHexStr(init));
            int block_len = data.Length;
            int zeroPad = 0;

            //1、不是8的倍数，末尾补0x80,0x00....直到是8的倍数
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

            //======以上准备好了要计算MAC的数据 buff========
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
            //init就是最终值,mac与之相等               
            return 0;
        }

        #region 复旦FM1208 COS文档中描述的加密解密算法
        /// <summary>
        /// 成功返回密文长度，否则返回0.
        /// </summary>
        /// <param name="data">待加密数据</param>
        /// <param name="key8">8字节密钥</param>
        /// <param name="enc">密文</param>
        /// <returns></returns>
        public static int PBOC_DesEnc_Key8(byte[] data, byte[] key8, ref byte[] enc)
        {
            if (key8.Length != 8) throw new ArgumentException("密钥长度必须是8字节.", "key8");
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
            if (enc.Length % 8 != 0) throw new ArgumentException("密文必须是8的倍数长度.", "enc");
            if (key8.Length != 8) throw new ArgumentException("密钥长度必须是8字节.", "key8");
            if (dec.Length < enc.Length) throw new ArgumentException("结果缓冲区dec的长度必须大于密文长度.", "dec");

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
            if (key16.Length !=16) throw new ArgumentException("密钥长度必须是16字节.", "key16");
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
            if (enc.Length % 8 != 0) throw new ArgumentException("密文必须是8的倍数长度.", "enc");
            if (key16.Length != 16) throw new ArgumentException("密钥长度必须是16字节.", "key16");
            if (data.Length < enc.Length) throw new ArgumentException("结果缓冲区data的长度必须大于密文长度.", "data");
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
        /// 使用16字节的密钥key16计算数据data的MAC，即使用3DES算法。注意：MAC计算结果总是8字节,实际只取最终结果的左侧4字节使用。
        /// 该函数通过认证。
        /// </summary>
        /// <param name="data">待计算MAC的数据。</param>
        /// <param name="key16">16字节密钥。</param>
        /// <param name="init">8字节随机数。</param>
        /// <param name="mac">计算得出的MAC数据，长度为8字节。PBOC规定最终得到是从计算结果左侧取得的4字节长度的MAC。</param>
        /// <returns>成功返回0，错误返回非0错误码。</returns>
        public static int PBOC_GetKey16MAC(byte[] data, byte[] key16, byte[] init, ref byte[] mac)
        {
            System.Diagnostics.Trace.Assert(key16.Length == 16, "KEY长度错误，应该是16字节");
            System.Diagnostics.Trace.Assert(init.Length == 8, "IV长度错误，应该是8字节");

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


            //======以上准备好了要计算MAC的数据 buff========
            logger.InfoFormat("待计算MAC的数据:{0}", Utility.ByteArrayToHexStr(buff));

            //byte[] init = new byte[8];
            //Array.Clear(init, 0, 8);
            int rc = 0;
            byte[] key8left = new byte[8];
            Array.Copy(key16, 0, key8left, 0, 8);//将左半部分copy给key8   

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
        /// PBOC规定的取反。
        /// </summary>
        /// <param name="In">输入数据</param>
        /// <param name="in_len">输入数据大小</param>
        /// <param name="Out">输出缓存</param>
        /// <returns>成功返回0，异常返回非0。</returns>
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
        /// PBOC密钥分散算法 .等效于PBOC_Diversify128函数。
        /// ZL := ALG(IMK)[Y] 
        /// ZR := ALG(IMK)[Y⊕(‘FF’ || ‘FF’ || ‘FF’ || ‘FF’ || ‘FF’ || ‘FF’ || ‘FF’ || ‘FF’)] 
        /// Z := (ZL || ZR) 
        /// </summary>
        /// <param name="Km">主控密钥</param>
        /// <param name="Seed">分散因子.如果分散因子小于8个字节，则补F，直到有8字节，
        /// 也即16个数字，每2个数字1个字节。如果分散因子大于等于8字节，则取右端8字节。
        /// </param>
        /// <param name="Out">输出缓冲</param>
        /// <returns>成功返回0，异常返回非0。.</returns>
        public static int PBOC_Diversify64(byte[] Km, byte[] Seed, ref byte[] Out)   
        {   
            if (Km.Length != 16)
            {
                logger.ErrorFormat("主密钥长度不是16字节。");
                return -1;
            }
            if (Out.Length < 16)
            {
                logger.ErrorFormat("分散密钥缓冲区太小。");
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
            //=======分散因子准备好========      
            
            byte[] iv = new byte[8];
            Array.Clear(iv, 0, iv.Length);
            //3DES_ECB加密
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
        /// 基于128位分组加密算法的计算方法.等效于PBOC_Diversify64函数。
        /// Z := ALG(IMK)[Y || (Y⊕(‘FF’ || ‘FF’ || ‘FF’ || ‘FF’ || ‘FF’ || ‘FF’ || ‘FF’ || ‘FF’))] 
        /// </summary>
        /// <param name="Km">主控密钥</param>
        /// <param name="Seed">分散因子</param>
        /// <param name="Out">输出缓冲区</param>
        /// <returns>成功返回0，异常返回非0。</returns>
        public static int PBOC_Diversify128(byte[] Km, byte[] Seed, ref byte[] Out)
        {
            if (Km.Length != 16)
            {
                logger.ErrorFormat("主密钥长度不是16字节。");
                return -1;
            }
            if (Out.Length < 16)
            {
                logger.ErrorFormat("分散密钥缓冲区太小。");
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
        /// 将形如 9000|执行成功 的字符串解析成字典条目。如果SW字中用X代表任意数字，则被替换成0。
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
                logger.WarnFormat("无效的SW Item定义：{0}", line);               
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
                    logger.ErrorFormat("{0}无法解析成COS状态字或命令码", line);
                }
            }
            else
            {
                logger.WarnFormat("无效的Cmd/SW Item定义：{0}", line);
            }
            kvp = new KeyValuePair<UInt16, string>(0, String.Empty);
            return false;
        }

        /// <summary>
        /// 从缓冲区中获取状态字。状态字是末尾2字节数据，如9000,6130这样的数字。
        /// </summary>
        /// <param name="rlen">接收长度</param>
        /// <param name="rbuff">接收缓冲区</param>
        /// <returns>提取的状态码</returns>
        public static ushort GetSW(byte rlen, byte[] rbuff)
        {
            if (rlen < 2) return 0;
            ushort rc = rbuff[rlen - 2];
            rc <<= 8;
            rc += rbuff[rlen - 1];
            return rc;
        }

        /// <summary>
        /// 将高位在先的字节数组转换成整数。
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
                logger.WarnFormat("字节数组长度大于4，只转换低4字节。");
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
        /// 将整数Int32转成高位在先的byte数组；
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
        /// 将整数UInt32转成高位在先的byte数组；
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
        /// 将整数UInt16转成高位在先的byte数组；
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
                s.AppendFormat("TL={0,2:X2} 长度字节 ", rbuff[0]);
                s.AppendFormat(" / ");
                s.AppendFormat("T0={0,2:X2} TA1 存在={1},TB1 存在={2},TC1 存在={3} ,FSCI={4}bit",
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
                s.AppendFormat("T1={0,2:X2} COS版本", rbuff[rlen - 11]);
                s.AppendFormat(" / ");
                s.AppendFormat("T2={0,2:X2} COS厂商代码", rbuff[rlen - 10]);
                s.AppendFormat(" / ");
                s.AppendFormat("T3={0,2:X2} 保留字节", rbuff[rlen - 9]);
                s.AppendFormat(" / ");
                s.AppendFormat("Card SN={0}", ByteArrayToHexStr(rbuff, rlen - 8, 8, ""));
                
                return s.ToString();
            }
            return String.Format("ATS={0}", ByteArrayToHexStr(rbuff, rlen, " "));
        }
        /// <summary>
        /// 将Byte数组转换成ASCII字符串。不能转换的字符，以.表示。
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
        /// 将byte数组转换成GB18030字符串
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
        /// COS返回码是9000，表示执行成功.
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
        /// 返回两个字符之间的子字符串，若这2个字符不存在，或者只存在部分，则返回原字符串.
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

                throw new ArgumentOutOfRangeException(String.Format("获取两个字符(含)间的字符串值失败，原因：格式错误 src={0},leftChar={1},rightChar={2}",
                    src, leftChar, rightChar));               
            }
            return src.Substring(lkh , rkh - lkh + 1);    
        }

        public static bool ConvertDateTo745b(DateTime dateVal,ref byte[] result)
        {
            return ConvertDateTo745b((byte)(dateVal.Year % 100), (byte)dateVal.Month, (byte)dateVal.Day, ref result);
        }

        /// <summary>
        /// 将年-月-日转换成7bit年-4bit月-5bit日格式，共占2字节；
        /// 年最多可表达0~127，故可覆盖1个世纪（100年）;高位在前(左)(即所见所得方式,解析时：低字节高位)
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
                logger.ErrorFormat("输出缓冲区至少2字节。");
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
                logger.ErrorFormat("b745日期表示法至少2字节。");
                return false;
            }
            //===================================
            year = (byte)(b745[0] >> 1);
            month = (byte)(((b745[0] & 0x01) << 3) + (b745[1] >> 5));
            day = (byte)(b745[1] & 0x01F);
            return true;
        }

        /// <summary>
        /// 将年月日表达为UINT16的7-4-5编码格式
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
        /// 将年-月-日转换成6bit年-4bit月-5bit日格式，共占2字节；最末位预留；
        /// 年最多可表达0~63；高位在前(左)(即所见所得方式,解析时：低字节高位)
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
                logger.ErrorFormat("输出缓冲区至少2字节。");
                return false;
            }
            System.Diagnostics.Debug.Assert(year < 64);
            System.Diagnostics.Debug.Assert(month <= 12);
            System.Diagnostics.Debug.Assert(day <= 31);            
            //year向高字节方向移动2位，month的高2位填充空出来的2位；
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
                logger.ErrorFormat("b645结构表示法必须2字节。");
                return false;
            }

            year = (byte)(b645[0] >> 2);
            month = (byte)(((b645[0] & 0x03) << 2) + (b645[1] >> 6));
            day = (byte)((b645[1] & 0x3E) >> 1);
            return true;
        }


        /// <summary>
        /// 将年月日表达为UINT16的6-4-5格式
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

        #region 计算校验和
        /// <summary>
        /// 计算输入字节的校验和
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte CheckSumXor(byte[] input)
        {
            return CheckSumXor(input, 0, input.Length);
        }
        /// <summary>
        /// 计算buff中从0开始，length字节的数据的校验和
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte CheckSumXor(byte[] buff, int length)
        {
            return CheckSumXor(buff, 0, length);
        }


        /// <summary>
        /// 计算BUFF中从startIdx开始Length字节的数据的校验和。
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
