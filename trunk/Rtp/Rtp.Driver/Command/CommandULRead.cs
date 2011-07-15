using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// 读取UL卡的1个Page
    /// </summary>
    class CommandULRead : ICommand
    {
        /// <summary>
        /// 读取UL卡的1个page；
        /// </summary>
        /// <param name="input">16进制整数，小于0xFF</param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public bool execute(string input, CommandContext ctx)
        {
            byte addr = byte.Parse(input, System.Globalization.NumberStyles.AllowHexSpecifier);
            int rc=ctx.Rfid.UL_read(addr, ctx.rbuff);
            if (rc == 0) return true;
            else
            {
                ctx.ReportMessage("ERR>>UL_read failed:rc={0}", rc);
                return false;
            }
        }

        public string CommandName
        {
            get { return "UL<READ"; }
        }
    }
}
