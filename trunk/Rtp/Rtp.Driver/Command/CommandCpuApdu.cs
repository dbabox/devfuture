/*
 * 普通明文COS指令
 * 
 * 
 * */
using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// 执行CPU卡COS指令。
    /// </summary>
    public class CommandCpuApdu:ICommand
    {
        

         
        public CommandCpuApdu()
        {
            
        }
        #region ICommand 成员

        public bool execute(string commandBody, CommandContext ctx)
        {
            #region 直接命令 ==>结束
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
                ctx.ReportMessage("SYS>> CommandBody={0}", commandBody);
                ctx.rc = ctx.Rfid.CPU_APDU(ctx.slen, ctx.sbuff, ref ctx.rlen, ctx.rbuff);
                ctx.ReportMessage("SYS>> CPU_APDU RC={0}", ctx.rc);
                return ctx.rc==0 && Utility.IsSwSuccess(ctx.rlen, ctx.rbuff);
            }
            else
            {
                ctx.ReportMessage("ERR: Command format incorrect:{0}.", commandBody);
                return false;
            }
            #endregion
        }

        public string CommandName
        {
            get { return "CPU"; }
        }

        #endregion
    }
}
