using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// 组合操作符/。它用于执行连续几个同类指令。如：00 A4 00 02 3F 00/10 01/00 19
    /// 要求后续组合的部分可完全替换前面的部分，且替换的部分要相等。
    /// 该操作符以及废弃，默认的，允许cos指令使用/替换串联；2011-2-15
    /// </summary>
    public class CommandCompose:ICommand
    {
        

        
        public CommandCompose()
        {
            
        }

        #region ICommand 成员

        public bool execute(string commandBody, CommandContext ctx)
        {
            if (commandBody.Contains("/")) //组合命令，不允许有MAC
            {
                
            }
            return false;
        }

        public string CommandName
        {
            get { return "/"; }
        }

        #endregion
    }
}
