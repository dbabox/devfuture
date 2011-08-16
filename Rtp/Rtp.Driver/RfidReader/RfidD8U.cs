using System;
using System.Collections.Generic;
using System.Text;

using Rtp.Driver.CardIO;

namespace Rtp.Driver.RfidReader
{
    public class RfidD8U : RfidBase 
    {
        
        private readonly byte COS_CMD_TIME_OUT = 10;
        private const short D8U_PORT_NUM = 100;
        private readonly ushort BEEP_MSEC = 2;              //����ʱ�� 2m 
        private IntPtr icdev = System.IntPtr.Zero;          //�豸������

     

        /// <summary>
        /// �������豸������.ֻ��.
        /// </summary>
        public IntPtr Icdev
        {
            get { return icdev; }
        }

        #region �ڲ���������

        /// <summary>
        /// ����Request������õ�����ֵ�жϿ����͡�
        /// </summary>
        /// <param name="tagType"></param>
        /// <returns></returns>
        internal CardPhysicalType GetCardType(int tagType)
        {
            logger.InfoFormat("card tag type:{0}", tagType);
            switch (tagType)
            {
                case NMDcrf32V8.RQ_TRAIT_ULTRALIGHT: return CardPhysicalType.UltraLight;
                case NMDcrf32V8.RQ_TRAIT_MIFARE_PRO: return CardPhysicalType.CPU_TypeA;
                case NMDcrf32V8.RQ_TRAIT_MIFARE1: return CardPhysicalType.MifareOne;
                default:
                    return CardPhysicalType.Unknown;
            }
        }

        /// <summary>
        /// Read������ÿ�ֿ�һ�οɶ�ȡ���ֽ�����ҪôΪ8��ҪôΪ16��
        /// </summary>
        /// <param name="cpt"></param>
        /// <returns></returns>
        internal int GetReadSize(CardPhysicalType cpt)
        {
            switch (cpt)
            {
                case CardPhysicalType.MifareOne: return 16;
                case CardPhysicalType.UltraLight: return 16;
                case CardPhysicalType.MifareLight: return 8;
                default: throw new ArgumentException("Read������֧�ֵĿ����ͣ�" + cpt.ToString());
            }
        }

        #endregion

        #region IRfid ��Ա

        public override int Open()
        {
            System.Diagnostics.Trace.Assert(icdev == System.IntPtr.Zero);
            icdev = NMDcrf32V8.dc_init(D8U_PORT_NUM, 0);
            logger.InfoFormat("===============dc_init :icdev={0}============", icdev);
            if (icdev.ToInt32() > 0)
            {
                NMDcrf32V8.dc_beep(icdev, BEEP_MSEC);
                byte[] ver = new byte[3];
                NMDcrf32V8.dc_getver(icdev, ver);
                logger.InfoFormat("SYS>> Device Version:{0,2:X2}{1,2:X2}.", ver[0], ver[2]);

            }
            else
            {
                logger.WarnFormat("dc_init failed!");
                icdev = System.IntPtr.Zero;
            }
            return icdev.ToInt32();
        }

        public override int Close()
        {
            NMDcrf32V8.dc_beep(icdev, BEEP_MSEC);
            short rc = NMDcrf32V8.dc_exit(icdev);
            logger.InfoFormat("*****************dc_exit:icdev={0},rc={1}**************", icdev.ToString(), rc);
            if (rc == 0) icdev = System.IntPtr.Zero;//�ɹ��رգ������豸������Ϊ0.
            return rc;
        }

        public override int DeviceReset()
        {
            short rc = NMDcrf32V8.dc_reset(icdev, 2);
            if (rc == 0)
                logger.InfoFormat("��Ƶ��λ�ɹ���");
            else
                logger.ErrorFormat("��Ƶ��λʧ�ܣ�");
            return rc;
        }

        public override bool IsOpened()
        {
            return icdev != System.IntPtr.Zero;
        }

        public override string Request(out CardPhysicalType cpt)
        {
            cpt = CardPhysicalType.Unknown;
            ushort tagType = 0;
            bool isTwoTried = false;
        TWOTRY:

            //����
            short rc = NMDcrf32V8.dc_request(icdev, NMDcrf32V8.SELECT_MODE_IDLE, ref tagType);//�������ʧ�ܣ�������tagType����ֵ�����
            if (rc == 0)
            {
                cpt = GetCardType(tagType);
                logger.InfoFormat("dc_request OK! Card Type={0}", cpt);
                byte[] snr = new byte[8];
                Array.Clear(snr, 0, snr.Length);

                rc = NMDcrf32V8.dc_anticoll(icdev, 0, snr); //����ͻ
                if (rc == 0)
                {
                    logger.InfoFormat("dc_anticoll OK! snr={0}", Utility.ByteArrayToHexStr(snr, 4));
                    uint temp = 0;
                    uint iSnr = snr[0];
                    iSnr |= ((temp = snr[1]) << 8);
                    iSnr |= ((temp = snr[2]) << 16);
                    iSnr |= ((temp = snr[3]) << 24);

                    byte size = 0;
                    rc = NMDcrf32V8.dc_select(icdev, iSnr, ref size); //ѡ��
                    if (rc == 0)
                    {
                        logger.InfoFormat("dc_select OK! size={0}", size);
                        return Utility.ByteArrayToHexStr(snr, 4);
                    }
                    else
                    {
                        logger.InfoFormat("dc_select failed! rc={0}", rc);
                    }
                }
            }
            else
            {
                logger.WarnFormat("dc_request failed.rc={0},tagType={1}", rc, tagType);
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
            byte[] ver = new byte[3];
            NMDcrf32V8.dc_getver(icdev, ver);           
            return Utility.ByteArrayToHexStr(ver,2,"");
        }

        #endregion

        #region ICpuIO ��Ա
        /// <summary>
        /// �ǽӴ�ʽCPU����λ��һ��ģ�ʹ��CPU��Ѱ��������ø�λ��Ϣ��
        /// </summary>
        /// <param name="rlen"></param>
        /// <param name="rbuff"></param>
        /// <returns></returns>
        public override int CPU_Reset(out byte rlen, byte[] rbuff)
        {
            byte type = 0;
            return NMDcrf32V8.dc_cardAB(icdev, out rlen, rbuff, out type);//type='A'��ʾA����'B'��ʾB����
        }

        /// <summary>
        /// ִ��CPU��COSָ����COS��Ӧ���ɹ�����0�����򷵻ط�0�����롣
        /// </summary>
        /// <param name="slen"></param>
        /// <param name="sbuff"></param>
        /// <param name="rlen"></param>
        /// <param name="rbuff"></param>
        /// <returns></returns>
        public override int CPU_APDU(byte slen, byte[] sbuff, ref byte rlen, byte[] rbuff)
        {
            if (slen == 0 || sbuff==null || sbuff.Length==0)
            {
                return -1;
            }
           
           
            logger.InfoFormat("dc_pro_command:slen={0},sbuff={1}", slen, Utility.ByteArrayToHexStr(sbuff, slen));
            OnCpuRequest(slen, sbuff);
            int rc = NMDcrf32V8.dc_pro_command(icdev, slen, sbuff, ref rlen, rbuff, COS_CMD_TIME_OUT);
            if (rc == 0)
            {
                OnCpuResponse(rlen, rbuff);
            }
            else
            {
                logger.InfoFormat("dc_pro_command failed:rc={0}", rc);
            }
            return rc;
        }

        #endregion

        #region ISamIO ��Ա
        /// <summary>
        /// ��λSAM��������0�ɹ���
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="rlen"></param>
        /// <param name="dataBuff"></param>
        /// <returns></returns>
        public override int SAM_Reset(byte slot, ref byte rlen, byte[] rbuff)
        {
            if (currentSamSlot != slot)
            {
                SAM_SetSlot(slot);
            }
            short rc = NMDcrf32V8.dc_cpureset(icdev, ref rlen, rbuff);
            if (rc == 0)
            {
                logger.InfoFormat("dc_cpureset success: rlen={0},databuff={1}",
                    rlen, Utility.ByteArrayToHexStr(rbuff, rlen));
                OnSamResponse(slot, rlen, rbuff);
            }
            else
            {
                logger.ErrorFormat("dc_cpureset failed: rc={0}", rc.ToString("X"));
            }
            return rc;
        }

        public override int SAM_SetSlot(byte slot)
        {
            short rc= NMDcrf32V8.dc_setcpu(icdev, slot);
            if (rc == 0)
            {
                currentSamSlot = slot;
            }
            else
            {
                currentSamSlot = INVALID_SAM_SLOT;
                logger.ErrorFormat("dc_setcpu failed: rc={0}", rc.ToString("X"));
            }
            return rc;
        }

        public override int SAM_APDU(byte slot, byte slen, byte[] sbuff, ref byte rlen, byte[] rbuff)
        {
            if (currentSamSlot != slot) SAM_SetSlot(slot);
            OnSamRequest(slot, slen, sbuff);
            short rc = NMDcrf32V8.dc_cpuapdu(icdev, slen, sbuff, ref rlen, rbuff);
            if (rc == 0)
            {
                OnSamResponse(slot,rlen, rbuff);
            }
            else
            {
                logger.ErrorFormat("dc_cpuapdu device failed:rc={0}", rc.ToString("X"));
            }
            return rc;
        }

        public override int SAM_SetPara(byte slot, byte cpupro, byte cpuetu)
        {
            if (currentSamSlot != slot) SAM_SetSlot(slot);
            short rc = NMDcrf32V8.dc_setcpupara(icdev, slot, cpupro, cpuetu);
            if (rc == 0)
            {
                logger.InfoFormat("dc_setcpupara success:cputype={0},cpupro={1},cpuetu={2}", slot, cpupro, cpuetu);
            }
            else
            {
                logger.ErrorFormat("dc_setcpupara failed:cputype={0},cpupro={1},cpuetu={2}", slot, cpupro, cpuetu);
            }
            return rc;
        }
        #endregion

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
