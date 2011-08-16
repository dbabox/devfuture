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
        #region 公共常量定义
        public const byte INVALID_SAM_SLOT = 0xFF;

        /// <summary>
        /// COS交换命令缓冲区长度
        /// </summary>
        public const int PDU_BUFF_LEN = 128;
        /// <summary>
        /// 块的大小
        /// </summary>
        public const int BLOCK_LEN = 16;
        /// <summary>
        /// MIFARE Ultralight卡容量，64字节，即512bit。
        /// </summary>
        public const int UL_EEPROM_SIZE = 64;

        /// <summary>
        /// 当前SAM卡槽位
        /// </summary>
        protected byte currentSamSlot = 0x00;

        #endregion

        #region PBOC/住建部/ISO 标准方法
        
        private byte[] GetChallenge_sbuff = new byte[5] { 0x00, 0x84, 0x00, 0x00, 0x00 };

        /// <summary>
        /// 住建部标准COS命令：取随机数
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
                //必须以9000结尾
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
                //必须以9000结尾
                System.Diagnostics.Trace.Assert(rbuff[rlen - 2] == 0x90 && rbuff[rlen - 1] == 0x00);
            }
            return rc;
        }


        #endregion

        #region CPU卡APDU事件
        /// <summary>
        /// 发出CPU COS指令后激发。
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
        /// 收到CPU COS响应后激发。
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
                //TODO:从状态字描述表中获取状态字描述
              
                CpuResponse(this, new CpuResponseEventArgs(sw, Utility.ByteArrayToHexStr(rbuff, rlen, " ")));
            }
        }
        #endregion

        #region SAM卡APDU事件
        /// <summary>
        /// 发出SAM卡COS指令后激发。
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
                //TODO:从命令描述表中获取状态字描述
               
                SamRequest(this, new SamRequestEventArgs(slot,cmd, Utility.ByteArrayToHexStr(sbuff, slen, " ")));

            }
        }

        /// <summary>
        /// 收到SAM卡COS响应后激发。
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
                //TODO:从状态字描述表中获取状态字描述
                string swDescription = String.Empty;
                SamResponse(this, new SamResponseEventArgs(slot,sw, Utility.ByteArrayToHexStr(rbuff, rlen, " ")));
            }
        }
        #endregion

        #region IRfid 成员

        public abstract int Open();
        public abstract int Close();
        public abstract int DeviceReset();
        public abstract bool IsOpened();
        public abstract string Request(out CardPhysicalType cpt);
        public abstract string DeviceVersion();
        #endregion

        #region IDisposable 成员

        public virtual void Dispose()
        {
            if (IsOpened())
            {
                Close();
            }
        }
        

        #endregion

        #region ICpuIO 成员

        public abstract int CPU_Reset(out byte rlen, byte[] rbuff);
        public abstract int CPU_APDU(byte slen, byte[] sbuff, ref byte rlen, byte[] rbuff);
        #endregion
         
        #region ISamIO 成员

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

        #region IUltralightIO 成员
        public abstract int UL_read(byte addr, byte[] rbuff);
        public abstract int UL_write(byte addr, byte[] wbuff);       
        #endregion
    }
}
