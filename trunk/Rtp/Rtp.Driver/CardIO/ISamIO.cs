/*
 * �Ӵ�ʽCPU����SAM����д�����ӿڡ�
 * 
 * 
 * */
using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.CardIO
{
    public class SamResponseEventArgs : EventArgs
    {
     
        private UInt16 sw;              //״̬��
        private byte slot;              //���۱�ʶ

        public byte Slot
        {
            get { return slot; }
            set { slot = value; }
        }



        public UInt16 Sw
        {
            get { return sw; }
            set { sw = value; }
        }
       
        public SamResponseEventArgs(byte slot_, UInt16 sw_, string responseStr_)
        {
            slot = slot_;
            sw = sw_;
           
            responseString = responseStr_;
            
        }

        public override string ToString()
        {
            return String.Format("SAM{0,2:X2}:{1}",slot, responseString);
        }

        private string responseString;

        public string ResponseString
        {
            get { return responseString; }
            set { responseString = value; }
        }
    }

    public class SamRequestEventArgs : EventArgs
    {
   
        private UInt16 cmd;
        private string cmdstr;
        private byte slot;


        public string Cmdstr
        {
            get { return cmdstr; }
            set { cmdstr = value; }
        }

        public UInt16 Cmd
        {
            get { return cmd; }
            set { cmd = value; }
        }
        
        public SamRequestEventArgs(byte slot_, UInt16 cmd_, string cmdstr_)
        {
            slot = slot_;
            cmd = cmd_;            
            cmdstr = cmdstr_;           
        }

        public override string ToString()
        {
            return String.Format("SAM{0,2:X2}:{1}", slot, cmdstr);
        }
    }

    public interface ISamIO
    {
        

        #region �Ӵ�ʽCPU������
        byte CurrentSamSlot { get;set;}

        int SAM_GetChallenge(byte slot, byte le, ref byte rlen, byte[] rbuff);

        /// <summary>
        /// SAM����λ
        /// </summary>
        /// <param name="rlen"></param>
        /// <param name="dataBuff"></param>
        /// <returns></returns>
        int SAM_Reset(byte slot, ref byte rlen, byte[] dataBuff);
        /// <summary>
        /// ѡ����
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        int SAM_SetSlot(byte slot);
        /// <summary>
        /// ִ��SAM��COS����
        /// </summary>
        /// <param name="slen"></param>
        /// <param name="sbuff"></param>
        /// <param name="rlen"></param>
        /// <param name="rbuff"></param>
        /// <returns></returns>
        int SAM_APDU(byte slot, byte slen, byte[] sbuff, ref byte rlen, byte[] rbuff);
        /// <summary>
        /// ����SAM��ͨ�Ų���
        /// </summary>
        /// <param name="slot">��ۺ�</param>
        /// <param name="cpupro">Э������</param>
        /// <param name="cpuetu">������</param>
        /// <returns></returns>
        int SAM_SetPara(byte slot, byte cpupro, byte cpuetu);

        #endregion
    }
}
