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
    public class RtpCore
    {
        public const string STATEMENT_BLOCK = "{}";
        public const string TARGET_HEADER = "<";
  
        public const string COMPOSE = "/";
 
        /// <summary>
        /// �������Ĳ����������ǿ����ٽű��ж���ʹ�õĹؼ���.
        /// </summary>
        public const string ARG_OPERATION = "SET,SAM SLOT,SAM PARA,BUFF,ADD,SUB,PRINT,SAM RESET";
        /// <summary>
        /// ���������Ĳ���
        /// </summary>
        public const string NONE_ARG_OPRATION = "HELP,OPEN READER,CLOSE READER,RESET READER,REQUEST CARD,MAC ON,MAC OFF"; //PAUSE
        /// <summary>
        /// ����������������ֻ���ڿ��е��á�
        /// </summary>
        public const string ARG_FUNCTION = "KEY16MAC,KEY08MAC,DES,TRIPDES,DIVERSIFY";
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
             
            #region ������Ԥ���� ������������ִ�У���<��COSָ����ָ��
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
                //����������Ŀ��
                return commandEngine[ctx.CmdTarget].execute(calCmdL3, ctx); 
            }
            //���ˣ��������$����{}����(),��<
            #endregion

            #region ֱ������
            //�������ֱ��ִ��
            if (line.Contains(COMPOSE)) return commandEngine[COMPOSE].execute(line, ctx); //�����������ض��򣬹�����ִ��
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

            #region �޲β�����ǰ׺����
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
                    gvEnd = cmd.IndexOfAny(new char[] { ' ', '\r', '\n', ')', '<', ',', ';', '|' } , gvIdx);

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
                    return false;//�л����
                }
                cmd = line.Substring(idx + 1, line.Length - idx - 1);//ֻ������������
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
}
