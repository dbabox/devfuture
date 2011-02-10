using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// HELP命令。打印帮助。
    /// </summary>
    public class CommandHelp:ICommand
    {
        

    
        public CommandHelp()
        {
            
        }

        #region ICommand 成员

        public bool execute(string commandBody, CommandContext ctx)
        {
            #region  //打印帮助信息
            ctx.ReportMessage("OPEN READER\t:打开读卡器");
            ctx.ReportMessage("CLOSE READER\t:关闭读卡器");
            ctx.ReportMessage("RESET READER\t:重置读卡器");
            ctx.ReportMessage("REQUEST CARD\t:寻卡");
            ctx.ReportMessage("MAC ON \t:自动计算命令的MAC");
            ctx.ReportMessage("MAC OFF \t:关闭命令MAC计算");
            ctx.ReportMessage("{DES(RN8,KEY8} \t:DES加密");
            ctx.ReportMessage("{TripDES(RN8,KEY16)} \t:3DES加密.");
            ctx.ReportMessage("$VAR_NAME \t:取变量.");
            ctx.ReportMessage("Diversify($KM,$SEED) \t:使用密钥KM(16字节)，分散因子SEED，计算出分散密钥.");
            ctx.ReportMessage("XX XX/XX XX \t:组合COS指令，如00 A4 00 00 02 3F 00/10 01 /00 19");
            ctx.ReportMessage("SET [GVNAME[=GVVALUE]]|ALL \t:显示和设置全局变量值.");
            ctx.ReportMessage("PAUSE \t:暂停，用户按任意键继续，可在脚本中插入调试断点.");
            ctx.ReportMessage("DESC XXXX \t:显示命令或状态字含义.");
            ctx.ReportMessage("RBUFF [offset],[GV|Length],[ /VALUE] \t:缓冲区操作命令.");
            ctx.ReportMessage("HELP \t:打印本消息.");
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
