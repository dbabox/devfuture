using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// ��Trace����̨����ı���
    /// </summary>
    class CommandPrint:ICommand
    {
        #region ICommand ��Ա

        public bool execute(string commandBody, CommandContext ctx)
        {
            //���������������̨
            ctx.ReportMessage(commandBody);
            return true;
        }

        public string CommandName
        {
            get { return "SYS<PRINT"; }
        }

        #endregion
    }
}
