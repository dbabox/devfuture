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
            //将内容输出到控制台
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
