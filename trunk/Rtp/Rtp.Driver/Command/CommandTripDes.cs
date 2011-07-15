using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// 3DES���ܺ�����{TripDES(data,KEY16) }
    /// ע�⣺����ֻ����{} ����ִ�С�
    /// </summary>
    public class CommandTripDes:ICommand
    {     
        /// <summary>
        /// ����.
        /// ��������TripDES(data,KEY16) ����䣬����KEY16Ϊ16�ֽ���Կ��dataΪ8�ı��������ֽڵ����ġ�
        /// </summary>
        /// <param name="cmdBody_"></param>
        public CommandTripDes()
        {
            
        }
        #region ICommand ��Ա

        public bool execute(string input, CommandContext ctx)
        {
            #region 3DES
            //ȡ�������� 
            //���ŷָ��ģ����ҵ�����
            int lxkhIdx = input.IndexOf('(', 3);
            int dhIdx = input.IndexOf(',', lxkhIdx);//����            
            int rxkhIdx = input.IndexOf(')', dhIdx);

            string datastr =input.Substring(lxkhIdx + 1, dhIdx - lxkhIdx - 1);
            string key16Str = input.Substring(dhIdx + 1, rxkhIdx - dhIdx - 1);

            ctx.rlen = (byte)Utility.HexStrToByteArray(datastr, ref ctx.rbuff);
            byte[] data = new byte[ctx.rlen];
            Array.Copy(ctx.rbuff, data, data.Length);
            Array.Clear(ctx.rbuff, 0, ctx.rbuff.Length);
            //��Կ 16 �ֽ�            
            byte[] key16 = new byte[16];
            Utility.HexStrToByteArray(key16Str, ref key16);
            int rc = -1;
            try
            {
                //ִ��DES����
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
