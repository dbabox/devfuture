using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// �ر������MAC�Զ����㹦�ܡ���COSָ����Ҫ��·����ʱʹ�á�
    /// </summary>
    public class CommandMacOff:ICommand
    {
        

        
        public CommandMacOff()
        {
            
        }


        #region ICommand ��Ա

        public bool execute(string commandBody, CommandContext ctx)
        {
            if (commandBody.Equals(CommandName, StringComparison.OrdinalIgnoreCase))
            {
                ctx.IsMacOn = false;
                ctx.ReportMessage("SYS>> Turn OFF MAC");
                return true;
            }
            ctx.ReportMessage("ERR>>{0} is not {2}.", commandBody, CommandName);
            return false;
        }

        public string CommandName
        {
            get { return "MACOFF"; }
        }

        #endregion
    }
}
