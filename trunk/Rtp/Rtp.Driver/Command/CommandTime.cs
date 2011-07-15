using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// {Time}�������������commandBody�Ѿ�ȥ���˿���š�
    /// </summary>
    class CommandTime:ICommand
    {
        #region ICommand ��Ա

        public bool execute(string input, CommandContext ctx)
        {
            string resultStr = DateTime.Now.ToString("HHmmss");
            ctx.rlen = (byte)Utility.HexStrToByteArray(resultStr, ref ctx.rbuff);
            return true;
        }

        public string CommandName
        {
            get { return "SYS<TIME"; }
        }

        #endregion
    }
}
