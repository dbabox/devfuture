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

        //private bool paused = false;
      

        public CommandPause()
        {
            
        }


        #region ICommand ��Ա

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
