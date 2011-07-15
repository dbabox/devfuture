using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// 函数
    /// {Date}函数，输入参数commandBody已经去除了块符号。
    /// </summary>
    class CommandDate:ICommand
    {
        #region ICommand 成员

        public bool execute(string commandBody, CommandContext ctx)
        {
            string resultStr = DateTime.Now.ToString("yyyyMMdd");
            ctx.rlen = (byte)Utility.HexStrToByteArray(resultStr, ref ctx.rbuff);
            return true;       
        }

        public string CommandName
        {
            get { return "SYS<DATE"; }
        }

        #endregion
    }
}
