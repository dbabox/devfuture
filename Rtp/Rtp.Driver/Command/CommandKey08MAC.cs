using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// KEY08MAC (data,key8,init8)
    /// PBOC_GetKey8MAC，建设部MAC算法，密钥长度8byte，8字节随机数作为IV，若仅计算
    /// data部分的MAC，init8可以为0.
    /// </summary>
    class CommandKey08MAC:ICommand
    {
        #region ICommand 成员

        public bool execute(string commandBody, CommandContext ctx)
        {
            if (commandBody.StartsWith(CommandName))
            {
                string par = commandBody.Substring(9, commandBody.Length - 10);
                string[] ps = par.Split(',');
                if (ps.Length != 3)
                {
                    ctx.ReportMessage("{0} parameters format error.", commandBody);
                    return false;
                }
                ctx.rlen = (byte)Utility.HexStrToByteArray(ps[0], ref ctx.rbuff);
                if (ctx.rlen == 0)
                {
                    ctx.ReportMessage("data={0} length is wrong!", ps[0]);
                    return false;
                }
                byte[] data = new byte[ctx.rlen];
                Array.Copy(ctx.rbuff, data, data.Length);

                ctx.rlen = (byte)Utility.HexStrToByteArray(ps[1], ref ctx.rbuff);
                if (ctx.rlen != 8)
                {
                    ctx.ReportMessage("Key8={0} length is wrong!", ps[1]);
                    return false;
                }
                byte[] key8 = new byte[8];
                Array.Copy(ctx.rbuff, key8, key8.Length);


                ctx.rlen = (byte)Utility.HexStrToByteArray(ps[2], ref ctx.rbuff);
                if (ctx.rlen != 8)
                {
                    ctx.ReportMessage("init8={0} length is wrong!", ps[2]);
                    return false;
                }
                byte[] init8 = new byte[8];
                Array.Copy(ctx.rbuff, init8, 8);
                if (0 == Utility.PBOC_GetKey8MAC(data, key8, init8, ref ctx.rbuff))
                {
                    ctx.rlen = 8;
                    return true;
                }
                ctx.rlen = 0;
                return false;
            }
            return false;
        }

        public string CommandName
        {
            get { return "KEY08MAC"; }
        }

        #endregion
    }
}
