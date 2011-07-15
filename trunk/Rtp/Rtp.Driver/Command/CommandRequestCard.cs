using System;
using System.Collections.Generic;
using System.Text;
using Rtp.Driver.RfidReader;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// REQUEST CARD ����һ�ſ�����Ѱ����
    /// </summary>
    public class CommandRequestCard:ICommand
    {
        

       

        public CommandRequestCard()
        {
            
        }

        #region ICommand ��Ա

        public bool execute(string commandBody, CommandContext ctx)
        {
            if (!commandBody.Equals(CommandName, StringComparison.OrdinalIgnoreCase))
            {
                ctx.ReportMessage("ERR>>{0} is not {2}.", commandBody, CommandName);
                return false;
            }
            #region Ѱ��
            CardPhysicalType cpt = CardPhysicalType.Unknown;           
            string snr = ctx.Rfid.Request(out cpt);
            ctx.ReportMessage("CARD>> {0} / {1}", snr, cpt);
            bool result=false;
            switch(cpt)
            {
                case CardPhysicalType.CPU_TypeA:
                    {
                         //�Զ�Reset
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
                        ctx.ReportMessage("ERR>>ϵͳĿǰ��֧��TypeB��");
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
                        ctx.ReportMessage("ERR>>��δʵ�ֶ�M1����֧��.");
                        break;
                    }
                default:
                    {
                        result = false;
                        ctx.ReportMessage("ERR>>δ֪�Ŀ�����.");
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
