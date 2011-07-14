using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// �򿪶�����OPEN READER������
    /// </summary>
    public class CommandOpenReader:ICommand
    {
        

   

        #region ICommand ��Ա

        public bool execute(string commandBody, CommandContext ctx)
        {
            if (commandBody.Equals(CommandName, StringComparison.OrdinalIgnoreCase))
            {
                int rc = ctx.Rfid.Open();
                ctx.ReportMessage("SYS>> OPEN READER RC={0}", rc);
                return rc > 0;
            }
            ctx.ReportMessage("ERR>>{0} is not {2}.", commandBody, CommandName);
            return false;
        }

        public string CommandName
        {
            get { return "OPENREADER"; }
        }

        #endregion
    }
}