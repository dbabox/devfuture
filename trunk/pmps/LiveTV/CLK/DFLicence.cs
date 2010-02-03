/*
 *
 * 
 * 服务器具有Vender提供的授权码（根据客户特定信息生成，如用户的名字，许可的客户端数量）。此授权码由Vender使用其私钥签名得到。
 * 客户使用的服务器端软件，内嵌Vender提供的公钥，服务器端软件启动时，先获取客户自身的特征信息（如用户的名字，许可的客户端数量），
 * 然后使用公钥验证特征信息,即校验（特征信息，授权码）,验证通过则允许启动。
 * 
 * 客户端验证有2种模式，第一种是服务器授权模式，第二种是客户机绑定模式。
 * 
 * 当使用服务器授权模式时，客户端软件独立于硬件。需要使用用户名和密码登入服务器，然后才能获得服务。这种情况下，客户端软件其实是免费的。
 * 
 * 当使用客户机绑定模式时，客户端软件与硬件绑定。Vender将提供客户端软件授权码，客户端软件将之存储到宿主机（注册表，本地Licence等），也可存储于服务器上。
 * 客户端软件启动时获取宿主机器的机器码，使用内置的Vender提供的与服务器相同的公钥，校验（机器码，授权码）。
 * 
 * 
 * */

using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.Security.Cryptography;

namespace DevFuture.Common.Security
{
    /// <summary>
    /// 授客户端授权
    /// 从
    /// </summary>
    internal class DFLicence
    {
        /// <summary>
        /// 1024byte的公钥，这是Vender发布的公钥，用于验证每份软件copy。仅当软件与硬件绑定时，使用此密钥认证。
        /// </summary>
        private static readonly string publicRSAKey1024 = "<RSAKeyValue><Modulus>pyCMuKQ/XEtu6a7+hT+gbOmzz4S1M2urOXrFBceyLzP++t2MmQIdxLoX50ko/hneBGhQPhM7Bw/CoOSGW0imWtxN2kWvpHMZ2yx9iSSrb5ZTpw0NYrYHRl8n3Aoe/oX9QIlMcwIWV7hCR/PblsTMkYX3NXmVl383OYR4fPwxMV8=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

#if CLIENT
        /// <summary>
        /// 当服务器需认证客户端时，服务器发布的公钥。注意：每个软件产品唯一，在发布软件时，写入即可，也可从配置中获取。
        /// </summary>
        private static readonly string SCA_publicRSAKey1024 = "<RSAKeyValue><Modulus>sI3JjiSu2nIV3CzVgZ+TVv3zljDI8TwjpGV0pJXQLrntvJHQMMoMVSZ0l8SdfZHYkRKDfGojeN4mjwUq2VuMch2sMgTqoYba39XI92CWuxK9ys62RaKl8T2B0H10MWqeGnm+skaw2Ic2oqmDGAVwhX6Kcfn7isQCcY8XlvEbaJ0=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
#endif

#if SERVER
        /// <summary>
        /// 当服务器需认证客户端时，服务器持有的私钥。注意：每个软件产品唯一，在发布软件时，写入即可，也可从配置中获取。
        /// </summary>
        private static readonly string SCA_privateRSAKey1024 = "<RSAKeyValue><Modulus>sI3JjiSu2nIV3CzVgZ+TVv3zljDI8TwjpGV0pJXQLrntvJHQMMoMVSZ0l8SdfZHYkRKDfGojeN4mjwUq2VuMch2sMgTqoYba39XI92CWuxK9ys62RaKl8T2B0H10MWqeGnm+skaw2Ic2oqmDGAVwhX6Kcfn7isQCcY8XlvEbaJ0=</Modulus><Exponent>AQAB</Exponent><P>3vVhG3lBTEtG819wZXfoGHthLXVXJkhlqHc04PwXl7z0Cn13u8ejJljIoNKWOxXPiWBhp4+SL8bjvx9aGYpSRw==</P><Q>yrfnY3hjEoFYcfz/JVbYXt4GXwA+FKYx9pv8yxph/9cIO1F2UD31kjHovZE175sjJOUE9Hc4uSkhkgmggBbb+w==</Q><DP>EgYgLYpl3vcO60nB2lIRLzl6J6SewPeFUFMisTVv6tJZoCd4cHO5GrZ+sZGUl34x4tcpzdPra3VWn/K24+2srQ==</DP><DQ>QmRozBMpn23tYafSiJAg3TEqNQMHUgv8YSBFct95KSlr8MGFVlJ0kyT1bOCaIZdVs0euj8JcOYhXDlVI06qo8Q==</DQ><InverseQ>huxxzFt220B4OhySJnbFmOEKPsQ2hSXCqn6qd5AX5FE/RDNd3iZ6xsX/38deh1p2lha0MbDS7CG0dvYxHxrmRQ==</InverseQ><D>lsD7EbyQ8UXpFI/T31L65JUDoYMWkaXgaUeYSdECamOodBOtyXs8/JhRhiYEtGwMmBNO4rVehJ5sHkMzS1W4nWzuhUg4SzIguTVF9ta8Y2i8V2UDB54IGSYHHuV3OLejNBb07IGMKHe4qdiTaJnoatwbmJEpo4jUOkjHrgej/qE=</D></RSAKeyValue>";
#endif
        
        /// <summary>
        /// 消息校验码密钥
        /// 生成的校验码总是8个byte
        /// </summary>
        private static readonly byte[] KEY24_MACTripleDES ={ 69, 86, 77, 111, 78, 26, 225, 3, 199, 98, 196, 200, 184, 22, 177, 59, 128, 149, 87, 3, 215, 81, 175, 193 };
        
        /// <summary>
        /// 获得本机硬件ID
        /// </summary>
        /// <returns></returns>
        internal static string GetHID()
        {
            //create out management class object using the
            //Win32_NetworkAdapterConfiguration class to get the attributes
            //af the network adapter
            using (ManagementClass mgmt = new ManagementClass("Win32_NetworkAdapterConfiguration"))
            {
                //create our ManagementObjectCollection to get the attributes with
                using (ManagementObjectCollection objCol = mgmt.GetInstances())
                {
                    string address = String.Empty;
                    //loop through all the objects we find
                    foreach (ManagementObject obj in objCol)
                    {
                        //只赋值一次
                        if (address == String.Empty)  // only return MAC Address from first card
                        {
                            //grab the value from the first network adapter we find
                            //you can change the string to an array and get all
                            //network adapters found as well
                            if ((bool)obj["IPEnabled"] == true) address = obj["MacAddress"].ToString();
                        }
                        //dispose of our object
                        obj.Dispose();
                    }
                    //replace the ":" with an empty space, this could also
                    //be removed if you wish
                    address = address.Replace(":", "");
                    //return the mac address
                    return address;
                }
            }

        }

        /// <summary>
        /// 本机认证
        /// 当使用客户机绑定模式时，客户端软件与硬件绑定。Vender将提供客户端软件授权码(使用Vender的私钥签名客户机器码)，
        /// 客户端软件将之存储到宿主机（注册表，本地Licence等），也可存储于服务器上。
        /// 客户端软件启动时获取宿主机器的机器码，使用内置的Vender提供的与服务器相同的公钥，校验（机器码，授权码）。
        /// </summary>
        /// <param name="caCode">使用RAS-1024生成的授权码(机器码签名)，转化成BASE26字符串，去除了分隔符。共14个字符构成一个UInt64。</param>
        /// <returns></returns>
        internal static bool LocalCA(string caCode)
        {
            //将caCode转化成Base26，双UInt32的字节数组，注意：caCode是一连串字符串，无分隔符，每14个字符构成一个UInt64。
            if (String.IsNullOrEmpty(caCode)||
                caCode.Length % Base26.Bit64Base26MaxLength != 0)
            {
                return false;
            }

            int longCnt = caCode.Length / Base26.Bit64Base26MaxLength;

            byte[] signedData = new byte[longCnt * 8];//签名数据
            long rc = 0;
            for (int i = 0; i < longCnt; i++)
            {
                rc = Base26.Instance.DecodeUInt32CoupleOneStringAsInt64(caCode.Substring(i * Base26.Bit64Base26MaxLength, Base26.Bit64Base26MaxLength));
                BitConverter.GetBytes(rc).CopyTo(signedData, i * 8);
            }            
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(publicRSAKey1024);
                string hid = GetHID();
                return rsa.VerifyData(System.Text.Encoding.UTF8.GetBytes(hid), new SHA1CryptoServiceProvider(), signedData);
            }
             
        }

#if CLIENT
        /// <summary>
        /// 获取本机认证码，以供服务器认证。
        /// </summary>
        /// <returns></returns>
        internal static string SCA_GetLocalCACode()
        {
            
            //step 1:加密
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(SCA_publicRSAKey1024);
                string hid = GetHID();
                System.Diagnostics.Debug.Assert(hid.Length <= 117);
                //128byte
                byte[] rr= rsa.Encrypt(System.Text.Encoding.UTF8.GetBytes(hid), false);
                System.Diagnostics.Debug.Assert(rr.Length == 128);

                using (MACTripleDES macdes = new MACTripleDES(KEY24_MACTripleDES))
                {
                    //这个总是8字节
                    byte[] hash = macdes.ComputeHash(rr);//哈希码
                    System.Diagnostics.Debug.Assert(hash.Length == 8);
                    StringBuilder rcsb = new StringBuilder((rr.Length / 8+1)*Base26.Bit64Base26MaxLength+1);

                    byte[] encTmp=new byte[8];
                    //在这里将128byte密文和8byte校验码组合在一起，构成xxxxxxxx-xxxx结构的CACode。
                    for (int i = 0; i < (rr.Length/8); i++)
                    {
                        Array.Copy(rr, i * 8, encTmp, 0, 8);
                        rcsb.Append(Base26.Instance.Encode8BytesArrayAsUInt32Couple(encTmp));
                    }
                    rcsb.Append("-");
                    rcsb.Append(Base26.Instance.Encode8BytesArrayAsUInt32Couple(hash));
                    return rcsb.ToString();
                }
            }



        }
#else


        /// <summary>
        /// 服务器认证客户端.
        /// </summary>
        /// <param name="sca">客户端机器将自身机器码RAS-1024加密，然后对密文生成MAC校验。</param>
        /// <returns></returns>
        internal static bool ServerAuthenticateClient(string caCode)
        {
            //caCode前14*16个字符是密文，末14个字符串是校验码，中间一个-分隔符
            
            if (caCode.Length != (Base26.Bit64Base26MaxLength * 17)) return false;
            byte[] encMsg = new byte[128];//密文总是128字节
            long rc = 0;
            for (int i = 0; i < 16; i++)
            {
                rc = Base26.Instance.DecodeUInt32CoupleOneStringAsInt64(caCode.Substring(i * Base26.Bit64Base26MaxLength, Base26.Bit64Base26MaxLength));
                BitConverter.GetBytes(rc).CopyTo(encMsg, i * 8);
            }

            using (MACTripleDES macdes = new MACTripleDES(KEY24_MACTripleDES))
            {
                if (BitConverter.ToInt64(macdes.ComputeHash(encMsg),0) !=
                    Base26.Instance.DecodeUInt32CoupleOneStringAsInt64(caCode.Substring(Base26.Bit64Base26MaxLength * 16 + 1, Base26.Bit64Base26MaxLength)))
                {
                    Console.WriteLine("授权码没有通过校验,无效的授权码。");
                    return false;
                }
            }
            //通过了校验
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(SCA_privateRSAKey1024);
                byte[] msgBytes= rsa.Decrypt(encMsg, false);//解密成功
                string msg = Encoding.UTF8.GetString(msgBytes);
                //Note:解密成功，可以对明文进一步处理，例如检查msg在数据库中是否存在；
                return true;               
            }
          
        }
#endif

    }
}
