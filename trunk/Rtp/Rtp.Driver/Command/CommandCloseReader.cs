using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// CLOSE READER 操作。关闭当前读卡器。
    /// </summary>
    public class CommandCloseReader:ICommand
    {
       
        
        public CommandCloseReader()
        {
            
        }

        #region ICommand 成员

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
