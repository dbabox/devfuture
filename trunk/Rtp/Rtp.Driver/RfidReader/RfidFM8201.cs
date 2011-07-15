using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.RfidReader
{
    public class RfidFM8201 : RfidBase
    {
        public override int Open()
        {
            throw new NotImplementedException();
        }

        public override int Close()
        {
            throw new NotImplementedException();
        }

        public override int DeviceReset()
        {
            throw new NotImplementedException();
        }

        public override bool IsOpened()
        {
            throw new NotImplementedException();
        }

        public override string Request(out CardPhysicalType cpt)
        {
            throw new NotImplementedException();
        }

        public override string DeviceVersion()
        {
            throw new NotImplementedException();
        }

        public override int CPU_Reset(out byte rlen, byte[] rbuff)
        {
            throw new NotImplementedException();
        }

        public override int CPU_APDU(byte slen, byte[] sbuff, ref byte rlen, byte[] rbuff)
        {
            throw new NotImplementedException();
        }

        public override int SAM_Reset(byte slot, ref byte rlen, byte[] dataBuff)
        {
            throw new NotImplementedException();
        }

        public override int SAM_SetSlot(byte slot)
        {
            throw new NotImplementedException();
        }

        public override int SAM_APDU(byte slot, byte slen, byte[] sbuff, ref byte rlen, byte[] rbuff)
        {
            throw new NotImplementedException();
        }

        public override int SAM_SetPara(byte slot, byte cpupro, byte cpuetu)
        {
            throw new NotImplementedException();
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
