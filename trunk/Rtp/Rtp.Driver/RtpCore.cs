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
    public delegate void ReceiveContxtMessage(string message);

    public class RtpCore
    {
        public const string STATEMENT_BLOCK = "{}";
        /// <summary>
        /// 重定向指令；
        /// 
        /// </summary>
        public const string TARGET_TAG = "<";
  
        public const string COMPOSE = "/";
 
        /// <summary>
        /// 带参数的系统命令，操作是可以再脚本中独立使用的关键字.
        /// </summary>
        //public const string ARG_OPERATION = "SET,SAMSLOT,SAMPARA,BUFF,ADD,SUB,PRINT,SAMRESET,EXECUTEMODE,DESC";
        /// <summary>
        /// 不带参数的系统命令
        /// </summary>
        //public const string NONE_ARG_OPRATION = "HELP,OPENREADER,CLOSEREADER,RESETREADER,REQUESTCARD,MACON,MACOFF"; //PAUSE
        /// <summary>
        /// 带参数函数，参数用小括号包围。函数只能在块中调用。
        /// </summary>
        //public const string ARG_FUNCTION = "KEY16MAC,KEY08MAC,DES,TRIPDES,DIVERSIFY,PBOCDESENCKEY16,PBOCDESENCKEY8,PBOCDESDECKEY16,PBOCDESDECKEY8";
        /// <summary>
        /// 无参数函数。
        /// </summary>
        //public const string NONE_ARG_FUNCTION = "DATE,TIME,DATETIME";
        Dictionary<string, ICommand> commandEngine = new Dictionary<string, ICommand>();     
    
       

        ICosDictionary cosIO;
        public ICosDictionary CosIO
        {
            get { return cosIO; }
            set { cosIO = value; }
        }

        CommandContext ctx;
        public RtpCore(CommandContext ctx_)
        {
            ctx = ctx_;
            cosIO = new FileMapCosIO().ReadCosFile("default.cos");
            #region 注册命令
            CommandCloseReader ccr = new CommandCloseReader();
            commandEngine.Add(ccr.CommandName, ccr);

          

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

            CommandDesc cth = new CommandDesc(cosIO);
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

            CommandExecuteMode cem = new CommandExecuteMode();
            commandEngine.Add(cem.CommandName, cem);

            CommandPbocDesEncKey8 cmdpbocEncKey8 = new CommandPbocDesEncKey8();
            commandEngine.Add(cmdpbocEncKey8.CommandName, cmdpbocEncKey8);

            CommandPbocDesEncKey16 cmdpbocEncKey16 = new CommandPbocDesEncKey16();
            commandEngine.Add(cmdpbocEncKey16.CommandName, cmdpbocEncKey16);

            CommandPbocDesDecKey16 cmdpbocDecKey16 = new CommandPbocDesDecKey16();
            commandEngine.Add(cmdpbocDecKey16.CommandName, cmdpbocDecKey16);

            CommandPbocDesDecKey8 cmdpbocDecKey8 = new CommandPbocDesDecKey8();
            commandEngine.Add(cmdpbocDecKey8.CommandName, cmdpbocDecKey8);

            CommandULRead cmdULRead = new CommandULRead();
            commandEngine.Add(cmdULRead.CommandName, cmdULRead);

            CommandULWrite cmdULWrite = new CommandULWrite();
            commandEngine.Add(cmdULWrite.CommandName, cmdULWrite);
            #endregion

          
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

            #region 执行命令
            //行命令预处理 变量，函数先执行，带<的COS指令先指向
            //不论什么命令，总是先处理$，然后处理{}           
            string calCmdL1;
            if (!AnalyzeOperatorGV(line, out calCmdL1)) //所有变量替换完成
            {
                ctx.ReportMessage("ERR>> Command format error:{0}, GV error.", line);
                return false;
            }
            //变量替换完成后，执行命令块中函数
            string calCmdL2;
            if (!ExcuteFunctionBlock(calCmdL1, out calCmdL2)) //所有函数执行完成
            {
                ctx.ReportMessage("ERR>> Command format error:{0}, ExcuteFunctionBlock error.", calCmdL1);
                return false;
            }          
            if (calCmdL2.Contains(TARGET_TAG) == false)
            {
                //自动为其添加上一次的Header
                string fmtLine = String.Format("{0}{1}{2}", ctx.CmdTarget.Substring(0, ctx.CmdTarget.IndexOf('<')), TARGET_TAG, calCmdL2);
                ctx.ReportMessage("SYS>>自动添加命令TARGET_TAG:实际执行命令:{0}", fmtLine);
                calCmdL2 = fmtLine;              
            }
            //先处理目标头
            string args;
            //优先执行
            if (!CommandAnalyze(calCmdL2, out args)) //分析命令执行的对象
            {
                ctx.ReportMessage("ERR>> Command format error:{0}, AnalyzeRedirect error.", line);
                return false;
            }
            if (commandEngine.ContainsKey(ctx.CmdTarget))
            {
                return commandEngine[ctx.CmdTarget].execute(args, ctx);
            }
            else
            {
                ctx.ReportMessage("不可识别的命令:{0} {1}", ctx.CmdTarget,args);
                return false;
            }
            
            #endregion
                           
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
                    gvEnd = cmd.IndexOfAny(new char[] { ' ', '\r', '\n', ')', '<', ',', ';', '|','}','\t' } , gvIdx);

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
        /// 匹配:
        /// UL { Read 04
        /// M1 { Read 05
        /// CPU{ APDU 00 84 xx 
        /// SYS{ REQUESTCARD
        /// SAM{APDU slot, 00 84 
        /// </summary>
        const string REG_CARDTYPE_CMD = @"([\S]+)[\s]*(<)[\s]*([\S]*)(\s.*)?";
        const string REG_HEX_STRING = @"[A-Fa-f0-9\s]+";
        readonly System.Text.RegularExpressions.Regex regCmd = new System.Text.RegularExpressions.Regex(REG_CARDTYPE_CMD);
        readonly System.Text.RegularExpressions.Regex regHexStr = new System.Text.RegularExpressions.Regex(REG_HEX_STRING);
        

        /// <summary>
        /// 分析重定向语句，必须在GV分析之后，COS指令指向之前进行。
        /// 如:SAM00{ 80 CA 00 00 09
        /// </summary>
        /// <param name="line"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool CommandAnalyze(string line, out string args)
        {
            args = String.Empty;
            if (line.Contains(TARGET_TAG) && regCmd.IsMatch(line))//包含命令方向标志
            {
                var rc = regCmd.Match(line);
                if (rc.Groups.Count != 5)
                {
                    ctx.ReportMessage("ERR>>{0} format incorrect .", line);
                    return false;
                }
                ctx.CmdTarget = String.Format("{0}{1}{2}", rc.Groups[1].Value,TARGET_TAG, rc.Groups[3].Value);
                args = rc.Groups[4].Value; //此时cmd只包含参数
                ctx.ReportMessage("SYS>>{0} TargetHeader is {1}.", line, ctx.CmdTarget);
                return true;
            }
            ctx.ReportMessage("ERR>>{0} format incorrect .", line);
            return false;           
        }

        /// <summary>
        /// 执行函数计算块{}.目前仅支持DES/TripDES计算。
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public bool ExcuteFunctionBlock(string cmdIn, out string cmdOut)
        {
            cmdOut = cmdIn;//先保存最原始的字符串           
            string funcBlock = null;
            string statementBody = null;
            string resultStr = null;            
            while (cmdIn.Contains("{") && cmdIn.Contains("}"))
            {
                funcBlock = Utility.GetSubStringBetweenCharsInclude(cmdIn, '{', '}');
                statementBody = Utility.GetSubStringBetweenChars(funcBlock, '{', '}'); 
                ctx.ReportMessage("SYS>> 计算:{0}", funcBlock);

                //先处理目标头
                string args;
                if (!CommandAnalyze(statementBody, out args))
                {
                    ctx.ReportMessage("ERR>> Command format error:{0}, AnalyzeRedirect error.", statementBody);
                    return false;
                }
                if (commandEngine[ctx.CmdTarget].execute(args, ctx)) //若语句执行成功
                {
                    //用执行结果替换
                    resultStr = Utility.ByteArrayToHexStr(ctx.rbuff, ctx.rlen - 2);
                    cmdIn = cmdIn.Replace(funcBlock, resultStr);
                    continue;
                }
                return false;
            }
            cmdOut = cmdIn;//全部计算完毕
            return true;
        }
        #endregion

         
    }

    /// <summary>
    /// 下面的类都是为了编译执行而设
    /// </summary>
    public class TagMap
    {
        #region 系统操作映射 00
        public const ushort SYS_OPENREADER = 0x0001;
        public const ushort SYS_CLOSEREADER = 0x0002;
        public const ushort SYS_RESETREADER = 0x0003;
        public const ushort SYS_REQUESTCARD = 0x0004;
        public const ushort SYS_MACON = 0x0005;
        public const ushort SYS_MACOFF = 0x0006;
        public const ushort SYS_SET = 0x0007;
        public const ushort SYS_DESC = 0x0008;
        public const ushort SYS_HELP = 0x0009;
        public const ushort SYS_SAMSLOT = 0x000A;
        public const ushort SYS_SAMRESET = 0x000B;
        public const ushort SYS_SAMPARA = 0x000C;
        public const ushort SYS_Add = 0x000D;
        public const ushort SYS_SUB = 0x000E;
        public const ushort SYS_BUFF = 0x000F;
        public const ushort SYS_PRINT = 0x0010;
        #endregion

        #region CPU COS操作映射 01

        public const ushort CPU_A = 0x0100;
        public const ushort CPU_B = 0x0101;

        #endregion

        /// <summary>
        /// SAM=02 XX(XX为slot号).
        /// </summary>
        public const ushort SAM_TAG_PREFIX = 0x0200;

        #region 系统函数调用映射 03
        public const ushort FNC_DES = 0x0301;
        public const ushort FNC_3DES = 0x0302;
        public const ushort FNC_DATE = 0x0303;
        public const ushort FNC_TIME = 0x0304;
        public const ushort FNC_DATETIME = 0x0305;
        public const ushort FNC_DIVERSIFY = 0x0306;
        public const ushort FNC_KEY08MAC = 0x0307;
        public const ushort FNC_KEY16MAC = 0x0308;
        #endregion
        /// <summary>
        /// VAR=04 XX 系统支持最多255个全局持久变量.
        /// </summary>
        public const ushort VAR_TAG_PREFIX = 0x0400;
        /// <summary>
        /// TLV模板字符
        /// 例：FF[TAG][LENGTH]FF[TAG][LENGTH][CONTENT]
        /// </summary>
        public const byte TLV = 0xFF;
    }

    public class TLVItem
    {
        public const int VAR_BUFF_SIZE = 128;
        public TLVItem()
        {
            var = new byte[VAR_BUFF_SIZE];
            length = 0;
            tag = 0;
        }

        public UInt32 tag;
        public byte length;
        public readonly byte[] var;

        /// <summary>
        /// Tag的高2字节。
        /// Tag类别：SYS=00 XX(XX为系统调用代码),
        /// CPU=01 XX(XX表示CPU类别，typeA, typeB等),
        /// SAM=02 XX(XX为slot号),
        /// FNC=03 XX(XX为函数代码),
        /// VAR=04 XX 系统支持最多255个全局持久变量；
        /// </summary>
        public byte TagType
        {
            get
            {
                byte tagType=(byte)(tag >> 24);
                return tagType;
            }
        }

        /// <summary>       
        /// Tag类别的尾字节。
        /// </summary>
        public byte TagSubType
        {
            get
            {
                byte tagType = (byte)(tag >> 16);
                tagType &= 0x00FF;
                return (byte)tagType;
            }
        }



    }
 
}
