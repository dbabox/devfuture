using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// HELP�����ӡ������
    /// </summary>
    public class CommandHelp:ICommand
    {
        

    
        public CommandHelp()
        {
            
        }

        #region ICommand ��Ա

        public bool execute(string commandBody, CommandContext ctx)
        {
            #region  //��ӡ������Ϣ
            ctx.ReportMessage("OPEN READER\t:�򿪶�����");
            ctx.ReportMessage("CLOSE READER\t:�رն�����");
            ctx.ReportMessage("RESET READER\t:���ö�����");
            ctx.ReportMessage("REQUEST CARD\t:Ѱ��");
            ctx.ReportMessage("MAC ON \t:�Զ����������MAC");
            ctx.ReportMessage("MAC OFF \t:�ر�����MAC����");
            ctx.ReportMessage("{DES(RN8,KEY8} \t:DES����");
            ctx.ReportMessage("{TripDES(RN8,KEY16)} \t:3DES����.");
            ctx.ReportMessage("$VAR_NAME \t:ȡ����.");
            ctx.ReportMessage("Diversify($KM,$SEED) \t:ʹ����ԿKM(16�ֽ�)����ɢ����SEED���������ɢ��Կ.");
            ctx.ReportMessage("XX XX/XX XX \t:���COSָ���00 A4 00 00 02 3F 00/10 01 /00 19");
            ctx.ReportMessage("SET [GVNAME[=GVVALUE]]|ALL \t:��ʾ������ȫ�ֱ���ֵ.");
            ctx.ReportMessage("PAUSE \t:��ͣ���û�����������������ڽű��в�����Զϵ�.");
            ctx.ReportMessage("DESC XXXX \t:��ʾ�����״̬�ֺ���.");
            ctx.ReportMessage("RBUFF [offset],[GV|Length],[ /VALUE] \t:��������������.");
            ctx.ReportMessage("HELP \t:��ӡ����Ϣ.");
            #endregion
            return true;
        }

        public string CommandName
        {
            get { return "HELP"; }
        }

        #endregion
    }
}
