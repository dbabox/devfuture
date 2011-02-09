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
            System.Diagnostics.Trace.TraceInformation("OPEN READER\t:�򿪶�����");
            System.Diagnostics.Trace.TraceInformation("CLOSE READER\t:�رն�����");
            System.Diagnostics.Trace.TraceInformation("RESET READER\t:���ö�����");
            System.Diagnostics.Trace.TraceInformation("REQUEST CARD\t:Ѱ��");
            System.Diagnostics.Trace.TraceInformation("MAC ON \t:�Զ����������MAC");
            System.Diagnostics.Trace.TraceInformation("MAC OFF \t:�ر�����MAC����");
            System.Diagnostics.Trace.TraceInformation("{DES(RN8,KEY8} \t:DES����");
            System.Diagnostics.Trace.TraceInformation("{TripDES(RN8,KEY16)} \t:3DES����.");
            System.Diagnostics.Trace.TraceInformation("$VAR_NAME \t:ȡ����.");
            System.Diagnostics.Trace.TraceInformation("Diversify($KM,$SEED) \t:ʹ����ԿKM(16�ֽ�)����ɢ����SEED���������ɢ��Կ.");
            System.Diagnostics.Trace.TraceInformation("XX XX/XX XX \t:���COSָ���00 A4 00 00 02 3F 00/10 01 /00 19");
            System.Diagnostics.Trace.TraceInformation("SET [GVNAME[=GVVALUE]]|ALL \t:��ʾ������ȫ�ֱ���ֵ.");
            System.Diagnostics.Trace.TraceInformation("PAUSE \t:��ͣ���û�����������������ڽű��в�����Զϵ�.");
            System.Diagnostics.Trace.TraceInformation("DESC XXXX \t:��ʾ�����״̬�ֺ���.");
            System.Diagnostics.Trace.TraceInformation("RBUFF [offset],[GV|Length],[ /VALUE] \t:��������������.");
            System.Diagnostics.Trace.TraceInformation("HELP \t:��ӡ����Ϣ.");
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
