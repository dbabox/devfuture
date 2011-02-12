/* Create By:Samon Sun. 2011-1-27
 * SVN:
 * https://sgsoft-las.googlecode.com/svn/trunk
 * 
 * 核心执行引擎。 
 * 
 * */
using System;
using System.Collections.Generic;
using System.Text;
using Rtp.Driver.CardIO;
using Rtp.Driver.RfidReader;
using Rtp.Driver.Command;


namespace Rtp.Driver
{
    public class RtpCore
    {
        public const string STATEMENT_BLOCK = "{}";
        public const string TARGET_HEADER = "<";
  
        public const string COMPOSE = "/";
 
        /// <summary>
        /// 带参数的操作，操作是可以再脚本中独立使用的关键字.
        /// </summary>
        public const string ARG_OPERATION = "SET,SAM SLOT,SAM PARA,BUFF,ADD,SUB,PRINT,SAM RESET";
        /// <summary>
        /// 不带参数的操作
        /// </summary>
        public const string NONE_ARG_OPRATION = "HELP,OPEN READER,CLOSE READER,RESET READER,REQUEST CARD,MAC ON,MAC OFF"; //PAUSE
        /// <summary>
        /// 带参数函数。函数只能在块中调用。
        /// </summary>
        public const string ARG_FUNCTION = "KEY16MAC,KEY08MAC,DES,TRIPDES,DIVERSIFY";
        /// <summary>
        /// 无参数函数。
        /// </summary>
        public const string NONE_ARG_FUNCTION = "DATE,TIME,DATETIME";

        Dictionary<string, ICommand> commandEngine = new Dictionary<string, ICommand>();
        System.Collections.Specialized.StringCollection noneArgOperation = new System.Collections.Specialized.StringCollection();
        System.Collections.Specialized.StringCollection argOperation = new System.Collections.Specialized.StringCollection();

        System.Collections.Specialized.StringCollection noneArgFunction = new System.Collections.Specialized.StringCollection();
        System.Collections.Specialized.StringCollection argFunction = new System.Collections.Specialized.StringCollection();


        ICosIO cosIO;
        public ICosIO CosIO
        {
            get { return cosIO; }
            set { cosIO = value; }
        }

        CommandContext ctx;
        public RtpCore(CommandContext ctx_)
        {
            ctx = ctx_;

            #region 注册命令
            CommandCloseReader ccr = new CommandCloseReader();
            commandEngine.Add(ccr.CommandName, ccr);

            CommandCompose cc = new CommandCompose();
            commandEngine.Add(cc.CommandName, cc);

            CommandCpuApdu cca = new CommandCpuApdu();
            commandEngine.Add(cca.CommandName, cca);

            CommandDes cd = new CommandDes();
            commandEngine.Add(cd.CommandName, cd);

            CommandDiversify cdf = new CommandDiversify();
            commandEngine.Add(cdf.CommandName, cdf);

            CommandHelp ch = new CommandHelp();
            commandEngine.Add(ch.CommandName, ch);

            CommandMacOff cmo = new CommandMacOff();
            commandEngine.Add(cmo.CommandName, cmo);

            CommandMacOn cmon = new CommandMacOn();
            commandEngine.Add(cmon.CommandName, cmon);

            CommandOpenReader cor = new CommandOpenReader();
            commandEngine.Add(cor.CommandName, cor);

            CommandPause cp = new CommandPause();
            commandEngine.Add(cp.CommandName, cp);

            CommandRequestCard crc = new CommandRequestCard();
            commandEngine.Add(crc.CommandName, crc);

            CommandResetReader crr = new CommandResetReader();
            commandEngine.Add(crr.CommandName, crr);

            CommandSamApdu csa = new CommandSamApdu();
            commandEngine.Add(csa.CommandName, csa);

            CommandSamReset csr = new CommandSamReset();
            commandEngine.Add(csr.CommandName, csr);

            CommandSamSlot css = new CommandSamSlot();
            commandEngine.Add(css.CommandName, css);

            CommandSet cset = new CommandSet();
            commandEngine.Add(cset.CommandName, cset);

            CommandTargetHeader cth = new CommandTargetHeader();
            commandEngine.Add(cth.CommandName, cth);

            CommandTripDes ctd = new CommandTripDes();
            commandEngine.Add(ctd.CommandName, ctd);

            CommandSamParameter csp = new CommandSamParameter();
            commandEngine.Add(csp.CommandName, csp);

            CommandBuff crb = new CommandBuff();
            commandEngine.Add(crb.CommandName, crb);         

            CommandKey16MAC mac16 = new CommandKey16MAC();
            commandEngine.Add(mac16.CommandName, mac16);

            CommandKey08MAC mac8 = new CommandKey08MAC();
            commandEngine.Add(mac8.CommandName, mac8);

            CommandDate cdate = new CommandDate();
            commandEngine.Add(cdate.CommandName, cdate);

            CommandTime ctime = new CommandTime();
            commandEngine.Add(ctime.CommandName, ctime);

            CommandDateTime cdatetime = new CommandDateTime();
            commandEngine.Add(cdatetime.CommandName, cdatetime);

            CommandAdd cadd = new CommandAdd();
            commandEngine.Add(cadd.CommandName, cadd);

            CommandSub csub = new CommandSub();
            commandEngine.Add(csub.CommandName, csub);

            CommandPrint cprt = new CommandPrint();
            commandEngine.Add(cprt.CommandName, cprt);


            #endregion

            noneArgOperation.AddRange(NONE_ARG_OPRATION.Split(','));
            argOperation.AddRange(ARG_OPERATION.Split(','));

            noneArgFunction.AddRange(NONE_ARG_FUNCTION.Split(','));
            argFunction.AddRange(ARG_FUNCTION.Split(','));

            cosIO = new FileMapCosIO().ReadCosFile("default.cos");
        }

        #region 核心执行引擎
        /// <summary>
        /// 命令解释执行器。它就是核心执行引擎。
        /// </summary>
        /// <param name="line"></param>
        public bool CommandExcuter(string line)
        {
            #region 注释和空行，以及中止命令处理
            if (String.IsNullOrEmpty(line)) return true;
            line = line.Trim();
            if (line.StartsWith("#") 
                || line.StartsWith("==") 
                || line.StartsWith("--") 
                || line.StartsWith("REM")) return true;
            if (line.StartsWith("BREAK"))
            {
                ctx.ReportMessage("SYS>> User break the script.");
                return false;
            }
            #endregion
                       
            #region 脚本执行
            if (line.StartsWith("!"))
            {
                //直接加载文件执行
                bool scriptRunResult = true;
                string path = line.Substring(1, line.Length - 1);
                if (System.IO.File.Exists(path))
                {
                    using (System.IO.StreamReader r = new System.IO.StreamReader(path, System.Text.Encoding.Default))
                    {
                        string cmd;
                        int lineNO = 0;
                        DateTime dtstart = DateTime.Now;
                        while ((cmd = r.ReadLine()) != null)
                        {
                            ++lineNO;
                            cmd = cmd.Trim();
                            if (cmd.Length > 0) ctx.ReportMessage("FIL>> LN{0}: {1}",lineNO,cmd);
                           
                            if (!CommandExcuter(cmd))
                            {
                                ctx.ReportMessage("ERR>> CommandExcuter failed at Line {0}: {1}", lineNO, cmd);
                                scriptRunResult = false;
                                break;
                            }

                        }
                        TimeSpan ts = DateTime.Now - dtstart;
                        ctx.ReportMessage("SYS>> 脚本执行完成，共耗时:{0}ms", ts.TotalMilliseconds);
                    }
                }
                else
                {
                    ctx.ReportMessage("SYS>> !号必须接脚本文件名.Eg:!C:\\cpu.txt");
                    scriptRunResult = false;
                }
                return scriptRunResult;
            }
            #endregion
             
            #region 行命令预处理 变量，函数先执行，带<的COS指令先指向
            //不论什么命令，总是先处理$，然后处理{}           
            string calCmdL1;
            if (!AnalyzeOperatorGV(line, out calCmdL1)) //所有变量替换完成
            {
                ctx.ReportMessage("ERR>> Command format error:{0}, GV error.", line);
                return false;
            }
            //变量替换完成后，执行命令块执行功能
            string calCmdL2;
            if (!ExcuteFunctionBlock(calCmdL1, out calCmdL2)) //所有函数执行完成
            {
                ctx.ReportMessage("ERR>> Command format error:{0}, ExcuteFunctionBlock error.", calCmdL1);
                return false;
            }
            line = calCmdL2;//至此，语句中无$，无{}，无()
            if (line.Contains(TARGET_HEADER))
            {
                //先处理目标头
                string calCmdL3;
                //优先执行
                if (!AnalyzeRedirect(calCmdL2, out calCmdL3))
                {
                    ctx.ReportMessage("ERR>> Command format error:{0}, AnalyzeRedirect error.", line);
                    return false;
                }
                //已设置命令目标
                return commandEngine[ctx.CmdTarget].execute(calCmdL3, ctx); 
            }
            //至此，语句中无$，无{}，无(),无<
            #endregion

            #region 直接命令
            //组合命令直接执行
            if (line.Contains(COMPOSE)) return commandEngine[COMPOSE].execute(line, ctx); //组合命令不允许重定向，故最先执行
            //DESC命令与COS相关，在RTPCORE中直接执行
            if (line.StartsWith("DESC "))
            {
                string cos = line.Substring(5);
                byte[] cosbuff = new byte[cos.Length / 2];
                int coslen = Utility.HexStrToByteArray(cos, ref cosbuff);
                if (coslen >= 2) //获取COS描述
                {
                    ctx.ReportMessage("SYS>> {0}", cosIO.GetDescription(cosbuff[0], cosbuff[1]));
                    return true;
                }
            }
            #endregion

            #region 无参操作和前缀操作
            //无参数操作
            if (noneArgOperation.Contains(line)) return commandEngine[line].execute(line, ctx);

            //有参数的操作 "SET,SAM SLOT,SAM PARA,BUFF,ADD";
            foreach (string ao in argOperation)
            {
                if (line.StartsWith(ao)) return commandEngine[ao].execute(line, ctx);
            }
            #endregion
 
            
           


            
            //普通COS命令
            return commandEngine[ctx.CmdTarget].execute(line, ctx);            
            
        }

        #endregion

        #region 辅助的命令元素解析函数

        //符号解析优先级为：$号最优先，{}其次
        //=================================================
        /// <summary>
        /// 本函数将命令中的$符号子句全部解析完成，如$MAC_EKY会被替换成真正的二进制值；
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public bool AnalyzeOperatorGV(string cmd_gv, out string cmd)
        {
            cmd = cmd_gv;
            if (cmd.Contains("$"))
            {
                //有需要取变量的值               
                int gvIdx = -1;
                int gvEnd = 0;
                string gv;
                string gvClause;
                #region 替换所有变量
                while ((gvIdx = cmd.IndexOf('$')) >= 0)
                {
                    //找结束符                    
                    gvEnd = cmd.IndexOfAny(new char[] { ' ', '\r', '\n', ')', '<', ',', ';', '|' } , gvIdx);

                    if (gvEnd == -1)
                    {
                        gvEnd = cmd.Length - 1;
                    }
                    else
                    {
                        --gvEnd;
                    }
                    //找到一个变量，名称为gvIdx与gvEnd之间的部分
                    if ((gvEnd - gvIdx) > 0 && gvIdx <= (cmd.Length - 1))
                    {
                        gv = cmd.Substring(gvIdx + 1, gvEnd - gvIdx);
                        gvClause = cmd.Substring(gvIdx, gvEnd - gvIdx + 1);

                        //默认的全局变量SBUFF,RBUFF
                        if (gv == "SBUFF")
                        {
                            ctx.ReportMessage("SYS>> Get sbuff value as GV.");
                            cmd = cmd.Replace(gvClause, Utility.ByteArrayToHexStr(ctx.sbuff, ctx.slen));
                            ctx.ReportMessage("SYS>> CMD={0}", cmd);
                            continue;
                        }
                        if (gv == "RBUFF")
                        {
                            ctx.ReportMessage("SYS>> Get rbuff value as GV.");
                            cmd = cmd.Replace(gvClause, Utility.ByteArrayToHexStr(ctx.rbuff, ctx.rlen));
                            ctx.ReportMessage("SYS>> CMD={0}", cmd);
                            continue;
                        }

                        //找到一个全局变量
                        if (ctx.GVDIC.ContainsKey(gv))
                        {
                            ctx.ReportMessage("SYS>> {0} = {1} ", gv, ctx.GVDIC[gv]);
                            //使用这个变量替换原来的语句
                            cmd = cmd.Replace(gvClause, ctx.GVDIC[gv]);
                            ctx.ReportMessage("SYS>> CMD={0}", cmd);
                            continue;
                        }
                        else
                        {
                            ctx.ReportMessage("ERR>> {0} 未知！", gv);
                            return false;
                        }
                    }
                }
                #endregion
            }
            return true;
        }

        /// <summary>
        /// 分析重定向语句，必须在GV分析之后，COS指令指向之前进行。
        /// 如:$SAM0 < 80 CA 00 00 09 $SA_CODE
        /// </summary>
        /// <param name="line"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public bool AnalyzeRedirect(string line, out string cmd)
        {
            cmd = line;
            if (line.Contains(TARGET_HEADER))//包含重定向语句
            {
                int idx = line.IndexOf(TARGET_HEADER);
                //执行切换命令目标的指令，目前支持指向SAM卡或者CPU卡
                if (!commandEngine[TARGET_HEADER].execute(line.Substring(0, idx), ctx)) return false;
                //成功才继续
                if (idx == (line.Length - 1))
                {
                    ctx.ReportMessage("{0}切换命令目标成功，但后续无COS指令.", line);
                    return false;//切换完成
                }
                cmd = line.Substring(idx + 1, line.Length - idx - 1);//只包含了命令体
            }
            else
            {
                ctx.CmdTarget = "CPU";//不含的话自动重置为CPU
            }
            return true;
        }

        /// <summary>
        /// 执行函数计算块{}.目前仅支持DES/TripDES计算。
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public bool ExcuteFunctionBlock(string cmdIn, out string cmdOut)
        {
            cmdOut = cmdIn;//先保存最原始的字符串
            int lkhIdx = -1;
            int rkhIdx = -1;
            int lxkhIdx = -1;
            int rxkhIdx = -1;
            string funcBlock = null;
            string statementBody = null;
            string resultStr = null;
            while (cmdIn.Contains("{") && cmdIn.Contains("}"))
            {
                lxkhIdx = -1;
                rxkhIdx = -1;

                lkhIdx = cmdIn.IndexOf('{');
                rkhIdx = cmdIn.IndexOf('}');
                funcBlock = cmdIn.Substring(lkhIdx, rkhIdx + 1 - lkhIdx);
                statementBody = funcBlock.Substring(1, funcBlock.Length - 2).Trim();
                ctx.ReportMessage("SYS>> 计算:{0}", funcBlock);

                //分解其中的语句
                lxkhIdx = cmdIn.IndexOf('(', lkhIdx); //左小括号
                if(lxkhIdx>0) rxkhIdx = cmdIn.IndexOf(')', lxkhIdx);//右小括号

                #region 明文命令块执行
                if (lxkhIdx < 0 && rxkhIdx < 0)//若语句中无小括号
                {
                    //直接命令，如:Date,Time,若不是直接命令，就当作明文COS执行
                    if (noneArgFunction.Contains(statementBody))
                    {
                        if (!commandEngine[statementBody].execute(statementBody, ctx)) return false;
                        cmdIn = cmdIn.Replace(funcBlock, Utility.ByteArrayToHexStr(ctx.rbuff, ctx.rlen));
                        continue;
                    }
                    
                    //如果不是无参函数 语句块中是明文                    
                    #region 明文COS指令
                    ctx.ReportMessage("SYS>> Statemants Block is plain cos instructor.");
                    //先处理目标头
                    string calCmdL3;
                    if (!AnalyzeRedirect(statementBody, out calCmdL3))
                    {
                        ctx.ReportMessage("ERR>> Command format error:{0}, AnalyzeRedirect error.", statementBody);
                        return false;
                    }
                    statementBody = calCmdL3;//至此，语句中无$，无{}，无(),无<
                    if (commandEngine[ctx.CmdTarget].execute(statementBody, ctx)) //若语句执行成功
                    {
                        //用执行结果替换
                        resultStr = Utility.ByteArrayToHexStr(ctx.rbuff, ctx.rlen - 2);
                        cmdIn = cmdIn.Replace(funcBlock, resultStr);
                        continue;
                    }
                    #endregion
                    return false;
                }
                #endregion

                #region 函数块执行
                if (lxkhIdx < 0 || rxkhIdx < lxkhIdx)
                {
                    ctx.ReportMessage("ERR>> Statemants Block format is incorrect.");
                    return false;
                }
                string funcName = cmdIn.Substring(lkhIdx + 1, lxkhIdx - lkhIdx - 1).Trim().ToUpper();//知道了函数名
                ctx.ReportMessage("SYS>> Function Name:{0}", funcName);
                //带参数函数执行
                if (commandEngine.ContainsKey(funcName) && commandEngine[funcName].execute(statementBody, ctx))
                {
                    //函数执行成功
                    resultStr = Utility.ByteArrayToHexStr(ctx.rbuff, ctx.rlen);
                    cmdIn = cmdIn.Replace(funcBlock, resultStr);
                }
                else
                {
                    ctx.ReportMessage("ERR>> unknown command :{0}.",cmdIn);
                    return false;
                }

               
                #endregion
            }
            cmdOut = cmdIn;//全部计算完毕
            return true;
        }
         

        #endregion

         
    }
}
