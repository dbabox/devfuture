using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// ϵͳ����
    /// ��ͣ����
    /// </summary>
    public class CommandPause:ICommand
    {
        

      

        public CommandPause()
        {
            
        }


        #region ICommand ��Ա

        public bool execute(string commandBody, CommandContext ctx)
        {
            ctx.ReportMessage("Press any key to continue the script.");
            Console.Read();

            //TODO:GUI����Ӧ�в�ͬ��ʵ��

            return true; 
        }

        public string CommandName
        {
            get { return "SYS<PAUSE"; }
        }

        #endregion
    }
}
