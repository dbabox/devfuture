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
       
        
        public CommandCloseReader()
        {
            
        }

        #region ICommand ��Ա

        public bool execute(string commandBody, CommandContext ctx)
        {
            int rc = ctx.Rfid.Close();
            ctx.ReportMessage("SYS>> CLOSE READER RC={0}", rc);            
            return rc==0;
        }

        public string CommandName
        {
            get { return "CLOSE READER"; }
        }

        #endregion
    }
}