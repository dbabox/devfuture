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

            #region 分解组合命令后执行
            string[] cmdarr = commandBody.Split('/');
            ctx.slen = (byte)Utility.HexStrToByteArray(cmdarr[0], ref ctx.sbuff); //得到第一条命令
            #region 执行命令
            if (ctx.slen > 0)
            {
                //执行命令
                if (ctx.IsMacOn || (ctx.sbuff[0] & 0x0F) == 0x04) //计算MAC
                {
                    //附加MAC
                    if (!ctx.AppandMac())
                    {
                        ctx.ReportMessage("ERR>>System call failed: AppandMac failed.");
                        return false;
                    }

                }
                ctx.ReportMessage("SYS>> CommandBody={0}", commandBody);
                ctx.rc = ctx.Rfid.CPU_APDU(ctx.slen, ctx.sbuff, ref ctx.rlen, ctx.rbuff);
                ctx.ReportMessage("SYS>> CPU_APDU RC={0}", ctx.rc);
                //继续执行后续语句...
                if (ctx.IsBreakOnFailed && (ctx.rc != 0 || !Utility.IsSwSuccess(ctx.rlen, ctx.rbuff)))
                {
                    ctx.ReportMessage("ERR>> Command[{0}] failed:{1}", 0, Utility.ByteArrayToHexStr(ctx.sbuff, ctx.slen));
                    if (ctx.IsBreakOnFailed) return false;
                }  
            }
            else
            {
                ctx.ReportMessage("ERR: Command format incorrect:{0}.", commandBody);
                return false;
            }
            #endregion

            byte[] parv = new byte[ctx.slen]; //后续的部分指令组合子
            int parlen = 0;
            for (int i = 1; i < cmdarr.Length; i++)
            {
                parlen = Utility.HexStrToByteArray(cmdarr[i], ref parv);
                if (parlen <= 0 || parlen > ctx.slen)
                {
                    ctx.ReportMessage("ERR>> Command format incorrect:{0}", commandBody);
                    return false;
                }
                Array.Copy(parv, 0, ctx.sbuff, ctx.slen - parlen, parlen);
                if (ctx.IsMacOn || (ctx.sbuff[0] & 0x0F) == 0x04) //计算MAC
                {
                    //附加MAC
                    if (!ctx.AppandMac())
                    {
                        ctx.ReportMessage("ERR>>System call failed: AppandMac failed.");
                        return false;
                    }

                }

                ctx.ReportMessage("SYS>> CommandBody={0}", commandBody);
                ctx.rc = ctx.Rfid.CPU_APDU(ctx.slen, ctx.sbuff, ref ctx.rlen, ctx.rbuff);
                ctx.ReportMessage("SYS>> CPU_APDU RC={0}", ctx.rc);

                if (ctx.IsBreakOnFailed && (ctx.rc != 0 || !Utility.IsSwSuccess(ctx.rlen, ctx.rbuff)))
                {
                    ctx.ReportMessage("ERR>> Command[{0}] failed:{1}", i, Utility.ByteArrayToHexStr(ctx.sbuff, ctx.slen));
                    if (ctx.IsBreakOnFailed) return false;
                }
            }
            //全部成功执行，返回true
            return true;
            
            #endregion
                        
        }

        public string CommandName
        {
            get { return "CPU"; }
        }

        #endregion
    }
}
