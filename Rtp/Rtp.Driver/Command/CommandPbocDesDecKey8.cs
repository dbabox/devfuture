using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    class CommandPbocDesDecKey8 : ICommand
    {
        /// <summary>
        /// 函数
        /// 调用PBOC_DesDec_Key8(byte[] data, byte[] key8, ref byte[] dec).
        /// 结果在ctx.rbuff中。
        /// 调用方式：{PBOCDESDECKEY8(data,key8)}
        /// </summary>
        /// <param name="commandBody"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public bool execute(string commandBody, CommandContext ctx)
        {
            int rc = -1;
            #region PBOC_DesEnc_Key8
            //取函数参数 
            //逗号分隔的，先找到逗号
            int lxkhIdx = commandBody.IndexOf('(', 3);
            int dhIdx = commandBody.IndexOf(',', lxkhIdx);//逗号            
            int rxkhIdx = commandBody.IndexOf(')', lxkhIdx);

            string dataStr = commandBody.Substring(lxkhIdx + 1, dhIdx - lxkhIdx - 1);
            rc = Utility.HexStrToByteArray(dataStr, ref ctx.sbuff);
            byte[] data = new byte[rc];
            Array.Copy(ctx.sbuff, data, rc);
            //密钥 8 字节
            string key8Str = commandBody.Substring(dhIdx + 1, rxkhIdx - dhIdx - 1);
            byte[] key8 = new byte[8];
            Utility.HexStrToByteArray(key8Str, ref key8);

            try
            {
                //执行DES加密
                rc = Utility.PBOC_DesDec_Key8(data, key8, ref ctx.rbuff);
                ctx.rlen = (byte)rc;
            }
            catch (Exception ex)
            {
                rc = -1;
                ctx.RecvCtxMsg(ex.Message);
            }
            #endregion
            //至此函数计算完毕，结果保存在ctx的rbuff中.
            return rc >= 8;
        }

        public string CommandName
        {
            get { return "PBOCDESDECKEY8"; }
        }
    }
}
