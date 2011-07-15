/* Create By:Samon Sun. 2011-1-27
 * SVN:
 * https://sgsoft-las.googlecode.com/svn/trunk
 * 
 * ����ִ�����档 
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
        /// �ض���ָ�
        /// 
        /// </summary>
        public const string TARGET_TAG = "<";
  
        public const string COMPOSE = "/";
 
        /// <summary>
        /// ��������ϵͳ��������ǿ����ٽű��ж���ʹ�õĹؼ���.
        /// </summary>
        //public const string ARG_OPERATION = "SET,SAMSLOT,SAMPARA,BUFF,ADD,SUB,PRINT,SAMRESET,EXECUTEMODE,DESC";
        /// <summary>
        /// ����������ϵͳ����
        /// </summary>
        //public const string NONE_ARG_OPRATION = "HELP,OPENREADER,CLOSEREADER,RESETREADER,REQUESTCARD,MACON,MACOFF"; //PAUSE
        /// <summary>
        /// ������������������С���Ű�Χ������ֻ���ڿ��е��á�
        /// </summary>
        //public const string ARG_FUNCTION = "KEY16MAC,KEY08MAC,DES,TRIPDES,DIVERSIFY,PBOCDESENCKEY16,PBOCDESENCKEY8,PBOCDESDECKEY16,PBOCDESDECKEY8";
        /// <summary>
        /// �޲���������
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
            #region ע������
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

        #region ����ִ������
        /// <summary>
        /// �������ִ�����������Ǻ���ִ�����档
        /// </summary>
        /// <param name="line"></param>
        public bool CommandExcuter(string line)
        {
            #region ע�ͺͿ��У��Լ���ֹ�����
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
                       
            #region �ű�ִ��
            if (line.StartsWith("!"))
            {
                //ֱ�Ӽ����ļ�ִ��
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
                        ctx.ReportMessage("SYS>> �ű�ִ����ɣ�����ʱ:{0}ms", ts.TotalMilliseconds);
                    }
                }
                else
                {
                    ctx.ReportMessage("SYS>> !�ű���ӽű��ļ���.Eg:!C:\\cpu.txt");
                    scriptRunResult = false;
                }
                return scriptRunResult;
            }
            #endregion

            #region ִ������
            //������Ԥ���� ������������ִ�У���<��COSָ����ָ��
            //����ʲô��������ȴ���$��Ȼ����{}           
            string calCmdL1;
            if (!AnalyzeOperatorGV(line, out calCmdL1)) //���б����滻���
            {
                ctx.ReportMessage("ERR>> Command format error:{0}, GV error.", line);
                return false;
            }
            //�����滻��ɺ�ִ��������к���
            string calCmdL2;
            if (!ExcuteFunctionBlock(calCmdL1, out calCmdL2)) //���к���ִ�����
            {
                ctx.ReportMessage("ERR>> Command format error:{0}, ExcuteFunctionBlock error.", calCmdL1);
                return false;
            }          
            if (calCmdL2.Contains(TARGET_TAG) == false)
            {
                //�Զ�Ϊ�������һ�ε�Header
                string fmtLine = String.Format("{0}{1}{2}", ctx.CmdTarget.Substring(0, ctx.CmdTarget.IndexOf('<')), TARGET_TAG, calCmdL2);
                ctx.ReportMessage("SYS>>�Զ��������TARGET_TAG:ʵ��ִ������:{0}", fmtLine);
                calCmdL2 = fmtLine;              
            }
            //�ȴ���Ŀ��ͷ
            string args;
            //����ִ��
            if (!CommandAnalyze(calCmdL2, out args)) //��������ִ�еĶ���
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
                ctx.ReportMessage("����ʶ�������:{0} {1}", ctx.CmdTarget,args);
                return false;
            }
            
            #endregion
                           
        }

        #endregion

        #region ����������Ԫ�ؽ�������

        //���Ž������ȼ�Ϊ��$�������ȣ�{}���
        //=================================================
        /// <summary>
        /// �������������е�$�����Ӿ�ȫ��������ɣ���$MAC_EKY�ᱻ�滻�������Ķ�����ֵ��
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public bool AnalyzeOperatorGV(string cmd_gv, out string cmd)
        {
            cmd = cmd_gv;
            if (cmd.Contains("$"))
            {
                //����Ҫȡ������ֵ               
                int gvIdx = -1;
                int gvEnd = 0;
                string gv;
                string gvClause;
                #region �滻���б���
                while ((gvIdx = cmd.IndexOf('$')) >= 0)
                {
                    //�ҽ�����                    
                    gvEnd = cmd.IndexOfAny(new char[] { ' ', '\r', '\n', ')', '<', ',', ';', '|','}','\t' } , gvIdx);

                    if (gvEnd == -1)
                    {
                        gvEnd = cmd.Length - 1;
                    }
                    else
                    {
                        --gvEnd;
                    }
                    //�ҵ�һ������������ΪgvIdx��gvEnd֮��Ĳ���
                    if ((gvEnd - gvIdx) > 0 && gvIdx <= (cmd.Length - 1))
                    {
                        gv = cmd.Substring(gvIdx + 1, gvEnd - gvIdx);
                        gvClause = cmd.Substring(gvIdx, gvEnd - gvIdx + 1);

                        //Ĭ�ϵ�ȫ�ֱ���SBUFF,RBUFF
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

                        //�ҵ�һ��ȫ�ֱ���
                        if (ctx.GVDIC.ContainsKey(gv))
                        {
                            ctx.ReportMessage("SYS>> {0} = {1} ", gv, ctx.GVDIC[gv]);
                            //ʹ����������滻ԭ�������
                            cmd = cmd.Replace(gvClause, ctx.GVDIC[gv]);
                            ctx.ReportMessage("SYS>> CMD={0}", cmd);
                            continue;
                        }
                        else
                        {
                            ctx.ReportMessage("ERR>> {0} δ֪��", gv);
                            return false;
                        }
                    }
                }
                #endregion
            }
            return true;
        }

        /// <summary>
        /// ƥ��:
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
        /// �����ض�����䣬������GV����֮��COSָ��ָ��֮ǰ���С�
        /// ��:SAM00{ 80 CA 00 00 09
        /// </summary>
        /// <param name="line"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool CommandAnalyze(string line, out string args)
        {
            args = String.Empty;
            if (line.Contains(TARGET_TAG) && regCmd.IsMatch(line))//����������־
            {
                var rc = regCmd.Match(line);
                if (rc.Groups.Count != 5)
                {
                    ctx.ReportMessage("ERR>>{0} format incorrect .", line);
                    return false;
                }
                ctx.CmdTarget = String.Format("{0}{1}{2}", rc.Groups[1].Value,TARGET_TAG, rc.Groups[3].Value);
                args = rc.Groups[4].Value; //��ʱcmdֻ��������
                ctx.ReportMessage("SYS>>{0} TargetHeader is {1}.", line, ctx.CmdTarget);
                return true;
            }
            ctx.ReportMessage("ERR>>{0} format incorrect .", line);
            return false;           
        }

        /// <summary>
        /// ִ�к��������{}.Ŀǰ��֧��DES/TripDES���㡣
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public bool ExcuteFunctionBlock(string cmdIn, out string cmdOut)
        {
            cmdOut = cmdIn;//�ȱ�����ԭʼ���ַ���           
            string funcBlock = null;
            string statementBody = null;
            string resultStr = null;            
            while (cmdIn.Contains("{") && cmdIn.Contains("}"))
            {
                funcBlock = Utility.GetSubStringBetweenCharsInclude(cmdIn, '{', '}');
                statementBody = Utility.GetSubStringBetweenChars(funcBlock, '{', '}'); 
                ctx.ReportMessage("SYS>> ����:{0}", funcBlock);

                //�ȴ���Ŀ��ͷ
                string args;
                if (!CommandAnalyze(statementBody, out args))
                {
                    ctx.ReportMessage("ERR>> Command format error:{0}, AnalyzeRedirect error.", statementBody);
                    return false;
                }
                if (commandEngine[ctx.CmdTarget].execute(args, ctx)) //�����ִ�гɹ�
                {
                    //��ִ�н���滻
                    resultStr = Utility.ByteArrayToHexStr(ctx.rbuff, ctx.rlen - 2);
                    cmdIn = cmdIn.Replace(funcBlock, resultStr);
                    continue;
                }
                return false;
            }
            cmdOut = cmdIn;//ȫ���������
            return true;
        }
        #endregion

         
    }

    /// <summary>
    /// ������඼��Ϊ�˱���ִ�ж���
    /// </summary>
    public class TagMap
    {
        #region ϵͳ����ӳ�� 00
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

        #region CPU COS����ӳ�� 01

        public const ushort CPU_A = 0x0100;
        public const ushort CPU_B = 0x0101;

        #endregion

        /// <summary>
        /// SAM=02 XX(XXΪslot��).
        /// </summary>
        public const ushort SAM_TAG_PREFIX = 0x0200;

        #region ϵͳ��������ӳ�� 03
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
        /// VAR=04 XX ϵͳ֧�����255��ȫ�ֳ־ñ���.
        /// </summary>
        public const ushort VAR_TAG_PREFIX = 0x0400;
        /// <summary>
        /// TLVģ���ַ�
        /// ����FF[TAG][LENGTH]FF[TAG][LENGTH][CONTENT]
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
        /// Tag�ĸ�2�ֽڡ�
        /// Tag���SYS=00 XX(XXΪϵͳ���ô���),
        /// CPU=01 XX(XX��ʾCPU���typeA, typeB��),
        /// SAM=02 XX(XXΪslot��),
        /// FNC=03 XX(XXΪ��������),
        /// VAR=04 XX ϵͳ֧�����255��ȫ�ֳ־ñ�����
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
        /// Tag����β�ֽڡ�
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
