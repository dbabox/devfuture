using System;
using System.Collections.Generic;
using System.Text;
using Rtp.Driver.RfidReader;

namespace Rtp.Driver.Command
{
    public class CommandRequestCard:ICommand
    {
        

       

        public CommandRequestCard()
        {
            
        }

        #region ICommand ��Ա

        public bool execute(string commandBody, CommandContext ctx)
        {
            #region Ѱ��
            CardPhysicalType cpt = CardPhysicalType.Unknown;           
            string snr = ctx.Rfid.Request(out cpt);
            System.Diagnostics.Trace.TraceInformation("CARD>> {0} / {1}", snr, cpt);
            if (cpt != CardPhysicalType.Unknown)
            {
                if (cpt == CardPhysicalType.CPU_TypeA
                    || cpt == CardPhysicalType.CPU_TypeB)
                {
                    //�Զ�Reset
                    ctx.rc = ctx.Rfid.CPU_Reset(out ctx.rlen, ctx.rbuff);
                    if (ctx.rc == 0)
                    {
                        System.Diagnostics.Trace.TraceInformation("CARD>> ATS={0}", Utility.ByteArrayToHexStr(ctx.rbuff, ctx.rlen));
                        return true;
                    }
                    else
                    {
                        System.Diagnostics.Trace.TraceInformation("SYS>> CPU RESET RC={0}", ctx.rc);
                        return false;
                    }
                    //����CPU��Ѱ����λ���
                }
                else
                {
                    //TODO:  ���������ֵĴ���
                    return false;
                }

            }
            System.Diagnostics.Trace.TraceError("ERR>> REQUEST CARD failed. cpt={0}", cpt);
            return false;
            #endregion
        }

        public string CommandName
        {
            get { return "REQUEST CARD"; }
        }

        #endregion
    }
}
