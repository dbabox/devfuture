using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// 3DES加密函数。{TripDES(data,KEY16) }
    /// 注意：函数只能在{} 块中执行。
    /// </summary>
    public class CommandTripDes:ICommand
    {     
        /// <summary>
        /// 函数.
        /// 计算形如TripDES(data,KEY16) 的语句，其中KEY16为16字节密钥；data为8的倍数长度字节的明文。
        /// </summary>
        /// <param name="cmdBody_"></param>
        public CommandTripDes()
        {
            
        }
        #region ICommand 成员

        public bool execute(string input, CommandContext ctx)
        {
            #region 3DES
            //取函数参数 
            //逗号分隔的，先找到逗号
            int lxkhIdx = input.IndexOf('(', 3);
            int dhIdx = input.IndexOf(',', lxkhIdx);//逗号            
            int rxkhIdx = input.IndexOf(')', dhIdx);

            string datastr =input.Substring(lxkhIdx + 1, dhIdx - lxkhIdx - 1);
            string key16Str = input.Substring(dhIdx + 1, rxkhIdx - dhIdx - 1);

            ctx.rlen = (byte)Utility.HexStrToByteArray(datastr, ref ctx.rbuff);
            byte[] data = new byte[ctx.rlen];
            Array.Copy(ctx.rbuff, data, data.Length);
            Array.Clear(ctx.rbuff, 0, ctx.rbuff.Length);
            //密钥 16 字节            
            byte[] key16 = new byte[16];
            Utility.HexStrToByteArray(key16Str, ref key16);
            int rc = -1;
            try
            {
                //执行DES加密
                rc=Utility.TripDesBlockEncrypt(key16, data, System.Security.Cryptography.CipherMode.ECB, ref ctx.rbuff);
                ctx.rlen = (byte)data.Length;
            }
            catch (Exception ex)
            {
                rc = -1;
                ctx.ReportMessage(ex.Message);
            }

            #endregion
            return rc>=0;
        }

        public string CommandName
        {
            get { return "SYS<TRIPDES"; }
        }

        #endregion
    }
}
