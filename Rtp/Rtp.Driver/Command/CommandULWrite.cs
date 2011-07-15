using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// 向UL卡写数据
    /// </summary>
    class CommandULWrite:ICommand
    {
        public bool execute(string input, CommandContext ctx)
        {
            int rc = -1;
            string par = Utility.GetSubStringBetweenChars(input, '(', ')').Trim().ToUpper();
            //commandBody格式 04,01 02 03 04
            Array.Clear(ctx.sbuff, 0, ctx.sbuff.Length);
            string[] args = par.Split(',');
            if (args.Length != 2)
            {
                ctx.ReportMessage("ERR>>命令格式错误:{0}", args);
                return false;
            }
            byte addr = Convert.ToByte(args[0], 16);
            ctx.slen=(byte)Utility.HexStrToByteArray(args[1], ref ctx.sbuff);
            if (ctx.slen > 0)
            {
                rc = ctx.Rfid.UL_write(addr, ctx.sbuff);
            }
            else
            {
                ctx.ReportMessage("ERR>>数据格式错误{0}", args[1]);
                rc = -1;
            }
            if (rc == 0) 
                return true;
            else
            {
                ctx.ReportMessage("ERR>>UL_write 失败:{0}", rc);
                return false;
            }


        }

        public string CommandName
        {
            get { return "UL<WRITE"; }
        }
    }
}
