using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    public class CommandTripDes:ICommand
    {     
        /// <summary>
        /// 计算形如TripDES(data,KEY16) 的语句，其中KEY16为16字节密钥；data为8的倍数长度字节的明文。
        /// </summary>
        /// <param name="cmdBody_"></param>
        public CommandTripDes()
        {
            
        }
        #region ICommand 成员

        public bool execute(string commandBody, CommandContext ctx)
        {
            #region 3DES
            //取函数参数 
            //逗号分隔的，先找到逗号
            int lxkhIdx = commandBody.IndexOf('(', 3);
            int dhIdx = commandBody.IndexOf(',', lxkhIdx);//逗号            
            int rxkhIdx = commandBody.IndexOf(')', dhIdx);

            string datastr =commandBody.Substring(lxkhIdx + 1, dhIdx - lxkhIdx - 1);
            string key16Str = commandBody.Substring(dhIdx + 1, rxkhIdx - dhIdx - 1);

            ctx.rlen = (byte)Utility.HexStrToByteArray(datastr, ref ctx.rbuff);
            byte[] data = new byte[ctx.rlen];
            Array.Copy(ctx.rbuff, data, data.Length);
            Array.Clear(ctx.rbuff, 0, ctx.rbuff.Length);
            //密钥 16 字节            
            byte[] key16 = new byte[16];
            Utility.HexStrToByteArray(key16Str, ref key16);          
            //执行DES加密
            Utility.TripDesBlockEncrypt(key16, data, System.Security.Cryptography.CipherMode.ECB, ref ctx.rbuff);
            ctx.rlen = (byte)data.Length; 
            #endregion
            return true;
        }

        public string CommandName
        {
            get { return "TRIPDES"; }
        }

        #endregion
    }
}
