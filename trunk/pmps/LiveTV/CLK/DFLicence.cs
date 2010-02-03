/*
 *
 * 
 * ����������Vender�ṩ����Ȩ�루���ݿͻ��ض���Ϣ���ɣ����û������֣���ɵĿͻ���������������Ȩ����Venderʹ����˽Կǩ���õ���
 * �ͻ�ʹ�õķ��������������ǶVender�ṩ�Ĺ�Կ�����������������ʱ���Ȼ�ȡ�ͻ������������Ϣ�����û������֣���ɵĿͻ�����������
 * Ȼ��ʹ�ù�Կ��֤������Ϣ,��У�飨������Ϣ����Ȩ�룩,��֤ͨ��������������
 * 
 * �ͻ�����֤��2��ģʽ����һ���Ƿ�������Ȩģʽ���ڶ����ǿͻ�����ģʽ��
 * 
 * ��ʹ�÷�������Ȩģʽʱ���ͻ������������Ӳ������Ҫʹ���û�������������������Ȼ����ܻ�÷�����������£��ͻ��������ʵ����ѵġ�
 * 
 * ��ʹ�ÿͻ�����ģʽʱ���ͻ��������Ӳ���󶨡�Vender���ṩ�ͻ��������Ȩ�룬�ͻ��������֮�洢����������ע�������Licence�ȣ���Ҳ�ɴ洢�ڷ������ϡ�
 * �ͻ����������ʱ��ȡ���������Ļ����룬ʹ�����õ�Vender�ṩ�����������ͬ�Ĺ�Կ��У�飨�����룬��Ȩ�룩��
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
    /// �ڿͻ�����Ȩ
    /// ��
    /// </summary>
    internal class DFLicence
    {
        /// <summary>
        /// 1024byte�Ĺ�Կ������Vender�����Ĺ�Կ��������֤ÿ�����copy�����������Ӳ����ʱ��ʹ�ô���Կ��֤��
        /// </summary>
        private static readonly string publicRSAKey1024 = "<RSAKeyValue><Modulus>pyCMuKQ/XEtu6a7+hT+gbOmzz4S1M2urOXrFBceyLzP++t2MmQIdxLoX50ko/hneBGhQPhM7Bw/CoOSGW0imWtxN2kWvpHMZ2yx9iSSrb5ZTpw0NYrYHRl8n3Aoe/oX9QIlMcwIWV7hCR/PblsTMkYX3NXmVl383OYR4fPwxMV8=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

#if CLIENT
        /// <summary>
        /// ������������֤�ͻ���ʱ�������������Ĺ�Կ��ע�⣺ÿ�������ƷΨһ���ڷ������ʱ��д�뼴�ɣ�Ҳ�ɴ������л�ȡ��
        /// </summary>
        private static readonly string SCA_publicRSAKey1024 = "<RSAKeyValue><Modulus>sI3JjiSu2nIV3CzVgZ+TVv3zljDI8TwjpGV0pJXQLrntvJHQMMoMVSZ0l8SdfZHYkRKDfGojeN4mjwUq2VuMch2sMgTqoYba39XI92CWuxK9ys62RaKl8T2B0H10MWqeGnm+skaw2Ic2oqmDGAVwhX6Kcfn7isQCcY8XlvEbaJ0=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
#endif

#if SERVER
        /// <summary>
        /// ������������֤�ͻ���ʱ�����������е�˽Կ��ע�⣺ÿ�������ƷΨһ���ڷ������ʱ��д�뼴�ɣ�Ҳ�ɴ������л�ȡ��
        /// </summary>
        private static readonly string SCA_privateRSAKey1024 = "<RSAKeyValue><Modulus>sI3JjiSu2nIV3CzVgZ+TVv3zljDI8TwjpGV0pJXQLrntvJHQMMoMVSZ0l8SdfZHYkRKDfGojeN4mjwUq2VuMch2sMgTqoYba39XI92CWuxK9ys62RaKl8T2B0H10MWqeGnm+skaw2Ic2oqmDGAVwhX6Kcfn7isQCcY8XlvEbaJ0=</Modulus><Exponent>AQAB</Exponent><P>3vVhG3lBTEtG819wZXfoGHthLXVXJkhlqHc04PwXl7z0Cn13u8ejJljIoNKWOxXPiWBhp4+SL8bjvx9aGYpSRw==</P><Q>yrfnY3hjEoFYcfz/JVbYXt4GXwA+FKYx9pv8yxph/9cIO1F2UD31kjHovZE175sjJOUE9Hc4uSkhkgmggBbb+w==</Q><DP>EgYgLYpl3vcO60nB2lIRLzl6J6SewPeFUFMisTVv6tJZoCd4cHO5GrZ+sZGUl34x4tcpzdPra3VWn/K24+2srQ==</DP><DQ>QmRozBMpn23tYafSiJAg3TEqNQMHUgv8YSBFct95KSlr8MGFVlJ0kyT1bOCaIZdVs0euj8JcOYhXDlVI06qo8Q==</DQ><InverseQ>huxxzFt220B4OhySJnbFmOEKPsQ2hSXCqn6qd5AX5FE/RDNd3iZ6xsX/38deh1p2lha0MbDS7CG0dvYxHxrmRQ==</InverseQ><D>lsD7EbyQ8UXpFI/T31L65JUDoYMWkaXgaUeYSdECamOodBOtyXs8/JhRhiYEtGwMmBNO4rVehJ5sHkMzS1W4nWzuhUg4SzIguTVF9ta8Y2i8V2UDB54IGSYHHuV3OLejNBb07IGMKHe4qdiTaJnoatwbmJEpo4jUOkjHrgej/qE=</D></RSAKeyValue>";
#endif
        
        /// <summary>
        /// ��ϢУ������Կ
        /// ���ɵ�У��������8��byte
        /// </summary>
        private static readonly byte[] KEY24_MACTripleDES ={ 69, 86, 77, 111, 78, 26, 225, 3, 199, 98, 196, 200, 184, 22, 177, 59, 128, 149, 87, 3, 215, 81, 175, 193 };
        
        /// <summary>
        /// ��ñ���Ӳ��ID
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
                        //ֻ��ֵһ��
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
        /// ������֤
        /// ��ʹ�ÿͻ�����ģʽʱ���ͻ��������Ӳ���󶨡�Vender���ṩ�ͻ��������Ȩ��(ʹ��Vender��˽Կǩ���ͻ�������)��
        /// �ͻ��������֮�洢����������ע�������Licence�ȣ���Ҳ�ɴ洢�ڷ������ϡ�
        /// �ͻ����������ʱ��ȡ���������Ļ����룬ʹ�����õ�Vender�ṩ�����������ͬ�Ĺ�Կ��У�飨�����룬��Ȩ�룩��
        /// </summary>
        /// <param name="caCode">ʹ��RAS-1024���ɵ���Ȩ��(������ǩ��)��ת����BASE26�ַ�����ȥ���˷ָ�������14���ַ�����һ��UInt64��</param>
        /// <returns></returns>
        internal static bool LocalCA(string caCode)
        {
            //��caCodeת����Base26��˫UInt32���ֽ����飬ע�⣺caCode��һ�����ַ������޷ָ�����ÿ14���ַ�����һ��UInt64��
            if (String.IsNullOrEmpty(caCode)||
                caCode.Length % Base26.Bit64Base26MaxLength != 0)
            {
                return false;
            }

            int longCnt = caCode.Length / Base26.Bit64Base26MaxLength;

            byte[] signedData = new byte[longCnt * 8];//ǩ������
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
        /// ��ȡ������֤�룬�Թ���������֤��
        /// </summary>
        /// <returns></returns>
        internal static string SCA_GetLocalCACode()
        {
            
            //step 1:����
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
                    //�������8�ֽ�
                    byte[] hash = macdes.ComputeHash(rr);//��ϣ��
                    System.Diagnostics.Debug.Assert(hash.Length == 8);
                    StringBuilder rcsb = new StringBuilder((rr.Length / 8+1)*Base26.Bit64Base26MaxLength+1);

                    byte[] encTmp=new byte[8];
                    //�����ｫ128byte���ĺ�8byteУ���������һ�𣬹���xxxxxxxx-xxxx�ṹ��CACode��
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
        /// ��������֤�ͻ���.
        /// </summary>
        /// <param name="sca">�ͻ��˻��������������RAS-1024���ܣ�Ȼ�����������MACУ�顣</param>
        /// <returns></returns>
        internal static bool ServerAuthenticateClient(string caCode)
        {
            //caCodeǰ14*16���ַ������ģ�ĩ14���ַ�����У���룬�м�һ��-�ָ���
            
            if (caCode.Length != (Base26.Bit64Base26MaxLength * 17)) return false;
            byte[] encMsg = new byte[128];//��������128�ֽ�
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
                    Console.WriteLine("��Ȩ��û��ͨ��У��,��Ч����Ȩ�롣");
                    return false;
                }
            }
            //ͨ����У��
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(SCA_privateRSAKey1024);
                byte[] msgBytes= rsa.Decrypt(encMsg, false);//���ܳɹ�
                string msg = Encoding.UTF8.GetString(msgBytes);
                //Note:���ܳɹ������Զ����Ľ�һ������������msg�����ݿ����Ƿ���ڣ�
                return true;               
            }
          
        }
#endif

    }
}
