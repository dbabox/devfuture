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
            bool result=false;
            switch(cpt)
            {
                case CardPhysicalType.CPU_TypeA:
                    {
                         //自动Reset
                        ctx.rc = ctx.Rfid.CPU_Reset(out ctx.rlen, ctx.rbuff);
                        if (ctx.rc == 0)
                        {
                            ctx.ReportMessage("CARD>> ATS={0}", Utility.ByteArrayToHexStr(ctx.rbuff, ctx.rlen));
                            result=true;
                        }
                        else
                        {
                            ctx.ReportMessage("SYS>> CPU RESET RC={0}", ctx.rc);
                            result= false;
                        }
                        break;
                    }
                case CardPhysicalType.CPU_TypeB:
                    {
                        result=false;
                        ctx.ReportMessage("ERR>>系统目前不支持TypeB卡");
                        break;
                    }
                case CardPhysicalType.UltraLight:
                    {
                        result = true;
                        ctx.ReportMessage("SYS>> UL SNR={0}", snr);
                        break;
                    }
                case CardPhysicalType.MifareOne:
                    {
                        result = false;
                        ctx.ReportMessage("ERR>>还未实现对M1卡的支持.");
                        break;
                    }
                default:
                    {
                        result = false;
                        ctx.ReportMessage("ERR>>未知的卡类型.");
                        break;
                    }
            }   
            return result;
            #endregion
        }

        public string CommandName
        {
            get { return "SYS<REQUESTCARD"; }
        }

        #endregion
    }
}
