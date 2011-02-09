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

        #region ICommand 成员

        public bool execute(string commandBody, CommandContext ctx)
        {
            if (!commandBody.StartsWith(CommandName)) throw new ArgumentException(String.Format("{0}命令格式错误:{1}", CommandName, commandBody));
            if (commandBody.Length > CommandName.Length)
            {
                string slotStr = commandBody.Substring(CommandName.Length, commandBody.Length - CommandName.Length).Trim();
                byte slot = 0;
                if (Byte.TryParse(slotStr,
                    System.Globalization.NumberStyles.HexNumber, null, out slot))
                {
                    //转成正确的格式了
                    return ctx.Rfid.SAM_Reset(slot,ref ctx.rlen,ctx.rbuff) == 0;
                }
            }
            //无参数时，重置当前SAM卡
            return ctx.Rfid.SAM_Reset(ctx.Rfid.CurrentSamSlot, ref ctx.rlen, ctx.rbuff)==0;
        }

        public string CommandName
        {
            get { return "SAM RESET"; }
        }

       

        #endregion
    }
}
