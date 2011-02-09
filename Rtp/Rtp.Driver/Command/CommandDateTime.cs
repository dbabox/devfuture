using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// {DATETIME}�������������commandBody�Ѿ�ȥ���˿���š�
    /// </summary>
    class CommandDateTime:ICommand
    {
        #region ICommand ��Ա

        public bool execute(string commandBody, CommandContext ctx)
        {
            string resultStr = DateTime.Now.ToString("yyyyMMddHHmmss");
            ctx.rlen = (byte)Utility.HexStrToByteArray(resultStr, ref ctx.rbuff);
            return true;
        }

        public string CommandName
        {
            get { return "DATETIME"; }
        }

        #endregion
    }
}
