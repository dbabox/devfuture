using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rtp.Driver.Command
{
    class CommandCheckSumXor : ICommand
    {
        public bool execute(string input, CommandContext ctx)
        {
            ctx.slen= (byte)Utility.HexStrToByteArray(input,ref ctx.sbuff);
            if (ctx.slen > 0)
            {
                byte cs= Utility.CheckSumXor(ctx.sbuff,ctx.slen);
                ctx.rlen = 1;
                ctx.rbuff[0] = cs;
                return true;
            }
            return false;
        }

        public string CommandName
        {
            get 
            {
                return "SYS<CHECKSUMXOR";
            }
        }
    }
}
