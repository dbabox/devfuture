using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// ϵͳ����
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
            if (!commandBody.Equals(CommandName, StringComparison.OrdinalIgnoreCase))
            {
                ctx.ReportMessage("ERR>>{0} is not {2}.", commandBody, CommandName);
                return false;
            }

            int rc = ctx.Rfid.DeviceReset();
            ctx.ReportMessage("SYS>> RESET READER RC={0}", rc);
            return rc==0;
        }

        public string CommandName
        {
            get { return "SYS<RESETREADER"; }
        }

        #endregion
    }
}
