using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// 启用COS指令线路保护。
    /// </summary>
    public class CommandMacOn:ICommand
    {
        
 

        public CommandMacOn()
        {
            
        }

        #region ICommand 成员

        public bool execute(string commandBody, CommandContext ctx)
        {
            ctx.IsMacOn = true;
            ctx.ReportMessage("SYS>> Turn ON MAC");
            return true;
        }

        public string CommandName
        {
            get { return "MAC ON"; }
        }

        #endregion
    }
}
