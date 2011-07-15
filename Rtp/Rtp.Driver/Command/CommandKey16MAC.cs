using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// ����
    /// KEY16MAC data,key16,init8
    /// PBOC_GetKey16MAC�����貿MAC�㷨����Կ����16byte��8�ֽ��������ΪIV����������
    /// data���ֵ�MAC��init8����Ϊ0.
    /// </summary>
    public class CommandKey16MAC:ICommand
    {
        #region ICommand ��Ա

        public bool execute(string commandBody, CommandContext ctx)
        {
            if (commandBody.StartsWith(CommandName))
            {
                string par = commandBody.Substring(9, commandBody.Length - 10);
                string[] ps = par.Split(',');
                if (ps.Length != 3)
                {
                    ctx.ReportMessage("ERR>>{0} parameters format error.", commandBody);
                    return false;
                }
                ctx.rlen=(byte)Utility.HexStrToByteArray(ps[0], ref ctx.rbuff);
                if (ctx.rlen == 0)
                {
                    ctx.ReportMessage("ERR>>{0} :data={1} length is wrong!",commandBody, ps[0]);
                    return false;
                }
                byte[] data = new byte[ctx.rlen];
                Array.Copy(ctx.rbuff, data, data.Length);

                ctx.rlen = (byte)Utility.HexStrToByteArray(ps[1], ref ctx.rbuff);
                if (ctx.rlen != 16)
                {
                    ctx.ReportMessage("ERR>>{0}:Key16={1} length is wrong!",commandBody, ps[1]);
                    return false;
                }
                byte[] key16 = new byte[16];
                Array.Copy(ctx.rbuff, key16, key16.Length);

                
                ctx.rlen=(byte)Utility.HexStrToByteArray(ps[2],ref ctx.rbuff);
                if(ctx.rlen!=8) 
                {
                    ctx.ReportMessage("ERR>>init8={0} length is wrong!",ps[2]);
                    return false;
                }
                byte[] init8=new byte[8];
                Array.Copy(ctx.rbuff,init8,8);
                if (0 == Utility.PBOC_GetKey16MAC(data, key16, init8, ref ctx.rbuff))
                {
                    ctx.rlen = 8;
                    return true;
                }
                ctx.rlen = 0;
                return false;
            }
            return false;
        }

        public string CommandName
        {
            get { return "KEY16MAC"; }
        }

        #endregion
    }
}
