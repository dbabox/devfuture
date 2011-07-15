using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Rtp.Driver.RfidReader
{
    public class RfidT6U : RfidBase
    {
        private IntPtr icdev = System.IntPtr.Zero;          //设备描述符
        private const Int16 USB_PORT = 100;
        private readonly byte BEEP_MSEC = 2;              //蜂鸣时长 2m


        /// <summary>
        /// 读卡器设备描述符.只读.
        /// </summary>
        public IntPtr Icdev
        {
            get { return icdev; }
        }

        public override int Open()
        {
            Trace.Assert(icdev == System.IntPtr.Zero);
            icdev = NMDcic32.IC_InitCommAdvanced(USB_PORT);
            if (icdev.ToInt32() > 0)
            {
                NMDcic32.IC_DevBeep(icdev, BEEP_MSEC);
                byte[] ver = new byte[10];
                NMDcic32.IC_ReadVer(icdev, ver); //读出来版本号
                Trace.TraceInformation("SYS>> Device Version:{0}.", Utility.ByteArrayToHexStr(ver));
            }
            else
            {
                Trace.TraceWarning("IC_InitComm failed!");
                icdev = System.IntPtr.Zero;
            }
            return icdev.ToInt32();

        }

        public override int Close()
        {
            NMDcic32.IC_DevBeep(icdev, BEEP_MSEC);
            short rc = NMDcic32.IC_ExitComm(icdev);
            Trace.TraceInformation("*****************IC_ExitComm:icdev={0},rc={1}**************", icdev.ToString(), rc);
            if (rc == 0) icdev = System.IntPtr.Zero;//成功关闭，则置设备描述符为0.
            return rc;
        }

        public override int DeviceReset()
        {
            //设备不支持
            throw new NotImplementedException("设备不支持");
        }

        public override bool IsOpened()
        {
            return icdev != System.IntPtr.Zero;
        }

        public override string Request(out CardPhysicalType cpt)
        {
            throw new NotImplementedException("设备不支持非接触式卡");
        }

        public override string DeviceVersion()
        {
            byte[] ver = new byte[10];
            if (NMDcic32.IC_ReadVer(icdev, ver)==0)
            {
                return Utility.ByteArrayToHexStr(ver, 10, "");
            }
            else
            {
                return "Get Version Error.";
            }
        }

        public override int CPU_Reset(out byte rlen, byte[] rbuff)
        {
            throw new NotImplementedException("设备不支持非接触式CPU卡.");
        }

        public override int CPU_APDU(byte slen, byte[] sbuff, ref byte rlen, byte[] rbuff)
        {
            throw new NotImplementedException("设备不支持非接触式CPU卡.");
        }

        public override int SAM_Reset(byte slot, ref byte rlen, byte[] rbuff)
        {
            if (currentSamSlot != slot) SAM_SetSlot(slot);
            short rc = NMDcic32.IC_CheckCard(icdev);
            if (rc > 0)
            {
                Trace.TraceInformation("Card type test ok:{0}", rc);
            }
            else
            {
                Trace.TraceError("Card type test failed");
                return -1;
            }
            //对CPU卡进行Reset
            rc = NMDcic32.IC_CpuReset(icdev, ref rlen, rbuff);
            if (rc == 0)
            {
                Trace.TraceInformation("IC_CpuReset success: rlen={0},rbuff={1}",
                    rlen, Utility.ByteArrayToHexStr(rbuff, rlen));
                OnSamResponse(slot, rlen, rbuff);
            }
            else
            {
                Trace.TraceError("dc_cpureset failed: rc={0}", rc.ToString("X"));
            }
            return rc;
             
        }

        public override int SAM_SetSlot(byte slot)
        {
            short rc = NMDcic32.IC_InitType(icdev, slot);
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
            short rc = NMDcic32.IC_CpuApdu(icdev, slen, sbuff, ref rlen, rbuff);
            if (rc == 0)
            {
                OnSamResponse(slot, rlen, rbuff);
            }
            else
            {
                Trace.TraceError("IC_CpuApdu device failed:rc={0}", rc.ToString("X"));
            }
            return rc;
        }

        public override int SAM_SetPara(byte slot, byte cpupro, byte cpuetu)
        {
            if (currentSamSlot != slot) SAM_SetSlot(slot);
            short rc = NMDcic32.IC_SetCpuPara(icdev, slot, cpupro, cpuetu);
            if (rc == 0)
            {
                Trace.TraceInformation("IC_SetCpuPara success:cputype={0},cpupro={1},cpuetu={2}", slot, cpupro, cpuetu);
            }
            else
            {
                Trace.TraceError("IC_SetCpuPara failed:cputype={0},cpupro={1},cpuetu={2}", slot, cpupro, cpuetu);
            }
            return rc;
        }

        #region IUltralightIO
        public override int UL_read(byte addr, byte[] rbuff)
        {
            throw new NotImplementedException();
        }

        public override int UL_write(byte addr, byte[] wbuff)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
