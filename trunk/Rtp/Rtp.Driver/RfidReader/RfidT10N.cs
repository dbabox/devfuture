/*
 * 
 * NMTrf32.cs 
 * dcard T10N USB读卡器封装
 * 
 * */
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Rtp.Driver.RfidReader
{
    public class RfidT10N : RfidBase
    {
        private const int USB_PORT = 100;
        private const uint BAUD = 115200;
        private readonly ushort BEEP_MSEC = 5;              //蜂鸣时长 2m 
        /// <summary>
        /// COS指令执行超时时间，至少10ms
        /// </summary>
        private const byte COS_CMD_TIME_OUT = 25;
        private const byte FG = 56;

        private int icdev;

        public int Icdev
        {
            get { return icdev; }
           
        }

        private int port;
        /// <summary>
        /// 串口端口号,USB无效
        /// </summary>
        public int Port
        {
            get { return port; }
            set { port = value; }
        }
        private uint baud;
        /// <summary>
        /// 串口波特率
        /// </summary>
        public uint Baud
        {
            get { return baud; }
            set { baud = value; }
        }


        private byte samBaudrate;

        /// <summary>
        /// 1表示9600，2表示14400，3表示19200，4表示38400，5表示57600，6表示115200
        /// </summary>
        public byte SamBaudrate
        {
            get { return samBaudrate; }
            set { samBaudrate = value; }
        }

        private byte volt;

        /// <summary>
        /// 2表示3.3V, 3表示5.0V
        /// </summary>
        public byte Volt
        {
            get { return volt; }
            set { volt = value; }
        }

        private byte samProtocol;
        /// <summary>
        /// 卡片协议（0表示T = 0, 1表示 T = 1）
        /// </summary>
        public byte SamProtocol
        {
            get { return samProtocol; }
            set { samProtocol = value; }
        }

        public RfidT10N()
        {
            port = USB_PORT;
            baud = 0;
            samBaudrate = 1;
            volt = 2;
            samProtocol = 0;
        }


        /// <summary>
        /// [in]  port  取值为0～19时，表示串口1～20；为100时，表示USB口通讯，此时波特率无效。 
        /// [in]  baud  通讯波特率，取值为9600～115200。 
        /// </summary>
        /// <returns></returns>
        public override int Open()
        {
            icdev = NMTrf32.dc_init(port, baud);
            if (icdev < 0)
            {
                Trace.TraceWarning("dc_init failed:port={0},baud={1}",port,baud);
            }
            else
            {
                NMTrf32.dc_beep(icdev, BEEP_MSEC);
                byte[] buff=new byte[4];
                NMTrf32.dc_getver(icdev, buff);
                Trace.TraceInformation("SYS>> Device Version:{0}.",Utility.ByteArrayToHexStr(buff));
            }
            return icdev;
        }

        public override int Close()
        {
            NMTrf32.dc_beep(icdev, BEEP_MSEC);
            int rc = NMTrf32.dc_exit(icdev);
            Trace.TraceInformation("*****************dc_exit:icdev={0},rc={1}**************", icdev, rc);
            if (rc == 0) icdev =0;//成功关闭，则置设备描述符为0.
            return rc;
        }

        public override int DeviceReset()
        {
            int rc = NMTrf32.dc_reset(icdev);
            if (rc == 0)
                Trace.TraceInformation("射频复位成功！");
            else
                Trace.TraceError("射频复位失败！");
            return rc;
        }

        public override bool IsOpened()
        {
            return icdev > 0;
        }

        /// <summary>
        /// 根据Request函数获得的特征值判断卡类型。
        /// </summary>
        /// <param name="tagType"></param>
        /// <returns></returns>
        internal CardPhysicalType GetCardType(int tagType)
        {
            Trace.TraceInformation("card tag type:{0}", tagType);
            switch (tagType)
            {
                case Trf32Constants.RQ_TRAIT_ULTRALIGHT: return CardPhysicalType.UltraLight;
                case Trf32Constants.RQ_TRAIT_MIFARE_PRO: return CardPhysicalType.CPU_TypeA;
                case Trf32Constants.RQ_TRAIT_MIFARE1: return CardPhysicalType.MifareOne;
                default:
                    return CardPhysicalType.Unknown;
            }
        }


        public override string Request(out CardPhysicalType cpt)
        {
            cpt = CardPhysicalType.Unknown;
            ushort tagType = 0;
            bool isTwoTried = false;
            uint snr = 0;
        TWOTRY:
            //请求
            int rc = NMTrf32.dc_request(icdev, Trf32Constants.SELECT_MODE_IDLE, ref tagType);//这里可能失败，但出现tagType被赋值的情况
            if (rc == 0)
            {
                cpt = GetCardType(tagType);                
                Trace.TraceInformation("dc_request OK! Card Type={0}", cpt);
                
                rc = NMTrf32.dc_anticoll(icdev, 0, ref snr); //防冲突
                if (rc == 0)
                {
                    Trace.TraceInformation("dc_anticoll OK! snr={0}", snr);
                    byte size = 0;
                    rc = NMTrf32.dc_select(icdev, snr, ref size); //选择
                    if (rc == 0)
                    {
                        Trace.TraceInformation("dc_select OK! size={0}", size);
                        return snr.ToString();
                    }
                    else
                    {
                        Trace.TraceInformation("dc_select failed! rc={0}", rc);
                    }
                }
            }
            else
            {
                Trace.TraceWarning("dc_request failed.rc={0},tagType={1}", rc, tagType);
                if (tagType != 0 && isTwoTried == false)
                {
                    isTwoTried = true;
                    if (DeviceReset() == 0) goto TWOTRY;
                }
            }
            return String.Empty;
        }

        public override string DeviceVersion()
        {
            byte[] buff = new byte[4];
            NMTrf32.dc_getver(icdev, buff);
            return Utility.ByteArrayToHexStr(buff, 4, "");
        }

        public override int CPU_Reset(out byte rlen, byte[] rbuff)
        {
            rlen = 0;
            return NMTrf32.dc_pro_reset(icdev, ref rlen, rbuff);
        }

        public override int CPU_APDU(byte slen, byte[] sbuff, ref byte rlen, byte[] rbuff)
        {
            if (slen == 0 || sbuff == null || sbuff.Length == 0)
            {
                return -1;
            }

            Trace.TraceInformation("dc_pro_commandlink:slen={0},sbuff={1}", slen, Utility.ByteArrayToHexStr(sbuff, slen));
            OnCpuRequest(slen, sbuff);
            int rc = NMTrf32.dc_pro_commandlink(icdev, slen, sbuff, ref rlen, rbuff, COS_CMD_TIME_OUT,FG);
            if (rc == 0)
            {
                OnCpuResponse(rlen, rbuff);
            }
            else
            {
                Trace.TraceInformation("dc_pro_commandlink failed:rc={0}", rc);
            }
            return rc;
        }

       

        /// </summary>
        /// <param name="slot"></param>
        /// <param name="rlen"></param>
        /// <param name="dataBuff"></param>
        /// <returns></returns>
        public override int SAM_Reset(byte slot, ref byte rlen, byte[] rbuff)
        {
            //currentSamSlot只能是:（0x0c表示附卡座,0x0d表示为SAM1,0x0e表示为SAM2,0x0f表示SAM3,0x10表示SAM4）
            //而这里的参数slot:（1表示SIM1,2表示SIM2,3表示大卡座,4表示SIM3,5表示SIM4）
            if (currentSamSlot != slot) SAM_SetSlot(slot);
            /// <summary>
            /// 说明:
            ///CPU卡上电复位函数，复位后自动判断卡片协议。 
            ///参数:
            ///[in]  icdev  通讯设备标识符。  
            ///[in]  cardtype  要操作的卡座号。（1表示SIM1,2表示SIM2,3表示大卡座,4表示SIM3,5表示SIM4）。 //注意和dc_setcpu参数值意义的不同。  
            ///[in]  baudrate  1表示9600，2表示14400，3表示19200，4表示38400，5表示57600，6表示115200。  
            ///[in]  Volt  2表示3.3V, 3表示5.0V。  
            ///[out]  rlen  返回复位信息的长度。  
            ///[out]  databuffer  存放返回的复位信息。  
            ///[in]  protocol  返回卡片协议（0表示T = 0, 1表示 T = 1）。  
            slot -= 12;
            int rc = NMTrf32.dc_cpureset(icdev, slot,samBaudrate, volt, ref rlen, rbuff, ref samProtocol);
            if (rc == 0)
            {
                Trace.TraceInformation("dc_cpureset success: rlen={0},databuff={1}",
                    rlen, Utility.ByteArrayToHexStr(rbuff, rlen));
                OnCpuResponse(rlen, rbuff);
            }
            else
            {
                Trace.TraceError("dc_cpureset failed: rc={0}", rc.ToString("X"));
            }
            return rc;
        }

        /// <summary>
        /// 0x0c表示附卡座,0x0d表示为SAM1,0x0e表示为SAM2,0x0f表示SAM3,0x10表示SAM4
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        public override int SAM_SetSlot(byte slot)
        {
            int rc = NMTrf32.dc_setcpu(icdev, slot);
            if (rc == 0)
            {
                currentSamSlot = slot;
            }
            else
            {
                currentSamSlot = INVALID_SAM_SLOT;
                Trace.TraceError("dc_setcpu failed: rc={0}", rc.ToString("X"));
            }
            return rc;
        }

        public override int SAM_APDU(byte slot, byte slen, byte[] sbuff, ref byte rlen, byte[] rbuff)
        {
            if (currentSamSlot != slot) SAM_SetSlot(slot);
            OnSamRequest(slot, slen, sbuff);
             
            int rc = NMTrf32.dc_cpuapdu(icdev,slot, slen, sbuff, ref rlen, rbuff,samProtocol);
            if (rc == 0)
            {
                OnSamResponse(slot, rlen, rbuff);
            }
            else
            {
                Trace.TraceError("dc_cpuapdu device failed:rc={0}", rc.ToString("X"));
            }
            return rc;
        }

        /// <summary>
        /// 设置协议类型,波特率
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="cpupro"></param>
        /// <param name="cpuetu">波特率</param>
        /// <returns></returns>
        public override int SAM_SetPara(byte slot, byte cpupro, byte cpuetu)
        {
            Trace.TraceWarning("T10N not support set cpuprotocal.");
            if (currentSamSlot != slot) SAM_SetSlot(slot);
            byte rlen=0;
            byte[] rbuff=new byte[64];
            slot -= 12;
            int rc=NMTrf32.dc_cpureset(icdev,slot, cpuetu, volt, ref rlen, rbuff, ref samProtocol);
            if (rc == 0)
            {
                samBaudrate = cpuetu;
            }
            Trace.TraceInformation("T10N SAM_SetPara:RC={0}, RBUFF={1},samProtocol={2}",rc, Utility.ByteArrayToHexStr(rbuff, rlen), samProtocol);
            return rc;
        }

        #region IUltralightIO
        public override int UL_read(byte addr, byte[] rbuff)
        {
            Trace.TraceInformation("读取Ultralight卡,addr={0}.", addr);
            int rc = NMTrf32.dc_read(icdev, addr, rbuff);
            Trace.TraceInformation("读取Ultralight卡,addr={0}.RBUFF={1},rc={2}", addr, Utility.ByteArrayToHexStr(rbuff, 4),rc);
            return rc;
        }

        public override int UL_write(byte addr, byte[] wbuff)
        {
            Trace.TraceInformation("读取Ultralight卡,addr={0}.", addr);
            int rc = NMTrf32.dc_write(icdev, addr, wbuff);
            Trace.TraceInformation("读取Ultralight卡,addr={0}.WBUFF={1},rc={2}", addr, Utility.ByteArrayToHexStr(wbuff, 4), rc);
            return rc;
        }
        #endregion
    }
}
