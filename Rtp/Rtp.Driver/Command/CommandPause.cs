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
        

      

        public CommandPause()
        {
            
        }


        #region ICommand 成员

        public bool execute(string commandBody, CommandContext ctx)
        {
            ctx.ReportMessage("Press any key to continue the script.");
            Console.Read();

            //TODO:GUI程序应有不同的实现

            return true; 
        }

        public string CommandName
        {
            get { return "SYS<PAUSE"; }
        }

        #endregion
    }
}
