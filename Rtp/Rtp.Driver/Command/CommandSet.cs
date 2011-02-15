using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// 设置环境变量命令。
    /// </summary>
    public class CommandSet:ICommand
    {
        
 
       
        #region ICommand 成员

        public bool execute(string commandBody, CommandContext ctx)
        {
            if (!commandBody.StartsWith(CommandName)) throw new ArgumentException(String.Format("{0}命令格式错误:{1}", CommandName, commandBody));

            #region SET命令
            if (commandBody.Length > 3)
            {
                int equIdx = commandBody.IndexOf('=', 3);

                #region 显示GV
                if (equIdx < 0) //没有=号，则显示该变量的值
                {
                    string partname = commandBody.Substring(3, commandBody.Length - 3).Trim().ToUpper();
                    if (String.IsNullOrEmpty(partname))
                    {
                        ctx.ReportMessage("SYS>>GVDIC:---------BEGIN-----------");
                        foreach (string key in ctx.GVDIC.Keys)
                        {
                            ctx.ReportMessage("GV>> {0}={1}", key, ctx.GVDIC[key]);
                        }
                        ctx.ReportMessage("SYS>>GVDIC:---------END-----------");
                    }
                    else
                    {
                        int okPartName = 0;
                        foreach (string key in ctx.GVDIC.Keys) //前缀匹配模式
                        {
                            if (key.StartsWith(partname))
                            {
                                ctx.ReportMessage("SYS>>GV>> {0}={1}", key, ctx.GVDIC[key]);
                                ++okPartName;
                            }
                        }
                        if (okPartName == 0)
                        {
                            ctx.ReportMessage("SYS>> {0} is not a GV", partname);
                        }
                    }
                    return true;
                }
                #endregion

                //设置变量值
                string gvName = commandBody.Substring(3, equIdx - 3).Trim().ToUpper();
                string gvValue = commandBody.Substring(equIdx + 1, commandBody.Length - equIdx - 1).Trim().ToUpper();

                ctx.ReportMessage("SYS>> GV={0},Value={1}", gvName, gvValue);

                if (ctx.GVDIC.ContainsKey(gvName))
                {
                    ctx.GVDIC[gvName] = gvValue;
                }
                else
                {
                    ctx.GVDIC.Add(gvName, gvValue);
                }
                return true;
            }
            #endregion
            return false;
        }

        public string CommandName
        {
            get { return "SET"; }
        }

        #endregion
    }
}
