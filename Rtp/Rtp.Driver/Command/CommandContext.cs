/*
 * ���������ġ�
 * 
 * */
using System;
using System.Collections.Generic;
using System.Text;
using Rtp.Driver.CardIO;
using Rtp.Driver.RfidReader;

namespace Rtp.Driver.Command
{
    public class CommandContext
    {
        /// <summary>
        /// Ĭ�����������ģ�����󶨵�һ��rfid��
        /// </summary>
        public CommandContext(IRfid rfid_)
        {
            rfid = rfid_;
            gVDic = new Dictionary<string, string>();
            gVDic.Add("CARD_RAND_8BYTE", "00 00 00 00 00 00 00 00 ");
            gVDic.Add("CARD_RAND_4BYTE", "00 00 00 00 ");
            gVDic.Add("MAC_KEY", "FF FF FF FF FF FF FF FF ");//FM1208-43 JSB CPU���ĳ�ʼ��ԿΪ16��F��
            gVDic.Add("CPU_CCK", "FF FF FF FF FF FF FF FF ");//FM1208-43 JSB CPU���ĳ�ʼ������ԿΪ16��F��

            slen = 0;
            rlen = 0;
            sbuff = new byte[128];
            rbuff = new byte[128];

            //����COS�����ļ�
            //cosIO = new FileMapCosIO().ReadCosFile("default.cos");
        }

        /// <summary>
        /// ��Ϲ��캯����
        /// </summary>
        /// <param name="sbuff_"></param>
        /// <param name="rbuff_"></param>
        /// <param name="cosIO_"></param>
        /// <param name="gVDic_"></param>
        public CommandContext(byte[] sbuff_, byte[] rbuff_, Dictionary<string, string> gVDic_, IRfid rfid_)
        {
            sbuff = sbuff_;
            rbuff = rbuff_;           
            gVDic = gVDic_;
            rfid = rfid_;
        }


        #region ���������ĳ�Ա

        public int rc;//������һ�β����ķ�����


        bool isMacOn = false; 

        public bool IsMacOn
        {
            get { return isMacOn; }
            set { isMacOn = value; }
        }

        public byte slen;        
        public byte[] sbuff;
        public byte rlen;       
        public byte[] rbuff;

       
 

        Dictionary<string, string> gVDic;

        public Dictionary<string, string> GVDIC
        {
            get { return gVDic; }
            set { gVDic = value; }
        }

        IRfid rfid;

        public IRfid Rfid
        {
            get { return rfid; }
            set { rfid = value; }
        }

        string cmdTarget="CPU";
        /// <summary>
        /// ����Ŀ��,������CPU,SAM ##��Ĭ��CPU��������ָ��������ΪCPU��
        /// </summary>
        public string CmdTarget
        {
            get { return cmdTarget; }
            set { cmdTarget = value; }
        }


        #endregion


        /// <summary>
        /// ��CPU��/SAM��ָ���MAC���Զ�����MAC_KEY�ĳ���ѡ��DES����3DES���㡣
        /// </summary>
        /// <param name="rf"></param>
        /// <returns></returns>
        public bool AppandMac()
        {
            int rc = 0;
            byte cla = sbuff[0];
            if ((cla & 0x0F) != 0x04)
            {
                Console.WriteLine("WARN:[AppandMac] Ҫ��MAC�������CLA��������ֽ�Ϊ0x04.");
                return true;
            }
            byte lc = sbuff[4];
            if (lc < 4) throw new ArgumentException(String.Format("{0}����LC����ȷ,LC�������򳤶ȣ������MAC���ȣ���˱�����ڵ���4.", Utility.ByteArrayToHexStr(sbuff, slen)));
            byte[] data = new byte[5 + lc - 4];
            Array.Copy(sbuff, data, data.Length);//������MAC�Ĳ���

            byte[] init = new byte[8];//8�ֽ������

            if (CmdTarget == "CPU")
            {
                rc = rfid.CPU_GetChallenge(8, ref rlen, rbuff);
            }
            else //ȡSAM���������
            {
                rc = rfid.SAM_GetChallenge(rfid.CurrentSamSlot, 8, ref rlen, rbuff);
            }
            
            if (rc != 0)
            {
                Console.WriteLine("ERROR:GetChallenge RC={0}", rc);
                return false;
            }
            Array.Copy(rbuff, init, 8);
            byte[] MAC_KEY_BUF = new byte[16];
            int mac_key_len = Utility.HexStrToByteArray(GVDIC["MAC_KEY"], ref MAC_KEY_BUF);
            if (mac_key_len == 16)
            {
                //3DES����MAC
                Utility.PBOC_GetKey16MAC(data, MAC_KEY_BUF, init, ref rbuff);//TODO:����ʹ�õ���PC����MAC��ʵ�ʿ���ʹ��SAM������MAC
            }
            else
            {
                //DES����MAC
                byte[] MAC_KEY = new byte[8];
                Array.Copy(MAC_KEY_BUF, MAC_KEY, 8);
                Utility.PBOC_GetKey8MAC(data, MAC_KEY, init, ref rbuff);//TODO:����ʹ�õ���PC����MAC��ʵ�ʿ���ʹ��SAM������MAC
            }
            //�ж�sbuff���Ƿ���le
            if ((5 + lc - 4) == slen)
            {
                //������le
                Array.Copy(rbuff, 0, sbuff, slen, 4);
            }
            else if ((5 + lc - 4 + 1) == slen)
            {
                //����le
                sbuff[slen + 3] = sbuff[slen - 1];
                Array.Copy(rbuff, 0, sbuff, slen, 4);
            }
            else
            {
                throw new ArgumentException(String.Format("{0}�����ʽ����ȷ������LC��ֵ��LE�Ƿ���ڡ�", Utility.ByteArrayToHexStr(sbuff, slen)));
            }
            slen += 4;//Note:�޸��˴���������ĳ���
            return true;
        }

      

    }
}
