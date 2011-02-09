using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// 项Trace控制台输出文本。
    /// </summary>
    class CommandPrint:ICommand
    {
        #region ICommand 成员

        public bool execute(string commandBody, CommandContext ctx)
        {
            if (commandBody.StartsWith(CommandName))
            {
                //将内容输出到控制台
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
