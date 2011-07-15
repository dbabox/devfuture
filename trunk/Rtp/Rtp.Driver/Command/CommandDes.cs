using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// ����
    /// DES���ܺ�����
    /// {DES(init8,key8)}
    /// </summary>
    public class CommandDes:ICommand
    {
        
 

        /// <summary>
        /// ��������DES(01 02 04 ...,04  05 05 ) �����
        /// </summary>
        /// <param name="cmdBody_"></param>
        public CommandDes()
        {
            
        }

        #region ICommand ��Ա

        public bool execute(string commandBody, CommandContext ctx)
        {
            #region DES
            //ȡ�������� 
            //���ŷָ��ģ����ҵ�����
            int lxkhIdx = commandBody.IndexOf('(', 3);
            int dhIdx = commandBody.IndexOf(',', lxkhIdx);//����            
            int rxkhIdx = commandBody.IndexOf(')', lxkhIdx);

            string initNumStr = commandBody.Substring(lxkhIdx + 1, dhIdx - lxkhIdx - 1);
            byte[] init8 = new byte[8];
            Utility.HexStrToByteArray(initNumStr, ref init8);

            //��Կ 8 �ֽ�
            string key8Str = commandBody.Substring(dhIdx + 1, rxkhIdx - dhIdx - 1);
            byte[] key8 = new byte[8];
            Utility.HexStrToByteArray(key8Str, ref key8);
            int rc = -1;
            try
            {
                //ִ��DES����
                rc = Utility.DesBlockEncrypt(key8, init8, System.Security.Cryptography.CipherMode.ECB, ref ctx.rbuff);
                ctx.rlen = 8; //���ܽ��ֻȡ8�ֽ�
            }
            catch (Exception ex)
            {
                rc = -1;
                ctx.RecvCtxMsg(ex.Message);
            }
            #endregion
            //���˺���������ϣ����������ctx��rbuff��.
            return rc>=8;
        }
      
        public string CommandName
        {
            get { return "SYS<DES"; }
        }

        #endregion
    }
}
