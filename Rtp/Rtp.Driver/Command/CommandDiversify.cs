using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// 密钥分散函数：
    /// {DIVERSIFY(KEY16,SEED) }
    /// 结果保存到ctx的rbuff中。
    /// </summary>
    public class CommandDiversify:ICommand
    {
        

   
        /// <summary>
        /// 函数
        /// 计算形如Diversify($KM,$SEED)的分散函数。结果保存到ctx的rbuff中。 
        /// </summary>
        /// <param name="cmdBody_"></param>
        public CommandDiversify()
        {
            
        }
        #region ICommand 成员

        public bool execute(string commandBody, CommandContext ctx)
        {
            #region //{Diversify($ACK,$SEED)}
            //取函数参数 
            //逗号分隔的，先找到逗号
            int lxkhIdx = commandBody.IndexOf('(', 3);
            int dhIdx = commandBody.IndexOf(',', lxkhIdx);//逗号            
            int rxkhIdx = commandBody.IndexOf(')', lxkhIdx);


        
            string ackStr = commandBody.Substring(lxkhIdx + 1, dhIdx - lxkhIdx - 1);
            byte[] km = new byte[16];
            Utility.HexStrToByteArray(ackStr, ref km);//获得分散密钥
            //分散因子
            string seedStr = commandBody.Substring(dhIdx + 1, rxkhIdx - dhIdx - 1);
            ctx.slen = (byte)Utility.HexStrToByteArray(seedStr, ref ctx.sbuff);

            byte[] seed = new byte[ctx.slen];
            Array.Copy(ctx.sbuff, seed, ctx.slen);
            int rc = -1;
            try
            {
                rc=Utility.PBOC_Diversify64(km, seed, ref ctx.rbuff);//得到16字节的分散结果
                ctx.rlen = 16;
            }
            catch (Exception ex)
            {
                ctx.ReportMessage(ex.Message);
                rc = -1;
            }
             
            #endregion
            return rc==0;
        }

        public string CommandName
        {
            get { return "DIVERSIFY"; }
        }

        #endregion
    }
}
