using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    public class CommandSamReset:ICommand
    {
                
        

        public CommandSamReset()
        {
            
        }

        #region ICommand ��Ա

        public bool execute(string commandBody, CommandContext ctx)
        {
            if (!commandBody.StartsWith(CommandName)) throw new ArgumentException(String.Format("{0}�����ʽ����:{1}", CommandName, commandBody));
            if (commandBody.Length > CommandName.Length)
            {
                string slotStr = commandBody.Substring(CommandName.Length, commandBody.Length - CommandName.Length).Trim();
                byte slot = 0;
                if (Byte.TryParse(slotStr,
                    System.Globalization.NumberStyles.HexNumber, null, out slot))
                {
                    //ת����ȷ�ĸ�ʽ��
                    return ctx.Rfid.SAM_Reset(slot,ref ctx.rlen,ctx.rbuff) == 0;
                }
            }
            //�޲���ʱ�����õ�ǰSAM��
            return ctx.Rfid.SAM_Reset(ctx.Rfid.CurrentSamSlot, ref ctx.rlen, ctx.rbuff)==0;
        }

        public string CommandName
        {
            get { return "SAM RESET"; }
        }

       

        #endregion
    }
}
