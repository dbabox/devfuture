/*
 * ��ͨ����COSָ��
 * 
 * 
 * */
using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// ִ��CPU��COSָ�
    /// </summary>
    public class CommandCpuApdu:ICommand
    {
        

         
        public CommandCpuApdu()
        {
            
        }
        #region ICommand ��Ա

        public bool execute(string commandBody, CommandContext ctx)
        {
            #region ֱ������ ==>����
            ctx.slen = (byte)Utility.HexStrToByteArray(commandBody, ref ctx.sbuff);
            if (ctx.slen > 0)
            {
                //ִ������
                if (ctx.IsMacOn || (ctx.sbuff[0] & 0x0F) == 0x04) //����MAC
                {
                    //����MAC
                    if (!ctx.AppandMac())
                    {
                        System.Diagnostics.Trace.TraceError("ERR: AppandMac failed.");
                        return false;
                    }

                }
                ctx.rc = ctx.Rfid.CPU_APDU(ctx.slen, ctx.sbuff, ref ctx.rlen, ctx.rbuff);
                System.Diagnostics.Trace.TraceInformation("SYS>> CPU_APDU RC={0}", ctx.rc);
                return ctx.rc==0 && Utility.IsSwSuccess(ctx.rlen, ctx.rbuff);
            }
            else
            {
                System.Diagnostics.Trace.TraceError("ERR: Command format incorrect:{0}.", commandBody);
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
