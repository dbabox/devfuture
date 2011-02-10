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
        

         
        public CommandOpenReader()
        {
            
        }

        #region ICommand ��Ա

        public bool execute(string commandBody, CommandContext ctx)
        {
            int rc = ctx.Rfid.Open();
            ctx.ReportMessage("SYS>> OPEN READER RC={0}", rc);
            return rc > 0;
        }

        public string CommandName
        {
            get { return "OPEN READER"; }
        }

        #endregion
    }
}
