using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    public class CommandSamApdu:ICommand
    {
          
        

        public CommandSamApdu()
        {
            
        }

        #region ICommand 成员

        public bool execute(string commandBody, CommandContext ctx)
        {
            //将命令字符转换成数组
            ctx.slen = (byte)Utility.HexStrToByteArray(commandBody, ref ctx.sbuff);
            if (ctx.slen > 0)
            {
                //执行命令
                if (ctx.IsMacOn || (ctx.sbuff[0] & 0x0F) == 0x04) //计算MAC
                {
                    //附加MAC
                    if (!ctx.AppandMac())
                    {
                        Console.WriteLine("ERR: AppandMac failed.");
                        return false;
                    }

                }
                ctx.rc = ctx.Rfid.SAM_APDU(ctx.Rfid.CurrentSamSlot,
                    ctx.slen, ctx.sbuff, ref ctx.rlen, ctx.rbuff);
                System.Diagnostics.Trace.TraceInformation("SYS>> SAM_APDU RC={0}", ctx.rc);
                return ctx.rc == 0 && Utility.IsSwSuccess(ctx.rlen, ctx.rbuff);
            }
            else
            {
                Console.WriteLine("ERR: Command format incorrect:{0}.", commandBody);
                return false;
            }
           
        }

        public string CommandName
        {
            get { return "SAM"; }
        }

       

        #endregion
    }
}
