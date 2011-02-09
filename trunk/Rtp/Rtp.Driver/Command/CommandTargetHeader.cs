using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    public class CommandTargetHeader:ICommand
    {
        

      
       
        #region ICommand 成员

        public bool execute(string commandBody, CommandContext ctx)
        {
            if (String.IsNullOrEmpty(commandBody)||commandBody=="CPU")
            {
                ctx.CmdTarget = "CPU";
                return true;
            }   
            if (commandBody.StartsWith("SAM") && commandBody.Length>4)
            {
                ctx.CmdTarget = "SAM";//eg:SAM 1
                string slotstr = commandBody.Substring(3, commandBody.Length - 3).Trim();
                slotstr = slotstr.Substring(2, slotstr.Length - 2);//去除0x前导符
                byte slot=0;
                if (Byte.TryParse(slotstr, System.Globalization.NumberStyles.HexNumber, null, out slot))
                {
                    ctx.Rfid.CurrentSamSlot = slot;
                    return true;
                }
                else
                {
                    System.Diagnostics.Trace.TraceError("ERR>>{0} format incorrect.", commandBody);
                    return false;
                }
            }
            return false;        
        }

        public string CommandName
        {
            get { return "<"; }
        }

       

        #endregion
    }
}
