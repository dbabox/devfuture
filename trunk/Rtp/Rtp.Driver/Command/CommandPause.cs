using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// 系统命令
    /// 暂停命令
    /// </summary>
    public class CommandPause:ICommand
    {

        //private bool paused = false;
      

        public CommandPause()
        {
            
        }


        #region ICommand 成员

        public bool execute(string commandBody, CommandContext ctx)
        {
            return false;
        }

        public string CommandName
        {
            get { return "SYS<PAUSE"; }
        }

        #endregion
    }
}
