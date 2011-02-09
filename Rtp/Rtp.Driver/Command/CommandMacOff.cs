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
            ctx.IsMacOn = false;
            System.Diagnostics.Trace.TraceInformation("SYS>> Turn OFF MAC");
            return true;
        }

        public string CommandName
        {
            get { return "MAC OFF"; }
        }

        #endregion
    }
}
