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
        
 

        public CommandMacOn()
        {
            
        }

        #region ICommand ��Ա

        public bool execute(string commandBody, CommandContext ctx)
        {
            ctx.IsMacOn = true;
            ctx.ReportMessage("SYS>> Turn ON MAC");
            return true;
        }

        public string CommandName
        {
            get { return "MAC ON"; }
        }

        #endregion
    }
}
