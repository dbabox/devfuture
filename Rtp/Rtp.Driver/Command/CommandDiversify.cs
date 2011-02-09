using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// ��Կ��ɢ������
    /// {DIVERSIFY(KEY16,SEED) }
    /// ������浽ctx��rbuff�С�
    /// </summary>
    public class CommandDiversify:ICommand
    {
        

   
        /// <summary>
          /// ��������Diversify($KM,$SEED)�ķ�ɢ������������浽ctx��rbuff�С� 
        /// </summary>
        /// <param name="cmdBody_"></param>
        public CommandDiversify()
        {
            
        }
        #region ICommand ��Ա

        public bool execute(string commandBody, CommandContext ctx)
        {
            #region //{Diversify($ACK,$SEED)}
            //ȡ�������� 
            //���ŷָ��ģ����ҵ�����
            int lxkhIdx = commandBody.IndexOf('(', 3);
            int dhIdx = commandBody.IndexOf(',', lxkhIdx);//����            
            int rxkhIdx = commandBody.IndexOf(')', lxkhIdx);


        
            string ackStr = commandBody.Substring(lxkhIdx + 1, dhIdx - lxkhIdx - 1);
            byte[] km = new byte[16];
            Utility.HexStrToByteArray(ackStr, ref km);//��÷�ɢ��Կ
            //��ɢ����
            string seedStr = commandBody.Substring(dhIdx + 1, rxkhIdx - dhIdx - 1);
            ctx.slen = (byte)Utility.HexStrToByteArray(seedStr, ref ctx.sbuff);

            byte[] seed = new byte[ctx.slen];
            Array.Copy(ctx.sbuff, seed, ctx.slen);
            Utility.PBOC_Diversify64(km, seed, ref ctx.rbuff);//�õ�16�ֽڵķ�ɢ���
            ctx.rlen = 16;
             
            #endregion
            return true;
        }

        public string CommandName
        {
            get { return "DIVERSIFY"; }
        }

        #endregion
    }
}
