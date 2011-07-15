using System;
using System.Collections.Generic;
using System.Text;
using Rtp.Driver.CardIO;

namespace Rtp.Driver.Command
{
    /// <summary>
    ///  DESC系统命令。
    /// 
    /// </summary>
    public class CommandDesc:ICommand
    {
        private ICosDictionary cos;
        public CommandDesc(ICosDictionary cos_)
        {
            cos = cos_;
        }
       
       
        #region ICommand 成员

        public bool execute(string commandBody, CommandContext ctx)
        {

            int coslen = Utility.HexStrToByteArray(commandBody, ref ctx.rbuff);
            if (coslen >= 2) //获取COS描述
            {                 
                ctx.ReportMessage("SYS>> {0}", cos.GetDescription(ctx.rbuff[0], ctx.rbuff[1]));
                return true;
            }
            return false;
        }

        public string CommandName
        {
            get { return "SYS<DESC"; }
        }

       

        #endregion
    }
}
