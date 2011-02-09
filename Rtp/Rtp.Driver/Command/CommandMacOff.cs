using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    public class CommandMacOff:ICommand
    {
        

        
        public CommandMacOff()
        {
            
        }


        #region ICommand ��Ա

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
