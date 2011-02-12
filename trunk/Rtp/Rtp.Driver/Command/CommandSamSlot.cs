using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// SAM SLOT ##
    /// eg:SAM SLOT C0
    /// 设定当前要操作的SAM卡插槽号。
    /// </summary>
    public class CommandSamSlot:ICommand
    {
        
        
        #region ICommand 成员

        public bool execute(string commandBody,CommandContext ctx)
        {
            if (!commandBody.StartsWith(CommandName)) throw new ArgumentException(String.Format("{0}命令格式错误:{1}", CommandName, commandBody));

            if (commandBody.Length>CommandName.Length)
            {
                string slotStr = commandBody.Substring(CommandName.Length, commandBody.Length - CommandName.Length).Trim();
                byte slot = 0;
                if (Byte.TryParse(slotStr, 
                    System.Globalization.NumberStyles.HexNumber, null, out slot))
                {
                    //转成正确的格式了
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
            ctx.ReportMessage("SYS>> Current SAM Slot is 0x{0,2:X2}", ctx.Rfid.CurrentSamSlot);
            return false;
        }

        public string CommandName
        {
            get { return "SAM SLOT"; }
        }

         

        #endregion
    }
}
