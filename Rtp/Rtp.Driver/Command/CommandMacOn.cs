using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    public class CommandMacOn:ICommand
    {
        
 

        public CommandMacOn()
        {
            
        }

        #region ICommand ³ÉÔ±

        public bool execute(string commandBody, CommandContext ctx)
        {
            ctx.IsMacOn = true;
            System.Diagnostics.Trace.TraceInformation("SYS>> Turn ON MAC");
            return true;
        }

        public string CommandName
        {
            get { return "MAC ON"; }
        }

        #endregion
    }
}
