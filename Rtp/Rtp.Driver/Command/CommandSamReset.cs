using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// SAM卡复位SAM RESET。
    /// </summary>
    public class CommandSamReset:ICommand
    {
                
        

        public CommandSamReset()
        {
            
        }

        #region ICommand 成员

        public bool execute(string commandBody, CommandContext ctx)
        {

            if (!commandBody.StartsWith(CommandName, StringComparison.OrdinalIgnoreCase))
            {
                ctx.ReportMessage("ERR>>Command format error:  {0}.", commandBody);
                return false;
            }
            byte slot = 0;
            if (commandBody.Length > CommandName.Length)
            {
                string slotStr = commandBody.Substring(CommandName.Length, commandBody.Length - CommandName.Length).Trim();

                if (!Byte.TryParse(slotStr,
                    System.Globalization.NumberStyles.HexNumber, null, out slot))
                {
                    ctx.ReportMessage("ERR>>{0} failed. SAM slot is not hex byte.", commandBody);
                    return false;
                }
            }
            else
            {
                slot = ctx.Rfid.CurrentSamSlot;
            }
            int rc = 0;
            //转成正确的格式了
            if ((rc = ctx.Rfid.SAM_Reset(slot, ref ctx.rlen, ctx.rbuff)) == 0)
            {
                ctx.ReportMessage("{0} success:rlen=0x{1,2:X2},rbuff={2},CurrentSamSlot={3,2:X2}", commandBody, ctx.rlen,
                    Utility.ByteArrayToHexStr(ctx.rbuff, ctx.rlen),ctx.Rfid.CurrentSamSlot);
                return true;
            }
            else
            {
                ctx.ReportMessage("ERR>> {0} failed.RC={1}", commandBody, rc);
                return false;
            }
             
        }

        public string CommandName
        {
            get { return "SAMRESET"; }
        }

       

        #endregion
    }
}
