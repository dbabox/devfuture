using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    public class CommandOpenReader:ICommand
    {
        

         
        public CommandOpenReader()
        {
            
        }

        #region ICommand ³ÉÔ±

        public bool execute(string commandBody, CommandContext ctx)
        {
            int rc = ctx.Rfid.Open();
            System.Diagnostics.Trace.TraceInformation("SYS>> OPEN READER RC={0}", rc);
            return rc > 0;
        }

        public string CommandName
        {
            get { return "OPEN READER"; }
        }

        #endregion
    }
}
