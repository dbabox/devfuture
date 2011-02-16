using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// ����COSָ����·������
    /// </summary>
    public class CommandMacOn:ICommand
    {
        
  

        #region ICommand ��Ա

        public bool execute(string commandBody, CommandContext ctx)
        {
            if (commandBody.Equals(CommandName, StringComparison.OrdinalIgnoreCase))
            {
                ctx.IsMacOn = true;
                ctx.ReportMessage("SYS>> Turn ON MAC");
                return true;
            }
            ctx.ReportMessage("ERR>>{0} is not {2}.", commandBody, CommandName);
            return false;
        }

        public string CommandName
        {
            get { return "MACON"; }
        }

        #endregion
    }
}
