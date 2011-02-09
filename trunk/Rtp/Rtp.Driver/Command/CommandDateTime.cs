using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// {DATETIME}函数，输入参数commandBody已经去除了块符号。
    /// </summary>
    class CommandDateTime:ICommand
    {
        #region ICommand 成员

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
