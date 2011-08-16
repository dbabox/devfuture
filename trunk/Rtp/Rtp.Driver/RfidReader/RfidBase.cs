using System;
using System.Collections.Generic;
using System.Text;
using Rtp.Driver.CardIO;
using Rtp.Driver;


namespace Rtp.Driver.RfidReader
{
    public abstract class RfidBase:IRfid
    {
        protected static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #region ������������
        public const byte INVALID_SAM_SLOT = 0xFF;

        /// <summary>
        /// COS���������������
        /// </summary>
        public const int PDU_BUFF_LEN = 128;
        /// <summary>
        /// ��Ĵ�С
        /// </summary>
        public const int BLOCK_LEN = 16;
        /// <summary>
        /// MIFARE Ultralight��������64�ֽڣ���512bit��
        /// </summary>
        public const int UL_EEPROM_SIZE = 64;

        /// <summary>
        /// ��ǰSAM����λ
        /// </summary>
        protected byte currentSamSlot = 0x00;

        #endregion

        #region PBOC/ס����/ISO ��׼����
        
        private byte[] GetChallenge_sbuff = new byte[5] { 0x00, 0x84, 0x00, 0x00, 0x00 };

        /// <summary>
        /// ס������׼COS���ȡ�����
        /// </summary>
        /// <param name="le"></param>
        /// <param name="rlen"></param>
        /// <param name="rbuff"></param>
        /// <returns></returns>
        public virtual int CPU_GetChallenge(byte le, ref byte rlen, byte[] rbuff)
        {
            GetChallenge_sbuff[4] = le;
            int rc = CPU_APDU((byte)(GetChallenge_sbuff.Length), GetChallenge_sbuff, ref rlen, rbuff);
            if (rc == 0)
            {
                if (le == 4)
                    System.Diagnostics.Trace.Assert(rlen == 6);
                else
                    System.Diagnostics.Trace.Assert(rlen == 10);
                //������9000��β
                System.Diagnostics.Trace.Assert(rbuff[rlen - 2] == 0x90 && rbuff[rlen - 1] == 0x00);
            }
            return rc;
        }

        private readonly byte[] cmd_SAM_GetChallenge = new byte[5] { 0x00, 0x84, 0x00, 0x00, 0x00 };

        public virtual int SAM_GetChallenge(byte slot, byte le, ref byte rlen, byte[] rbuff)
        {
            cmd_SAM_GetChallenge[4] = le;

            int rc = SAM_APDU(slot, 5, cmd_SAM_GetChallenge, ref rlen, rbuff);
            if (rc == 0)
            {
                if (le == 4)
                    System.Diagnostics.Trace.Assert(rlen == 6);
                else
                    System.Diagnostics.Trace.Assert(rlen == 10);
                //������9000��β
                System.Diagnostics.Trace.Assert(rbuff[rlen - 2] == 0x90 && rbuff[rlen - 1] == 0x00);
            }
            return rc;
        }


        #endregion

        #region CPU��APDU�¼�
        /// <summary>
        /// ����CPU COSָ��󼤷���
        /// </summary>
        public event EventHandler<CpuRequestEventArgs> CpuRequest;
        protected virtual void OnCpuRequest(byte slen, byte[] sbuff)
        {
            if (CpuRequest != null && slen >= 2)
            {
                UInt16 cmd = 0;
                cmd = sbuff[0];
                cmd <<= 8;
                cmd += sbuff[1];         
               
                CpuRequest(this, new CpuRequestEventArgs(cmd,  Utility.ByteArrayToHexStr(sbuff, slen, " ")));

            }
        }

        /// <summary>
        /// �յ�CPU COS��Ӧ�󼤷���
        /// </summary>
        public event EventHandler<CpuResponseEventArgs> CpuResponse;

        protected virtual void OnCpuResponse(byte rlen, byte[] rbuff)
        {
            if (CpuResponse != null && rlen >= 2)
            {
                UInt16 sw = 0;
                sw = rbuff[rlen - 2];
                sw <<= 8;
                sw += rbuff[rlen - 1];
                //TODO:��״̬���������л�ȡ״̬������
              
                CpuResponse(this, new CpuResponseEventArgs(sw, Utility.ByteArrayToHexStr(rbuff, rlen, " ")));
            }
        }
        #endregion

        #region SAM��APDU�¼�
        /// <summary>
        /// ����SAM��COSָ��󼤷���
        /// </summary>
        public event EventHandler<SamRequestEventArgs> SamRequest;
        protected virtual void OnSamRequest(byte slot,byte slen, byte[] sbuff)
        {
            if (SamRequest != null && slen >= 2)
            {
                UInt16 cmd = 0;
                cmd = sbuff[0];
                cmd <<= 8;
                cmd += sbuff[1];
                //TODO:�������������л�ȡ״̬������
               
                SamRequest(this, new SamRequestEventArgs(slot,cmd, Utility.ByteArrayToHexStr(sbuff, slen, " ")));

            }
        }

        /// <summary>
        /// �յ�SAM��COS��Ӧ�󼤷���
        /// </summary>
        public event EventHandler<SamResponseEventArgs> SamResponse;
        protected virtual void OnSamResponse(byte slot,byte rlen, byte[] rbuff)
        {
            if (SamResponse != null && rlen >= 2)
            {
                UInt16 sw = 0;
                sw = rbuff[rlen - 2];
                sw <<= 8;
                sw += rbuff[rlen - 1];
                //TODO:��״̬���������л�ȡ״̬������
                string swDescription = String.Empty;
                SamResponse(this, new SamResponseEventArgs(slot,sw, Utility.ByteArrayToHexStr(rbuff, rlen, " ")));
            }
        }
        #endregion

        #region IRfid ��Ա

        public abstract int Open();
        public abstract int Close();
        public abstract int DeviceReset();
        public abstract bool IsOpened();
        public abstract string Request(out CardPhysicalType cpt);
        public abstract string DeviceVersion();
        #endregion

        #region IDisposable ��Ա

        public virtual void Dispose()
        {
            if (IsOpened())
            {
                Close();
            }
        }
        

        #endregion

        #region ICpuIO ��Ա

        public abstract int CPU_Reset(out byte rlen, byte[] rbuff);
        public abstract int CPU_APDU(byte slen, byte[] sbuff, ref byte rlen, byte[] rbuff);
        #endregion
         
        #region ISamIO ��Ա

        public abstract int SAM_Reset(byte slot, ref byte rlen, byte[] dataBuff);
        public abstract int SAM_SetSlot(byte slot);
        public abstract int SAM_APDU(byte slot, byte slen, byte[] sbuff, ref byte rlen, byte[] rbuff);
        public abstract int SAM_SetPara(byte slot, byte cpupro, byte cpuetu);

        public byte CurrentSamSlot 
        { 
            get 
            { 
                return currentSamSlot; 
            }
            set
            {
                if (currentSamSlot == value) return;
                if (SAM_SetSlot(value) == 0)
                {
                    currentSamSlot = value;
                }
                else
                {
                    currentSamSlot = 0;
                    logger.ErrorFormat("SET SAM Slot to {0} failed.", value);
                }
            }
          
        }

        #endregion

        #region IUltralightIO ��Ա
        public abstract int UL_read(byte addr, byte[] rbuff);
        public abstract int UL_write(byte addr, byte[] wbuff);       
        #endregion
    }
}
