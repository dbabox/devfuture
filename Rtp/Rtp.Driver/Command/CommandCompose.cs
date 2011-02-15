using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// ��ϲ�����/��������ִ����������ͬ��ָ��磺00 A4 00 02 3F 00/10 01/00 19
    /// Ҫ�������ϵĲ��ֿ���ȫ�滻ǰ��Ĳ��֣����滻�Ĳ���Ҫ��ȡ�
    /// </summary>
    public class CommandCompose:ICommand
    {
        

        
        public CommandCompose()
        {
            
        }

        #region ICommand ��Ա

        public bool execute(string commandBody, CommandContext ctx)
        {
            if (commandBody.Contains("/")) //��������������MAC
            {
                #region �ֽ���������ִ��
                string[] cmdarr = commandBody.Split('/');
                ctx.slen = (byte)Utility.HexStrToByteArray(cmdarr[0], ref ctx.sbuff); //�õ���һ������
                ctx.rc = ctx.Rfid.CPU_APDU(ctx.slen, ctx.sbuff, ref ctx.rlen, ctx.rbuff);
                if (ctx.rc != 0 || Utility.IsSwSuccess(ctx.rlen, ctx.rbuff) == false)
                {
                    ctx.ReportMessage("ERR>> Command[0] failed:{0}", Utility.ByteArrayToHexStr(ctx.sbuff, ctx.slen));
                    return false;
                }
                byte[] parv = new byte[ctx.slen]; //�����Ĳ���ָ�������
                int parlen = 0;
                for (int i = 1; i < cmdarr.Length; i++)
                {
                    parlen = Utility.HexStrToByteArray(cmdarr[i], ref parv);
                    if (parlen <= 0 || parlen > ctx.slen)
                    {
                        ctx.ReportMessage("ERR>> Command format incorrect:{0}", commandBody);
                        return false;
                    }
                    Array.Copy(parv, 0, ctx.sbuff, ctx.slen - parlen, parlen);
                    
                    ctx.rc = ctx.Rfid.CPU_APDU(ctx.slen, ctx.sbuff, ref ctx.rlen, ctx.rbuff);
                    if (ctx.rc != 0 || !Utility.IsSwSuccess(ctx.rlen, ctx.rbuff))
                    {
                        ctx.ReportMessage("ERR>> Command[{0}] failed:{1}", i, Utility.ByteArrayToHexStr(ctx.sbuff, ctx.slen));
                        if(ctx.IsBreakOnFailed) return false;
                    }
                    else
                    {
                        ctx.ReportMessage("SYS>> Command[{0}] success:{1}", i, Utility.ByteArrayToHexStr(ctx.sbuff, ctx.slen));
                        //����ִ��
                    }
                }
                //ȫ���ɹ�ִ�У�����true
                return true;
                #endregion
            }
            return false;
        }

        public string CommandName
        {
            get { return "/"; }
        }

        #endregion
    }
}
