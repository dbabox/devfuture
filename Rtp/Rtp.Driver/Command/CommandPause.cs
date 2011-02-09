using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
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
            Console.WriteLine("Press any key to continue the script.");
            Console.Read();

            //TODO:GUI程序应有不同的实现

            return true; 
        }

        public string CommandName
        {
            get { return "PAUSE"; }
        }

        #endregion
    }
}
