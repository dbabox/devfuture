using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// 函数
    /// DES加密函数：
    /// {DES(init8,key8)}
    /// </summary>
    public class CommandDes:ICommand
    {
        
 

        /// <summary>
        /// 计算形如DES(01 02 04 ...,04  05 05 ) 的语句
        /// </summary>
        /// <param name="cmdBody_"></param>
        public CommandDes()
        {
            
        }

        #region ICommand 成员

        public bool execute(string commandBody, CommandContext ctx)
        {
            #region DES
            //取函数参数 
            //逗号分隔的，先找到逗号
            int lxkhIdx = commandBody.IndexOf('(', 3);
            int dhIdx = commandBody.IndexOf(',', lxkhIdx);//逗号            
            int rxkhIdx = commandBody.IndexOf(')', lxkhIdx);

            string initNumStr = commandBody.Substring(lxkhIdx + 1, dhIdx - lxkhIdx - 1);
            byte[] init8 = new byte[8];
            Utility.HexStrToByteArray(initNumStr, ref init8);

            //密钥 8 字节
            string key8Str = commandBody.Substring(dhIdx + 1, rxkhIdx - dhIdx - 1);
            byte[] key8 = new byte[8];
            Utility.HexStrToByteArray(key8Str, ref key8);
            int rc = -1;
            try
            {
                //执行DES加密
                rc = Utility.DesBlockEncrypt(key8, init8, System.Security.Cryptography.CipherMode.ECB, ref ctx.rbuff);
                ctx.rlen = 8; //加密结果只取8字节
            }
            catch (Exception ex)
            {
                rc = -1;
                ctx.RecvCtxMsg(ex.Message);
            }
            #endregion
            //至此函数计算完毕，结果保存在ctx的rbuff中.
            return rc>=8;
        }
      
        public string CommandName
        {
            get { return "SYS<DES"; }
        }

        #endregion
    }
}
