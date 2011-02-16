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
        private const int BUFF_LEN = 128;
        /// <summary>
        /// Ĭ�����������ģ�����󶨵�һ��rfid��
        /// </summary>
        public CommandContext(IRfid rfid_)
        {
            rfid = rfid_;
            gVDic = new Dictionary<string, string>();
            gVDic.Add("CARD_RAND_8BYTE", "00 00 00 00 00 00 00 00 ");
            gVDic.Add("CARD_RAND_4BYTE", "00 00 00 00 ");

            slen = 0;
            rlen = 0;
            sbuff = new byte[BUFF_LEN];
            rbuff = new byte[BUFF_LEN];             
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

        int executeMode=0x10;
        /// <summary>
        /// ִ��ģʽ:0x10=�Զ�ִ�У�ʧ������ֹ��0x20=����ִ�У�ʧ������ֹ�� 0x30=���ϵ�ִ�У�ʧ������ֹ;
        ///          0x11=�Զ�ִ�У�ʧ���Լ�����0x21=����ִ�У�ʧ���Լ����� 0x31=���ϵ�ִ�У�ʧ���Լ���;
        ///          Ĭ��ģʽΪ0x10
        /// </summary>
        public int ExecuteMode
        {
            get { return executeMode; }
            set { 
                executeMode = value;
                isBreakOnFailed = (executeMode & 0x0F) == 0;
            }
        }

        bool isBreakOnFailed = true;

        public bool IsBreakOnFailed
        {
            get { return isBreakOnFailed; }            
        }
       
        



        /// <summary>
        /// ������һ�β����ķ�����
        /// </summary>
        public int rc;


        bool isMacOn = false; 
        /// <summary>
        /// MAC�Զ������Ƿ�����
        /// </summary>
        public bool IsMacOn
        {
            get { return isMacOn; }
            set { isMacOn = value; }
        }
        /// <summary>
        /// ���ͳ���
        /// </summary>
        public byte slen;
        /// <summary>
        /// ���ͻ�����
        /// </summary>
        public byte[] sbuff;
        /// <summary>
        /// ���ճ���
        /// </summary>
        public byte rlen; 
        /// <summary>
        /// ���ջ���������
        /// </summary>
        public byte[] rbuff;

        Dictionary<string, string> gVDic;
        /// <summary>
        /// ȫ�ֱ����ֵ�
        /// </summary>
        public Dictionary<string, string> GVDIC
        {
            get { return gVDic; }
            set { gVDic = value; }
        }

        IRfid rfid;
        /// <summary>
        /// ��ǰʹ�õĶ�����
        /// </summary>
        public IRfid Rfid
        {
            get { return rfid; }
            set { rfid = value; }
        }

        string cmdTarget="CPU";
        /// <summary>
        /// ����Ŀ��,������CPU,SAM 0x##��Ĭ��CPU��������ָ��������ΪCPU��
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
                ReportMessage("WARN:[AppandMac] Ҫ��MAC�������CLA��������ֽ�Ϊ0x04.");
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
                ReportMessage("ERROR:GetChallenge RC={0}", rc);
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

        #region ��Ϣ����
        /// <summary>
        /// ��Ϣ����ί��
        /// </summary>
        ReceiveContxtMessage _recvCtxMsg;
        /// <summary>
        /// ��Ϣ����ί��
        /// </summary>
        public ReceiveContxtMessage RecvCtxMsg
        {
            get { return _recvCtxMsg; }
            set { _recvCtxMsg = value; }
        }

        public void ReportMessage(string message)
        {
            if (_recvCtxMsg != null)
                _recvCtxMsg(message);
            else
                System.Diagnostics.Trace.TraceInformation(message);
        }

        public void ReportMessage(string format,params object[] args)
        {
            if (_recvCtxMsg != null) 
                _recvCtxMsg(String.Format(format, args));
            else 
                System.Diagnostics.Trace.TraceInformation(format, args);

        }
        #endregion


    }
}
