using System;
using System.Collections.Generic;
using System.Text;
using Rtp.Driver.RfidReader;
using Rtp.Driver;

using System.IO;
using Rtp.Driver.CardIO;
using Rtp.Driver.Command;

namespace Rtp.Cli
{
    class Program
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static CommandContext ctx = null;
        static RtpCore rtp = null;
        static void Main(string[] args)
        {          

            #region CopyRight
            Console.WriteLine("RFID Test Platform:Release {0} Production on {1}{2}", 
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version,
                DateTime.Now.ToLocalTime(),Environment.NewLine);
            Console.WriteLine("Copyright (c) 1999, 2011, SIASUN.  All rights reserved.");
            Console.WriteLine("-------------------------------------------------------{0}", Environment.NewLine);
            #endregion                         
             

            string line = null;

            #region ¶Á¿¨
            using (RfidD8U rf = new RfidD8U())
            {
                rf.CpuRequest += new EventHandler<Rtp.Driver.CardIO.CpuRequestEventArgs>(rf_CpuRequest);
                rf.CpuResponse += new EventHandler<Rtp.Driver.CardIO.CpuResponseEventArgs>(rf_CpuResponse);
                rf.SamRequest += new EventHandler<SamRequestEventArgs>(rf_SamRequest);
                rf.SamResponse += new EventHandler<SamResponseEventArgs>(rf_SamResponse);
                rf.Open();
                
                ctx = new CommandContext(rf);
                
                rtp = new RtpCore(ctx);
                rtp.CommandExcuter("HELP");

                while ((line = Console.ReadLine()) != "exit")
                {                    
                    line = line.Trim();
                    line = line.ToUpper();
                    if (line.Length > 2)
                    {
                        if (!rtp.CommandExcuter(line))
                        {
                            Console.WriteLine("SYS>> Ö´ÐÐÊ§°Ü!");
                        }
                    }
                }
                rf.CpuRequest -= rf_CpuRequest;
                rf.CpuResponse -= rf_CpuResponse;
            }
            #endregion
           
        }

        static void rf_SamResponse(object sender, SamResponseEventArgs e)
        {
            logger.InfoFormat("{2}>>{0}|{1}", e.ResponseString, rtp.CosIO.GetDescription(e.Sw), ctx.CmdTarget);
             
        }

        static void rf_SamRequest(object sender, SamRequestEventArgs e)
        {
            logger.InfoFormat("{2}<<{0}|{1}", e.Cmdstr, rtp.CosIO.GetDescription(e.Cmd), ctx.CmdTarget);
            
               
        }

        static void rf_CpuResponse(object sender, Rtp.Driver.CardIO.CpuResponseEventArgs e)
        {
            logger.InfoFormat("{2}>>{0}|{1}", e.ResponseString, rtp.CosIO.GetDescription(e.Sw),ctx.CmdTarget);
        }

        static void rf_CpuRequest(object sender, Rtp.Driver.CardIO.CpuRequestEventArgs e)
        {
            logger.InfoFormat("{2}<<{0}|{1}", e.Cmdstr, rtp.CosIO.GetDescription(e.Cmd), ctx.CmdTarget);
        }

    


    }
}
