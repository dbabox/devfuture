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

        /// <summary>
        /// 执行SAM的COS指令
        /// </summary>
        /// <param name="args">slot,cos cmd</param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public bool execute(string args, CommandContext ctx)
        {
            int rc = 0;
            string parline = Utility.GetSubStringBetweenChars(args, '(', ')');

            #region 分解组合命令后执行
            string[] pars = parline.Split(',');
            if (pars.Length != 2)
            {
                ctx.ReportMessage("ERR>> CommandSamApdu:参数{0}格式错误，必须是slot,cos.", args);
                return false;
            }
            byte slot= Byte.Parse(pars[0], System.Globalization.NumberStyles.HexNumber);
            if (slot != ctx.Rfid.CurrentSamSlot)
            {
                rc = ctx.Rfid.SAM_SetSlot(slot);
                if (rc != 0)
                {
                    ctx.ReportMessage("ERR>> CommandSamApdu:Set_Slot({0}) 失败.RC={1}.",slot,rc);
                    return false;
                }
            }
            string[] cmdarr = pars[1].Split('/');
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
                ctx.ReportMessage("SYS>> CommandBody={0}", args);
                ctx.rc = ctx.Rfid.SAM_APDU(ctx.Rfid.CurrentSamSlot,
                    ctx.slen, ctx.sbuff, ref ctx.rlen, ctx.rbuff);
                ctx.ReportMessage("SYS>> SAM_APDU RC={0}", ctx.rc);
                //继续执行后续语句...
                if (ctx.IsBreakOnFailed && (ctx.rc != 0 || !Utility.IsSwSuccess(ctx.rlen, ctx.rbuff)))
                {
                    ctx.ReportMessage("ERR>> Command[{0}] failed:{1}", 0, Utility.ByteArrayToHexStr(ctx.sbuff, ctx.slen));
                    if (ctx.IsBreakOnFailed) return false;
                }
            }
            else
            {
                ctx.ReportMessage("ERR: Command format incorrect:{0}.", args);
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
                    ctx.ReportMessage("ERR>> Command format incorrect:{0}", args);
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

                ctx.ReportMessage("SYS>> CommandBody={0}", args);
                ctx.rc = ctx.Rfid.SAM_APDU(ctx.Rfid.CurrentSamSlot,
                     ctx.slen, ctx.sbuff, ref ctx.rlen, ctx.rbuff);
                ctx.ReportMessage("SYS>> SAM_APDU RC={0}", ctx.rc);

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
            get { return "SAM<APDU"; }
        }

       

        #endregion
    }
}
