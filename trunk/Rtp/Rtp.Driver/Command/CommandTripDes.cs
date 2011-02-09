using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    public class CommandTripDes:ICommand
    {     
        /// <summary>
        /// ��������TripDES(data,KEY16) ����䣬����KEY16Ϊ16�ֽ���Կ��dataΪ8�ı��������ֽڵ����ġ�
        /// </summary>
        /// <param name="cmdBody_"></param>
        public CommandTripDes()
        {
            
        }
        #region ICommand ��Ա

        public bool execute(string commandBody, CommandContext ctx)
        {
            #region 3DES
            //ȡ�������� 
            //���ŷָ��ģ����ҵ�����
            int lxkhIdx = commandBody.IndexOf('(', 3);
            int dhIdx = commandBody.IndexOf(',', lxkhIdx);//����            
            int rxkhIdx = commandBody.IndexOf(')', dhIdx);

            string datastr =commandBody.Substring(lxkhIdx + 1, dhIdx - lxkhIdx - 1);
            string key16Str = commandBody.Substring(dhIdx + 1, rxkhIdx - dhIdx - 1);

            ctx.rlen = (byte)Utility.HexStrToByteArray(datastr, ref ctx.rbuff);
            byte[] data = new byte[ctx.rlen];
            Array.Copy(ctx.rbuff, data, data.Length);
            Array.Clear(ctx.rbuff, 0, ctx.rbuff.Length);
            //��Կ 16 �ֽ�            
            byte[] key16 = new byte[16];
            Utility.HexStrToByteArray(key16Str, ref key16);          
            //ִ��DES����
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
