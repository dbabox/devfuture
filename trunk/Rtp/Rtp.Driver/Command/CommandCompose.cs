using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// 组合操作符/。它用于执行连续几个同类指令。如：00 A4 00 02 3F 00/10 01/00 19
    /// 要求后续组合的部分可完全替换前面的部分，且替换的部分要相等。
    /// </summary>
    public class CommandCompose:ICommand
    {
        

        
        public CommandCompose()
        {
            
        }

        #region ICommand 成员

        public bool execute(string commandBody, CommandContext ctx)
        {
            if (commandBody.Contains("/")) //组合命令，不允许有MAC
            {
                #region 分解组合命令后执行
                string[] cmdarr = commandBody.Split('/');
                ctx.slen = (byte)Utility.HexStrToByteArray(cmdarr[0], ref ctx.sbuff); //得到第一条命令
                ctx.rc = ctx.Rfid.CPU_APDU(ctx.slen, ctx.sbuff, ref ctx.rlen, ctx.rbuff);
                if (ctx.rc != 0 || Utility.IsSwSuccess(ctx.rlen, ctx.rbuff) == false)
                {
                    System.Diagnostics.Trace.TraceError("ERR>> Command[0] failed:{0}", Utility.ByteArrayToHexStr(ctx.sbuff, ctx.slen));
                    return false;
                }
                byte[] parv = new byte[ctx.slen]; //后续的部分指令组合子
                int parlen = 0;
                for (int i = 1; i < cmdarr.Length; i++)
                {
                    parlen = Utility.HexStrToByteArray(cmdarr[i], ref parv);
                    if (parlen <= 0 || parlen > ctx.slen)
                    {
                        System.Diagnostics.Trace.TraceError("ERR>> Command format incorrect:{0}", commandBody);
                        return false;
                    }
                    Array.Copy(parv, 0, ctx.sbuff, ctx.slen - parlen, parlen);
                    
                    ctx.rc = ctx.Rfid.CPU_APDU(ctx.slen, ctx.sbuff, ref ctx.rlen, ctx.rbuff);
                    if (ctx.rc != 0 || !Utility.IsSwSuccess(ctx.rlen, ctx.rbuff))
                    {
                        System.Diagnostics.Trace.TraceError("ERR>> Command[{0}] failed:{1}", i, Utility.ByteArrayToHexStr(ctx.sbuff, ctx.slen));
                        return false;
                    }
                    else
                    {
                        System.Diagnostics.Trace.TraceInformation("SYS>> Command[{0}] success:{1}", i, Utility.ByteArrayToHexStr(ctx.sbuff, ctx.slen));
                        //继续执行
                    }
                }
                return true;
                #endregion
            }
            return false;
        }

        public string CommandName
        {
            get { return "/"; }
        }

        #endregion
    }
}
