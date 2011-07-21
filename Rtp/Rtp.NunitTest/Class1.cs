using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Rtp.Driver;

namespace Rtp.NunitTest
{
    [TestFixture] 
    public class TestUtility
    {
        string key8str = "01 02 03 04 05 06 07 08 ";
        string key16str = "01020304050607081112131415161718";
        byte[] rbuff = new byte[128];
        byte[] sbuff = new byte[128];
        byte[] key8=new byte[8];
        byte[] key16=new byte[16];


        [SetUp]
        public void Init()
        {
            Utility.HexStrToByteArray(key8str, ref key8);
            Utility.HexStrToByteArray(key16str, ref key16);
            Console.WriteLine("���Կ�ʼ:" + DateTime.Now.ToString());
            System.Diagnostics.Trace.Listeners.Add(new System.Diagnostics.ConsoleTraceListener(false));

        }

        [TearDown]
        public void TestDown()
        {

            System.Diagnostics.Trace.Listeners.Clear();
            Console.WriteLine("���Խ���:" + DateTime.Now.ToString());
        }
        [Test]
        public void TestPBOC_DES()
        {
            Console.WriteLine("����PBOC DES KEY8 ���ܽ���");
            byte[] enc=new byte[128];
            int enc_len = 0;
            
            string datastr0 = " 01 02 03 04 05 06 07 ";
            byte[] data0 = new byte[7];
            Utility.HexStrToByteArray(datastr0, ref data0);

            enc_len=Utility.PBOC_DesEnc_Key8(data0, key8, ref enc);
            Console.WriteLine("Key8={0}", key8str);
            Console.WriteLine("data0={0}", datastr0);
            Console.WriteLine("enc={0}", Utility.ByteArrayToHexStr(enc,enc_len));

            byte[] to_dec=new byte[enc_len];
            Array.Copy(enc,to_dec,enc_len);

            int dec_len=Utility.PBOC_DesDec_Key8(to_dec, key8, ref enc);
            Console.WriteLine("dec={0}", Utility.ByteArrayToHexStr(enc, dec_len));

            Assert.IsTrue(enc[0] == 0x07);
            for (int i = 1; i < 8; i++)
            {
                Assert.IsTrue(enc[i] == data0[i-1]);
            }

            //==============================
            Console.WriteLine("����PBOC DES KEY16 ���ܽ���");

           
            string data1str = "21 22 23 24 25 26 27 28 29 ";
            byte[] data1 = new byte[9];
            Utility.HexStrToByteArray(data1str, ref data1);
            

            enc_len=Utility.PBOC_DesEnc_Key16(data1, key16, ref enc);
            Console.WriteLine("data1={0}", data1str);
            Console.WriteLine("key16={0}", key16str);
            Console.WriteLine("enc={0}", Utility.ByteArrayToHexStr(enc, enc_len));

            byte[] to_dec2 = new byte[enc_len];
            Array.Copy(enc, to_dec2, enc_len);
            dec_len = Utility.PBOC_DesDec_Key16(to_dec2, key16, ref enc);

            Console.WriteLine("dec={0}", Utility.ByteArrayToHexStr(enc, dec_len));
            
        }

        [Test]
        public void Test_DES()
        {
            string data8str = " 31 32 33 34 35 36 37 38";
            byte[] data8 = new byte[8];
            Utility.HexStrToByteArray(data8str, ref data8);

            Utility.DesBlockEncrypt(key8, data8, System.Security.Cryptography.CipherMode.ECB, ref rbuff);
            Console.WriteLine("data8={0}", data8str);
            Console.WriteLine("DES ENC={0}", Utility.ByteArrayToHexStr(rbuff, data8.Length));
            Array.Copy(rbuff,data8,data8.Length);
            Utility.DesBlockDecrypt(data8, System.Security.Cryptography.CipherMode.ECB, System.Security.Cryptography.PaddingMode.None, key8, ref rbuff);
            Console.WriteLine("DES DEC={0}", Utility.ByteArrayToHexStr(rbuff, data8.Length));


            string data16str = " 31 32 33 34 35 36 37 38 41 42 43 44 45 46 47 48";
            byte[] data16 = new byte[16];
            Utility.HexStrToByteArray(data16str, ref data16);
            Utility.TripDesBlockEncrypt(key16, data16, System.Security.Cryptography.CipherMode.ECB, ref rbuff);
            Console.WriteLine("Data16={0}", data16str);
            Console.WriteLine("3DES ENC={0}", Utility.ByteArrayToHexStr(rbuff, data16.Length));

            Array.Copy(rbuff, data16, data16.Length);
            Utility.TripleDESDecrypt(data16, key16, System.Security.Cryptography.CipherMode.ECB, System.Security.Cryptography.PaddingMode.None,ref rbuff);
            Console.WriteLine("3DES DEC={0}", Utility.ByteArrayToHexStr(rbuff, data16.Length));

        }

        [Test]
        public void Test_MAC()
        {
            Console.WriteLine("Test PBOC Key16 3DES MAC");
            string datastr = "000000F00200000000000120110130165711800000000000";
            int rlen=Utility.HexStrToByteArray(datastr, ref rbuff);
            byte[] data = new byte[rlen];
            Array.Copy(rbuff, data, rlen);
            byte[] init=new byte[8];
            Array.Clear(rbuff, 0, rbuff.Length);
            Utility.PBOC_GetKey16MAC(data, key16, init, ref rbuff);
            Console.WriteLine("Key16={0}\ndata={1}\ninit8={2}", key16str, datastr, 0);
            Console.WriteLine("MAC={0}", Utility.ByteArrayToHexStr(rbuff, 16));

        }

        [Test]
        public void Test_BCD()
        {
            Console.WriteLine("����BCD�������");
            string dtnow = DateTime.Now.ToString("yyyyMMddHHmmss");
            Console.WriteLine("DateTime Now String :{0}",dtnow);
            byte[] dtnow_bcd= BCDEncoding.Str2BCD(dtnow); //BCD����

            string result = Utility.ByteArrayToHexStr(dtnow_bcd);
            Console.WriteLine("BCD encoded:{0}", result);
            Assert.IsTrue(String.Compare(result, dtnow, StringComparison.Ordinal) == 0);

            string dstr = BCDEncoding.BCD2Str(dtnow_bcd, 0, dtnow.Length);
            Console.WriteLine("BCD2STR :{0}", dstr);
            Assert.IsTrue(String.Compare(dstr, dtnow, StringComparison.Ordinal) == 0);   
        }

        [Test]
        public void Test_Hex_Str()
        {
            byte[] tb = new byte[] {0x01,0x00,0x90,0x00};
            Console.WriteLine("ԭʼ���� Byte[0]={0,2:X2},Byte[1]={1,2:X2} Byte[2]={2,2:X2} Byte[3]={3,2:X2}", tb[0], tb[1], tb[2], tb[3]);
            string hexstr=Utility.ByteArrayToHexStr(tb);
            Console.WriteLine("HEXSTR:{0}",hexstr);
            byte[] tb2 = new byte[4];
            Utility.HexStrToByteArray(hexstr, ref tb2);
            Console.WriteLine("��ԭ���� Byte[0]={0,2:X2},Byte[1]={1,2:X2} Byte[2]={2,2:X2} Byte[3]={3,2:X2}", tb2[0], tb2[1], tb2[2], tb2[3]);

            Console.WriteLine("2<<3={0}", 2 << 3);
        }
        [Test]
        public void TestDate745()
        {
            int tt = 16;
            Console.WriteLine("tt<<2 ={0}", tt << 2);
            Console.WriteLine("tt>>2 ={0}", tt>> 2);

            byte[] rc=new byte[2];
            byte year=10;
            byte month=9;
            byte day=23;
            Utility.ConvertDateTo745b(year, month, day, ref rc);
            Console.WriteLine("ConvertDateTo745b :{2}, {3}, {4},={0,2:X2} {1,2:X2}", rc[0], rc[1],year,month,day);

            byte oy=0;byte om=0;byte od=0;
            Utility.Convert745bToDate(rc, out oy, out om, out od);
            Console.WriteLine("Convert745bToDate:out year={0},out month={1},out day={2}", oy, om, od);


            Utility.ConvertDateTo645b(year, month, day, ref rc);
            Console.WriteLine("ConvertDateTo645b :{2}, {3}, {4},={0,2:X2} {1,2:X2}", rc[0], rc[1], year, month, day);

            Utility.Convert645bToDate(rc, out oy, out om, out od);
            Console.WriteLine("Convert645bToDate:out year={0},out month={1},out day={2}", oy, om, od);

            UInt16 u16 = 0;

            u16 = Utility.ConvertDateTo745bU16(year, month, day);
            Console.WriteLine("ConvertDateTo745bU16:{0}-{1}-{2} ==>{3,4:X4}", year, month, day, u16);

            u16 = Utility.ConvertDateTo645bU16(year, month, day);
            Console.WriteLine("ConvertDateTo645bU16:{0}-{1}-{2} ==>{3,4:X4}", year, month, day, u16);

            Console.WriteLine("������������Ʊ��1<< ConvertDateTo645bU16:{0}-{1}-{2} ==>{3,4:X4}", year, month, day, (u16<<1));

          




            Console.WriteLine("-------------");
            int t32 = 32;
            byte[] b32 = BitConverter.GetBytes(t32);
            for (int i = b32.Length-1; i >=0; i--)
            {
                Console.Write("{0,2:X2} ", b32[i]);
            }
            Console.WriteLine("-------------");
           
            string idstr = "21058119800909003X";
           
            idstr = idstr.Replace('X', 'A');
            Console.WriteLine("���֤��:{0}ת16�����룺", idstr);
            byte[] idBytes=new byte[9];
            Utility.HexStrToByteArray(idstr, ref idBytes);

            for (int i =0; i<idBytes.Length; i++)
            {
                Console.Write("{0,2:X2} ", idBytes[i]);
            }

            Console.WriteLine("-------------");
        }


    }
}
