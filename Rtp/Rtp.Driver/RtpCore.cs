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
        public const string TARGET_HEADER = "<";
  
        public const string COMPOSE = "/";
 
        /// <summary>
        /// �������Ĳ����������ǿ����ٽű��ж���ʹ�õĹؼ���.
        /// </summary>
        public const string ARG_OPERATION = "SET,SAMSLOT,SAMPARA,BUFF,ADD,SUB,PRINT,SAMRESET,EXECUTEMODE";
        /// <summary>
        /// ���������Ĳ���
        /// </summary>
        public const string NONE_ARG_OPRATION = "HELP,OPENREADER,CLOSEREADER,RESETREADER,REQUESTCARD,MACON,MACOFF"; //PAUSE
        /// <summary>
        /// ����������������ֻ���ڿ��е��á�
        /// </summary>
        public const string ARG_FUNCTION = "KEY16MAC,KEY08MAC,DES,TRIPDES,DIVERSIFY,PBOCDESENCKEY16,PBOCDESENCKEY8,PBOCDESDECKEY16,PBOCDESDECKEY8";
        /// <summary>
        /// �޲���������
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

            #region ע������
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
            #endregion

            noneArgOperation.AddRange(NONE_ARG_OPRATION.Split(','));
            argOperation.AddRange(ARG_OPERATION.Split(','));

            noneArgFunction.AddRange(NONE_ARG_FUNCTION.Split(','));
            argFunction.AddRange(ARG_FUNCTION.Split(','));

            cosIO = new FileMapCosIO().ReadCosFile("default.cos");
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
             
            //������Ԥ���� ������������ִ�У���<��COSָ����ָ��
            //����ʲô��������ȴ���$��Ȼ����{}           
            string calCmdL1;
            if (!AnalyzeOperatorGV(line, out calCmdL1)) //���б����滻���
            {
                ctx.ReportMessage("ERR>> Command format error:{0}, GV error.", line);
                return false;
            }
            //�����滻��ɺ�ִ�������ִ�й���
            string calCmdL2;
            if (!ExcuteFunctionBlock(calCmdL1, out calCmdL2)) //���к���ִ�����
            {
                ctx.ReportMessage("ERR>> Command format error:{0}, ExcuteFunctionBlock error.", calCmdL1);
                return false;
            }
            line = calCmdL2;//���ˣ��������$����{}����()
            #region ����COSָ��
            if (line.Contains(TARGET_HEADER))
            {
                //�ȴ���Ŀ��ͷ
                string calCmdL3;
                //����ִ��
                if (!AnalyzeRedirect(calCmdL2, out calCmdL3))
                {
                    ctx.ReportMessage("ERR>> Command format error:{0}, AnalyzeRedirect error.", line);
                    return false;
                }
                if (calCmdL3.Trim().Length < 2) //��ִ���л�������
                {
                    return true;
                }
                line = calCmdL3;
                return commandEngine[ctx.CmdTarget].execute(line, ctx); 
            }
            
            #endregion

            #region DESC����
            //DESC������COS��أ���RTPCORE��ֱ��ִ��
            if (line.StartsWith("DESC "))
            {
                string cos = line.Substring(5);
                byte[] cosbuff = new byte[cos.Length / 2];
                int coslen = Utility.HexStrToByteArray(cos, ref cosbuff);
                if (coslen >= 2) //��ȡCOS����
                {
                    ctx.ReportMessage("SYS>> {0}", cosIO.GetDescription(cosbuff[0], cosbuff[1]));
                    return true;
                }
            }
            #endregion

            #region �޲�ϵͳ���ú�ǰ׺ϵͳ����
            //�޲�������
            if (noneArgOperation.Contains(line)) return commandEngine[line].execute(line, ctx);

            //�в����Ĳ��� "SET,SAM SLOT,SAM PARA,BUFF,ADD";
            foreach (string ao in argOperation)
            {
                if (line.StartsWith(ao)) return commandEngine[ao].execute(line, ctx);
            }
            #endregion            
            //��ͨCOS����
            return commandEngine[ctx.CmdTarget].execute(line, ctx);            
            
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
        /// �����ض�����䣬������GV����֮��COSָ��ָ��֮ǰ���С�
        /// ��:$SAM0 < 80 CA 00 00 09 $SA_CODE
        /// </summary>
        /// <param name="line"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public bool AnalyzeRedirect(string line, out string cmd)
        {
            cmd = line;
            if (line.Contains(TARGET_HEADER))//�����ض������
            {
                int idx = line.IndexOf(TARGET_HEADER);
                //ִ���л�����Ŀ���ָ�Ŀǰ֧��ָ��SAM������CPU��
                if (!commandEngine[TARGET_HEADER].execute(line.Substring(0, idx), ctx)) return false;
                //�ɹ��ż���
                if (idx == (line.Length - 1))
                {
                    ctx.ReportMessage("{0}�л�����Ŀ��ɹ�����������COSָ��.", line);
                    cmd = String.Empty;
                }
                else
                {
                    cmd = line.Substring(idx + 1, line.Length - idx - 1).Trim();//ֻ������������                    
                }
                return true;
            }
            else
            {
                ctx.CmdTarget = "CPU";//�����Ļ��Զ�����ΪCPU
            }
            return true;
        }

        /// <summary>
        /// ִ�к��������{}.Ŀǰ��֧��DES/TripDES���㡣
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public bool ExcuteFunctionBlock(string cmdIn, out string cmdOut)
        {
            cmdOut = cmdIn;//�ȱ�����ԭʼ���ַ���
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
                ctx.ReportMessage("SYS>> ����:{0}", funcBlock);

                //�ֽ����е����
                lxkhIdx = cmdIn.IndexOf('(', lkhIdx); //��С����
                if(lxkhIdx>0) rxkhIdx = cmdIn.IndexOf(')', lxkhIdx);//��С����

                #region ���������ִ��
                if (lxkhIdx < 0 && rxkhIdx < 0)//���������С����
                {
                    //ֱ�������:Date,Time,������ֱ������͵�������COSִ��
                    if (noneArgFunction.Contains(statementBody))
                    {
                        if (!commandEngine[statementBody].execute(statementBody, ctx)) return false;
                        cmdIn = cmdIn.Replace(funcBlock, Utility.ByteArrayToHexStr(ctx.rbuff, ctx.rlen));
                        continue;
                    }
                    
                    //��������޲κ��� ������������                    
                    #region ����COSָ��
                    ctx.ReportMessage("SYS>> Statemants Block is plain cos instructor.");
                    //�ȴ���Ŀ��ͷ
                    string calCmdL3;
                    if (!AnalyzeRedirect(statementBody, out calCmdL3))
                    {
                        ctx.ReportMessage("ERR>> Command format error:{0}, AnalyzeRedirect error.", statementBody);
                        return false;
                    }
                    statementBody = calCmdL3;//���ˣ��������$����{}����(),��<
                    if (commandEngine[ctx.CmdTarget].execute(statementBody, ctx)) //�����ִ�гɹ�
                    {
                        //��ִ�н���滻
                        resultStr = Utility.ByteArrayToHexStr(ctx.rbuff, ctx.rlen - 2);
                        cmdIn = cmdIn.Replace(funcBlock, resultStr);
                        continue;
                    }
                    #endregion
                    return false;
                }
                #endregion

                #region ������ִ��
                if (lxkhIdx < 0 || rxkhIdx < lxkhIdx)
                {
                    ctx.ReportMessage("ERR>> Statemants Block format is incorrect.");
                    return false;
                }
                string funcName = cmdIn.Substring(lkhIdx + 1, lxkhIdx - lkhIdx - 1).Trim().ToUpper();//֪���˺�����
                ctx.ReportMessage("SYS>> Function Name:{0}", funcName);
                //����������ִ��
                if (commandEngine.ContainsKey(funcName) && commandEngine[funcName].execute(statementBody, ctx))
                {
                    //����ִ�гɹ�
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
            cmdOut = cmdIn;//ȫ���������
            return true;
        }
         

        #endregion

         
    }


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
