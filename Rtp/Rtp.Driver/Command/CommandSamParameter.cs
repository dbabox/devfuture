using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// SAM PARA slot,cpupro:协议(T=0/1),cpuetu:波特率(9600/115200)
    /// 参数都是16进制
    /// </summary>
    class CommandSamParameter:ICommand
    {

        #region ICommand 成员

        public bool execute(string commandBody, CommandContext ctx)
        {
            if (commandBody.StartsWith(CommandName))
            {
                string par = commandBody.Substring(9, commandBody.Length - 9).Trim();
                string[] pars = par.Split(',');
                byte slot = 0;
                byte cpupro = 0;
                byte cputu = 0;
                if (Byte.TryParse(pars[0], System.Globalization.NumberStyles.HexNumber, null, out slot)
                    && Byte.TryParse(pars[1], System.Globalization.NumberStyles.HexNumber, null, out cpupro)
                    && Byte.TryParse(pars[2], System.Globalization.NumberStyles.HexNumber, null, out cputu)
                    )
                {
                    return 0==ctx.Rfid.SAM_SetPara(slot, cpupro, cputu);
                }
                System.Diagnostics.Trace.TraceWarning("Command {0} format incorrect.The correct format is SAM PARA slot,cpupro:协议(T=0/1),cpuetu:波特率(9600/115200).", commandBody);
                return false;
            }
            return false;
        }

        /// <summary>
        /// 参数格式为 SAM PARA slot,cpupro:协议(T=0/1),cpuetu:波特率(9600/115200)
        /// 参数以逗号分隔，全部以16进制表达，不要0x起头。
        /// </summary>
        public string CommandName
        {
            get { return "SAM PARA"; }
        }

        #endregion
    }
}
