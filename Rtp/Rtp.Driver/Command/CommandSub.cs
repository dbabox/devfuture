using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// SUB GV,##
    /// GV是字节数组表示。当字节数组表达为整数时，要求 byte[0]=00,byte[1]=04这样的数组表示的值是10进制整数4.
    /// 即，当字节数组表示一个整数时，采用低字节高位的规则。
    ///  
    /// 其中，##为16进制数，不带0x
    /// </summary>
    class CommandSub:ICommand
    {
        #region ICommand 成员

        public bool execute(string commandBody, CommandContext ctx)
        {
            if (commandBody.StartsWith(CommandName))
            {
                string par = commandBody.Substring(CommandName.Length, commandBody.Length - CommandName.Length);
                string[] pars = par.Split(',');
                if (pars.Length != 2)
                {
                    ctx.ReportMessage("ERR>>CommandSub:{0} format error.", commandBody);
                    return false;
                }
                string gvkey = pars[0].Trim();
                if (!ctx.GVDIC.ContainsKey(gvkey))
                {
                    ctx.ReportMessage("ERR>>CommandSub:{0} format error.GV parameter is not valid.", commandBody);
                    return false;
                }
                ctx.rlen = (byte)Utility.HexStrToByteArray(ctx.GVDIC[gvkey], ref ctx.rbuff);
                if (ctx.rlen > 5 || ctx.rlen == 0)
                {
                    ctx.ReportMessage("ERR>>CommandSub:{0} GV={1} to big.", commandBody, ctx.GVDIC[gvkey]);
                    return false;
                }                 
                int toAdd = 0;                
                for (int i = 0; i < ctx.rlen; i++)
                {
                    toAdd <<= 8;  
                    toAdd |= ctx.rbuff[i];
                   
                }

                //16进制数值
                string addstr = pars[1].Trim();
                int addvalue = 0;
                if (!int.TryParse(addstr, System.Globalization.NumberStyles.HexNumber, null, out addvalue))
                {
                    ctx.ReportMessage("ERR>>CommandSub:{0} format error.", commandBody);
                    return false;
                }              
                toAdd -= addvalue; //减法
                for (int i = ctx.rlen - 1; i >= 0; i--)
                {
                    ctx.rbuff[i] = (byte)(toAdd & 0x000000FF);
                    toAdd >>= 8;
                }
                ctx.GVDIC[gvkey] = Utility.ByteArrayToHexStr(ctx.rbuff,ctx.rlen);
                ctx.ReportMessage("SYS>>{0}={1}", gvkey, ctx.GVDIC[gvkey]);
                return true;

            }
            return false;
        }

        public string CommandName
        {
            get { return "SUB"; }
        }

        #endregion
    }
}
