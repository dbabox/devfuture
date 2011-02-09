using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    public class CommandResetReader:ICommand
    {
        

      

        public CommandResetReader()
        {
            
        }

        #region ICommand ³ÉÔ±

        public bool execute(string commandBody, CommandContext ctx)
        {
            int rc = ctx.Rfid.DeviceReset();
            System.Diagnostics.Trace.TraceInformation("SYS>> RESET READER RC={0}", rc);
            return rc==0;
        }

        public string CommandName
        {
            get { return "RESET READER"; }
        }

        #endregion
    }
}
