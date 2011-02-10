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
            if (commandBody.StartsWith(CommandName))
            {
                //���������������̨
                System.Diagnostics.Trace.TraceInformation("[{0}]",
                    commandBody.Substring(CommandName.Length, commandBody.Length - CommandName.Length));
                return true;
            }
            System.Diagnostics.Trace.TraceError("Print:command format error:{0}", commandBody);
            return false;
        }

        public string CommandName
        {
            get { return "PRINT"; }
        }

        #endregion
    }
}