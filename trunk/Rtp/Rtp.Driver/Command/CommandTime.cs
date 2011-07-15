using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// {Time}函数，输入参数commandBody已经去除了块符号。
    /// </summary>
    class CommandTime:ICommand
    {
        #region ICommand 成员

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
