using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// 系统命令
    /// 打开读卡器OPEN READER操作。
    /// </summary>
    public class CommandOpenReader:ICommand
    {
        

   

        #region ICommand 成员

        public bool execute(string commandBody, CommandContext ctx)
        {
            if (commandBody.Equals(CommandName, StringComparison.OrdinalIgnoreCase))
            {
                int rc = ctx.Rfid.Open();
                ctx.ReportMessage("SYS>> OPEN READER RC={0}", rc);
                return rc > 0;
            }
            ctx.ReportMessage("ERR>>{0} is not {1}.", commandBody, CommandName);
            return false;
        }

        public string CommandName
        {
            get { return "SYS<OPENREADER"; }
        }

        #endregion
    }
}
