using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// 关闭命令的MAC自动计算功能。当COS指令需要线路保护时使用。
    /// </summary>
    public class CommandMacOff:ICommand
    {
        

        
        public CommandMacOff()
        {
            
        }


        #region ICommand 成员

        public bool execute(string commandBody, CommandContext ctx)
        {
            if (commandBody.Equals(CommandName, StringComparison.OrdinalIgnoreCase))
            {
                ctx.IsMacOn = false;
                ctx.ReportMessage("SYS>> Turn OFF MAC");
                return true;
            }
            ctx.ReportMessage("ERR>>{0} is not {2}.", commandBody, CommandName);
            return false;
        }

        public string CommandName
        {
            get { return "MACOFF"; }
        }

        #endregion
    }
}
