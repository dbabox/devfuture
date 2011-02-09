using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// 实现BUFF指令
    /// BUFF offset,Length,GV，获取BUFF的值，从offset开始，长度为Length字节，放到GV变量中
    /// BUFF offset,$GV 设置BUFF的值,从offset开始；
    /// </summary>
    class CommandBuff:ICommand
    {
        const int BUFF_LEN = 256;
        private byte[] buff = new byte[BUFF_LEN];

        #region ICommand 成员

        public bool execute(string commandBody, CommandContext ctx)
        {
            if (commandBody.StartsWith(CommandName))
            {
                string par = commandBody.Substring(5, commandBody.Length - 5);
                string[] args = par.Split(',');
                if (args.Length == 3)
                {
                    System.Diagnostics.Trace.TraceInformation("Get buff vallue to GV:{0}", commandBody);
                    int offset = 0;
                    int length = 0;
                    if (int.TryParse(args[0], System.Globalization.NumberStyles.HexNumber, null, out offset)
                        && int.TryParse(args[1], System.Globalization.NumberStyles.HexNumber, null, out length))
                    {
                        string gvkey = args[2].Trim().ToUpper();

                        if (ctx.GVDIC.ContainsKey(gvkey))
                        {
                            ctx.GVDIC[gvkey] = Utility.ByteArrayToHexStr(buff, offset, length, "");                            
                        }
                        else
                        {
                            ctx.GVDIC.Add(gvkey, Utility.ByteArrayToHexStr(buff, offset, length, ""));
                        }
                        System.Diagnostics.Trace.TraceInformation("{0}={1}", gvkey, ctx.GVDIC[gvkey]);
                        return true;
                    }
                   
                }
                else if (args.Length == 2)
                {
                    System.Diagnostics.Trace.TraceInformation("set buff vallue :{0}", commandBody);
                    int offset = 0;
                    if (int.TryParse(args[0], System.Globalization.NumberStyles.HexNumber, null, out offset)
                        && (ctx.rlen = (byte)Utility.HexStrToByteArray(args[1], ref ctx.rbuff)) > 0)
                    {
                        if ((offset + ctx.rlen) > BUFF_LEN)
                        {
                            System.Diagnostics.Trace.TraceError("CommandBuff:command format error: value too long! {0}", commandBody);
                            return false;
                        }
                        Array.Copy(ctx.rbuff, 0, buff, offset, ctx.rlen);
                        System.Diagnostics.Trace.TraceInformation("BUFF={0}", Utility.ByteArrayToHexStr(buff, offset + ctx.rlen));
                        return true;
                    }
                }              
             
            }
            System.Diagnostics.Trace.TraceError("CommandBuff:command format error:{0}", commandBody);
            return false;
        }

        public string CommandName
        {
            get { return "BUFF"; }
        }

        #endregion
    }
}
