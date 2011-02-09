using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// {Date}�������������commandBody�Ѿ�ȥ���˿���š�
    /// </summary>
    class CommandDate:ICommand
    {
        #region ICommand ��Ա

        public bool execute(string commandBody, CommandContext ctx)
        {
            if (commandBody.Trim() == "DATE")
            {
                string resultStr = DateTime.Now.ToString("yyyyMMdd");
                ctx.rlen = (byte)Utility.HexStrToByteArray(resultStr, ref ctx.rbuff);
                return true;                
            }
            return false;
        }

        public string CommandName
        {
            get { return "DATE"; }
        }

        #endregion
    }
}
