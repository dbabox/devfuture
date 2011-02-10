/*
 * 
 * NMTrf32.cs 
 * dcard T10N USB��������װ
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
        private readonly ushort BEEP_MSEC = 5;              //����ʱ�� 2m 
        /// <summary>
        /// COSָ��ִ�г�ʱʱ�䣬����10ms
        /// </summary>
        private const byte COS_CMD_TIME_OUT = 16;
        private const byte FG = 56;

        private int icdev;

        public int Icdev
        {
            get { return icdev; }
           
        }

        private int port;

        public int Port
        {
            get { return port; }
            set { port = value; }
        }
        private uint baud;

        public uint Baud
        {
            get { return baud; }
            set { baud = value; }
        }


        private byte samBaudrate;

        /// <summary>
        /// 1��ʾ9600��2��ʾ14400��3��ʾ19200��4��ʾ38400��5��ʾ57600��6��ʾ115200
        /// </summary>
        public byte SamBaudrate
        {
            get { return samBaudrate; }
            set { samBaudrate = value; }
        }

        private byte volt;

        /// <summary>
        /// 2��ʾ3.3V, 3��ʾ5.0V
        /// </summary>
        public byte Volt
        {
            get { return volt; }
            set { volt = value; }
        }

        private byte samProtocol;
        /// <summary>
        /// ��ƬЭ�飨0��ʾT = 0, 1��ʾ T = 1��
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
        /// [in]  port  ȡֵΪ0��19ʱ����ʾ����1��20��Ϊ100ʱ����ʾUSB��ͨѶ����ʱ��������Ч�� 
        /// [in]  baud  ͨѶ�����ʣ�ȡֵΪ9600��115200�� 
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
            if (rc == 0) icdev =0;//�ɹ��رգ������豸������Ϊ0.
            return rc;
        }

        public override int DeviceReset()
        {
            int rc = NMTrf32.dc_reset(icdev);
            if (rc == 0)
                Trace.TraceInformation("��Ƶ��λ�ɹ���");
            else
                Trace.TraceError("��Ƶ��λʧ�ܣ�");
            return rc;
        }

        public override bool IsOpened()
        {
            return icdev > 0;
        }

        /// <summary>
        /// ����Request������õ�����ֵ�жϿ����͡�
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
            //����
            int rc = NMTrf32.dc_request(icdev, Trf32Constants.SELECT_MODE_IDLE, ref tagType);//�������ʧ�ܣ�������tagType����ֵ�����
            if (rc == 0)
            {
                cpt = GetCardType(tagType);                
                Trace.TraceInformation("dc_request OK! Card Type={0}", cpt);
                
                rc = NMTrf32.dc_anticoll(icdev, 0, ref snr); //����ͻ
                if (rc == 0)
                {
                    Trace.TraceInformation("dc_anticoll OK! snr={0}", snr);
                    byte size = 0;
                    rc = NMTrf32.dc_select(icdev, snr, ref size); //ѡ��
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

        public override int SAM_Reset(byte slot, ref byte rlen, byte[] dataBuff)
        {
            if (currentSamSlot != slot) SAM_SetSlot(slot);
            int rc = NMTrf32.dc_cpureset(icdev, slot, samBaudrate, volt, ref rlen, dataBuff, ref samProtocol);
            if (rc == 0)
            {
                Trace.TraceInformation("dc_cpureset success: rlen={0},databuff={1}",
                    rlen, Utility.ByteArrayToHexStr(dataBuff, rlen));
                OnCpuResponse(rlen, dataBuff);
            }
            else
            {
                Trace.TraceError("dc_cpureset failed: rc={0}", rc.ToString("X"));
            }
            return rc;
        }

        /// <summary>
        /// 0x0c��ʾ������,0x0d��ʾΪSAM1,0x0e��ʾΪSAM2,0x0f��ʾSAM3,0x10��ʾSAM4
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
        /// ����Э������,������
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="cpupro"></param>
        /// <param name="cpuetu">������</param>
        /// <returns></returns>
        public override int SAM_SetPara(byte slot, byte cpupro, byte cpuetu)
        {
            Trace.TraceWarning("T10N not support set cpupro.");
            byte rlen=0;
            byte[] rbuff=new byte[64];
            int rc=NMTrf32.dc_cpureset(icdev, slot, cpuetu, volt, ref rlen, rbuff, ref samProtocol);
            if (rc == 0)
            {
                samBaudrate = cpuetu;
            }
            return rc;
        }
    }
}
