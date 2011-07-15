using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// ϵͳ����
    /// ϵͳ������������ʵ��BUFFָ��
    /// BUFF offset,Length,GV|offset,Length,GV|offset,Length,GV����ȡBUFF��ֵ����offset��ʼ������ΪLength�ֽڣ��ŵ�GV�����У�������
    /// BUFF offset,$GV|offset,$GV|offset,$GV ����BUFF��ֵ,��offset��ʼ��������
    /// </summary>
    class CommandBuff:ICommand
    {
        const int BUFF_LEN = 256;
        private readonly byte[] buff = new byte[BUFF_LEN];

        #region ICommand ��Ա

        /// <summary>
        /// ���������ʽ 00,xx xx xx xx xx 
        /// </summary>
        /// <param name="commandBody"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public bool execute(string commandBody, CommandContext ctx)
        {
            string par =Utility.GetSubStringBetweenChars(commandBody,'(',')').Trim().ToUpper();
            string[] sections = par.Split('|');
            if (sections.Length == 0)
            {
                ctx.ReportMessage("ERR>>CommandBuff:command format error:{0}", commandBody);
                return false;
            }
            string[] args = null;
            for (int i = 0; i < sections.Length; i++)
            {
                args = sections[i].Split(',');
                if (args.Length == 3)
                {
                    #region ��BUFF��ȡֵ�洢��GV��
                    ctx.ReportMessage("SYS>>Get buff vallue to GV:{0}", commandBody);
                    int offset = 0;
                    int length = 0;
                    if (int.TryParse(args[0], System.Globalization.NumberStyles.HexNumber, null, out offset)
                        && int.TryParse(args[1], System.Globalization.NumberStyles.HexNumber, null, out length))
                    {
                        string gvkey = args[2].Trim().ToUpper();

                        if (ctx.GVDIC.ContainsKey(gvkey))
                        {
                            ctx.GVDIC[gvkey] = Utility.ByteArrayToHexStr(buff, offset, length, "");
                        }
                        else
                        {
                            ctx.GVDIC.Add(gvkey, Utility.ByteArrayToHexStr(buff, offset, length, ""));
                        }
                        ctx.ReportMessage("SYS>>{0}={1}", gvkey, ctx.GVDIC[gvkey]);

                    }
                    #endregion
                }
                else if (args.Length == 2)
                {
                    #region ��GV��ȡֵ�洢��BUFF��
                    ctx.ReportMessage("SYS>>Set buff vallue :{0}", commandBody);
                    int offset = 0;
                    if (int.TryParse(args[0], System.Globalization.NumberStyles.HexNumber, null, out offset)
                        && (ctx.rlen = (byte)Utility.HexStrToByteArray(args[1], ref ctx.rbuff)) > 0)
                    {
                        if ((offset + ctx.rlen) > BUFF_LEN)
                        {
                            ctx.ReportMessage("ERR>>CommandBuff:command format error: value too long! {0}", commandBody);
                            return false;
                        }
                        Array.Copy(ctx.rbuff, 0, buff, offset, ctx.rlen);
                        ctx.ReportMessage("SYS>>BUFF={0}", Utility.ByteArrayToHexStr(buff, offset + ctx.rlen));
                    }
                    #endregion
                }
                else
                {
                    ctx.ReportMessage("ERR>>CommandBuff:command format error:{0},error at {1} section.", commandBody, i + 1);
                    return false;
                }
            }
            return true;
        }

        public string CommandName
        {
            get { return "SYS<BUFF"; }
        }

        #endregion
    }
}
