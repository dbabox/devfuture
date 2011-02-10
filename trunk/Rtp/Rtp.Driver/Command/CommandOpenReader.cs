using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// 打开读卡器OPEN READER操作。
    /// </summary>
    public class CommandOpenReader:ICommand
    {
        

         
        public CommandOpenReader()
        {
            
        }

        #region ICommand 成员

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
