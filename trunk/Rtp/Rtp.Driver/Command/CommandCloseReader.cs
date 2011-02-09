using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    public class CommandCloseReader:ICommand
    {
       

        public CommandCloseReader()
        {
            
        }

        #region ICommand ³ÉÔ±

        public bool execute(string commandBody, CommandContext ctx)
        {
            int rc = ctx.Rfid.Close();
            System.Diagnostics.Trace.TraceInformation("SYS>> CLOSE READER RC={0}", rc);
            
            return rc==0;
        }

        public string CommandName
        {
            get { return "CLOSE READER"; }
        }

        #endregion
    }
}
