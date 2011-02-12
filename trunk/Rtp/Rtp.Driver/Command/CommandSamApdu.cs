using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// 执行SAM卡指令。实际脚本中，是用命令头来决定后续COS指令时针对SAM卡还是CPU卡的。
    /// </summary>
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
                        ctx.ReportMessage("ERR: AppandMac failed.");
                        return false;
                    }

                }
                ctx.rc = ctx.Rfid.SAM_APDU(ctx.Rfid.CurrentSamSlot,
                    ctx.slen, ctx.sbuff, ref ctx.rlen, ctx.rbuff);
                ctx.ReportMessage("SYS>> SAM_APDU RC={0}", ctx.rc);
                return ctx.rc == 0 && Utility.IsSwSuccess(ctx.rlen, ctx.rbuff);
            }
            else
            {
                ctx.ReportMessage("ERR: Command format incorrect:{0}.", commandBody);
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
