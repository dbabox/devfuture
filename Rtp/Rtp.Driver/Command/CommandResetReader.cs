using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// ���ö������豸��
    /// </summary>
    public class CommandResetReader:ICommand
    {
        

      

        public CommandResetReader()
        {
            
        }

        #region ICommand ��Ա

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
