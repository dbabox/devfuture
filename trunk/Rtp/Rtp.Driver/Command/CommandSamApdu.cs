using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// ִ��SAM��ָ�ʵ�ʽű��У���������ͷ����������COSָ��ʱ���SAM������CPU���ġ�
    /// </summary>
    public class CommandSamApdu:ICommand
    {
          
        

        public CommandSamApdu()
        {
            
        }

        #region ICommand ��Ա

        public bool execute(string commandBody, CommandContext ctx)
        {
            //�������ַ�ת��������
            ctx.slen = (byte)Utility.HexStrToByteArray(commandBody, ref ctx.sbuff);
            if (ctx.slen > 0)
            {
                //ִ������
                if (ctx.IsMacOn || (ctx.sbuff[0] & 0x0F) == 0x04) //����MAC
                {
                    //����MAC
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
