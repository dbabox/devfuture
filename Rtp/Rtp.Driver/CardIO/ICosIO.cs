using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.CardIO
{
    public interface ICosIO
    {
        System.Collections.Generic.IDictionary<UInt16, string> CosDic { get; }

        string GetDescription(ushort cmd);
        string GetDescription(byte B1, byte B2);

        string CosName { get; }
    }
}
