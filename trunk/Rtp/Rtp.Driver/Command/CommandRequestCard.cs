using System;
using System.Collections.Generic;
using System.Text;
using Rtp.Driver.RfidReader;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// REQUEST CARD 请求一张卡。即寻卡。
    /// </summary>
    public class CommandRequestCard:ICommand
    {
        

       

        public CommandRequestCard()
        {
            
        }

        #region ICommand 成员

        public bool execute(string commandBody, CommandContext ctx)
        {
            if (!commandBody.Equals(CommandName, StringComparison.OrdinalIgnoreCase))
            {
                ctx.ReportMessage("ERR>>{0} is not {2}.", commandBody, CommandName);
                return false;
            }
            #region 寻卡
            CardPhysicalType cpt = CardPhysicalType.Unknown;           
            string snr = ctx.Rfid.Request(out cpt);
            ctx.ReportMessage("CARD>> {0} / {1}", snr, cpt);
            if (cpt != CardPhysicalType.Unknown)
            {
                if (cpt == CardPhysicalType.CPU_TypeA
                    || cpt == CardPhysicalType.CPU_TypeB)
                {
                    //自动Reset
                    ctx.rc = ctx.Rfid.CPU_Reset(out ctx.rlen, ctx.rbuff);
                    if (ctx.rc == 0)
                    {
                        ctx.ReportMessage("CARD>> ATS={0}", Utility.ByteArrayToHexStr(ctx.rbuff, ctx.rlen));
                        return true;
                    }
                    else
                    {
                        ctx.ReportMessage("SYS>> CPU RESET RC={0}", ctx.rc);
                        return false;
                    }
                    //至此CPU卡寻卡复位完毕
                }
                else
                {
                    //TODO:  对其他卡种的处理
                    return false;
                }

            }
            System.Diagnostics.Trace.TraceError("ERR>> REQUEST CARD failed. cpt={0}", cpt);
            return false;
            #endregion
        }

        public string CommandName
        {
            get { return "REQUESTCARD"; }
        }

        #endregion
    }
}
