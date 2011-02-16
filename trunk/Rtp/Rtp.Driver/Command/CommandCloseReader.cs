using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// ϵͳ�޲�������.
    /// CLOSE READER �������رյ�ǰ��������
    /// </summary>
    public class CommandCloseReader:ICommand
    {
  
        #region ICommand ��Ա

        public bool execute(string commandBody, CommandContext ctx)
        {
            if (commandBody.Equals(CommandName, StringComparison.OrdinalIgnoreCase))
            {
                int rc = ctx.Rfid.Close();
                ctx.ReportMessage("SYS>> CLOSE READER RC={0}", rc);
                return rc == 0;
            }
            ctx.ReportMessage("ERR>>{0} is not {2}.", commandBody, CommandName);
            return false;
        }

        public string CommandName
        {
            get { return "CLOSEREADER"; }
        }

        #endregion
    }
}
