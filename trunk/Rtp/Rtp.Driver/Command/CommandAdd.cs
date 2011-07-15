using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// ϵͳ����
    /// ϵͳ������������ Add GV,xx 
    /// ��һ��������ȫ�ֱ��������֣��ڶ�����16��������ֵ��
    /// eg: Add CARD_BLANCE,BE
    /// ���ֽ������ʾһ������ʱ�����õ��ֽڸ�λ�Ĺ���
    /// </summary>
    public class CommandAdd:ICommand
    {
        #region ICommand ��Ա

        public bool execute(string commandBody, CommandContext ctx)
        {
            if (commandBody.StartsWith(CommandName))
            {
                string par = commandBody.Substring(CommandName.Length, commandBody.Length - CommandName.Length);
                string[] pars = par.Split(',');
                if (pars.Length != 2)
                {
                    ctx.ReportMessage("ERR>>CommandAdd:{0} format error.", commandBody);
                    return false;
                }
                string gvkey = pars[0].Trim().ToUpper();
                if (!ctx.GVDIC.ContainsKey(gvkey))
                {
                    ctx.ReportMessage("ERR>>CommandAdd:{0} format error.GV parameter is not valid.", commandBody);
                    return false;
                }
                ctx.rlen = (byte)Utility.HexStrToByteArray(ctx.GVDIC[gvkey], ref ctx.rbuff);
                if (ctx.rlen > 5 || ctx.rlen == 0)
                {
                    ctx.ReportMessage("ERR>>CommandAdd:{0} GV={1} to big.", commandBody, ctx.GVDIC[gvkey]);
                    return false;
                }
                int toAdd = 0;
                for (int i = 0; i < ctx.rlen; i++)
                {
                    toAdd <<= 8;   
                    toAdd |= ctx.rbuff[i];                                     
                }

                //16������ֵ
                string addstr = pars[1].Trim();
                int addvalue = 0;
                if (!int.TryParse(addstr, System.Globalization.NumberStyles.HexNumber, null, out addvalue))
                {
                    ctx.ReportMessage("ERR>>CommandAdd:{0} format error.", commandBody);
                    return false;
                }
                toAdd += addvalue; //
                for (int i = ctx.rlen-1; i >= 0; i--)
                {                    
                    ctx.rbuff[i] = (byte)(toAdd & 0x000000FF);
                    toAdd >>= 8;
                }
                ctx.GVDIC[gvkey] = Utility.ByteArrayToHexStr(ctx.rbuff, ctx.rlen);
                ctx.ReportMessage("SYS>>{0}={1}", gvkey, ctx.GVDIC[gvkey]);
                return true;

                
            }
            ctx.ReportMessage("ERR>> Command is not ADD xxx:{0}", commandBody);
            return false;
        }

        public string CommandName
        {
            get { return "SYS<ADD"; }
        }

        #endregion
    }
}
