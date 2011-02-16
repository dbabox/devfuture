using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// SAM SLOT ##
    /// eg:SAM SLOT C0
    /// �趨��ǰҪ������SAM����ۺš�
    /// </summary>
    public class CommandSamSlot:ICommand
    {
        
        
        #region ICommand ��Ա

        public bool execute(string commandBody,CommandContext ctx)
        {

            if (!commandBody.StartsWith(CommandName, StringComparison.OrdinalIgnoreCase))
            {
                ctx.ReportMessage("ERR>>Command format error:  {0}.", commandBody);
                return false;
            }

            if (commandBody.Length>CommandName.Length)
            {
                string slotStr = commandBody.Substring(CommandName.Length, commandBody.Length - CommandName.Length).Trim();
                byte slot = 0;
                if (Byte.TryParse(slotStr, 
                    System.Globalization.NumberStyles.HexNumber, null, out slot))
                {
                    //ת����ȷ�ĸ�ʽ��
                    if (ctx.Rfid.SAM_SetSlot(slot) == 0)
                    {
                        ctx.ReportMessage("SYS>> Current SAM Slot is 0x{0,2:X2}", ctx.Rfid.CurrentSamSlot);
                        return true;
                    }
                    else
                    {
                        ctx.ReportMessage("ERR>>Command ERROR: {0} ", commandBody);
                        return false;
                    }
                }
            }
            ctx.ReportMessage("ERR>> System Call failed:{0}; Current SAM Slot is 0x{0,2:X2}.",
                commandBody, ctx.Rfid.CurrentSamSlot);
            return false;
        }

        public string CommandName
        {
            get { return "SAMSLOT"; }
        }

         

        #endregion
    }
}
