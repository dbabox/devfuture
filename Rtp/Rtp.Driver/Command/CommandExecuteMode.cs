using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    class CommandExecuteMode : ICommand
    {
        public bool execute(string commandBody, CommandContext ctx)
        {
            if (!commandBody.StartsWith(CommandName)) throw new ArgumentException(String.Format("{0}命令格式错误:{1}", CommandName, commandBody));

            if (commandBody.Length > CommandName.Length)
            {
                string mode_str = commandBody.Substring(CommandName.Length, commandBody.Length - CommandName.Length).Trim();
                byte mode = 0;
                if (Byte.TryParse(mode_str,
                    System.Globalization.NumberStyles.HexNumber, null, out mode))
                {
                    if ((mode & 0xF0) > 0 && (mode & 0xF0) < 0x40 && (mode & 0x0F) < 2)
                    {
                        ctx.ExecuteMode = mode;
                        ctx.ReportMessage("SYS>>Change Execute Mode=0x{0,2:X2}", mode);
                        return true;
                    }
                 
                }
            }
            ctx.ReportMessage("ERR>>System Call failed:{0}", commandBody);
            return false;
        }

        public string CommandName
        {
            get { return "EXECUTEMODE"; }
        }
    }
}
