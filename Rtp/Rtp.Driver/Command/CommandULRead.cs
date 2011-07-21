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
        /// READ 01
        /// </summary>
        /// <param name="input">16进制整数，小于0xFF</param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public bool execute(string input, CommandContext ctx)
        {
            input = Utility.GetSubStringBetweenChars(input, '(', ')').Trim().ToUpper();
            byte addr = Convert.ToByte(input, 16);
            Array.Clear(ctx.rbuff, 0, ctx.rbuff.Length);
            int rc=ctx.Rfid.UL_read(addr, ctx.rbuff);
            if (rc == 0)
            {
                System.Diagnostics.Trace.WriteLine(String.Format( "UL_read :addr={0},rbuff={1}",
                    addr, Utility.ByteArrayToHexStr(ctx.rbuff,64)));
                ctx.rlen = 4;
                return true;
            }
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
