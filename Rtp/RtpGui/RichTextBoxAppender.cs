using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;
using System.Drawing;
using log4net.Core;
namespace Rtp.Gui
{
 /// <summary>
        /// Description of RichTextBoxAppender.
        /// </summary>
        public class RichTextBoxAppender : AppenderSkeleton
        {
                private System.Windows.Forms.RichTextBox _richtextBox;

                public RichTextBoxAppender()
                {
                }
               
                private delegate void UpdateControlDelegate(LoggingEvent loggingEvent);

                private void UpdateControl(log4net.Core.LoggingEvent loggingEvent)
                {
                    if (_richtextBox == null) return;
                        // I looked at the TortoiseCVS code to figure out how
                        // to use the RTB as a colored logger.  It noted a performance
                        // problem when the buffer got long, so it cleared it every 100K.
                        if(_richtextBox.TextLength > 100000)
                        {
                                _richtextBox.Clear();
                                _richtextBox.SelectionColor = Color.Gray;
                                _richtextBox.AppendText("(earlier messages cleared because of log length)\n\n");
                        }
                       
                        switch(loggingEvent.Level.ToString()) {
                                case "INFO":
                                        _richtextBox.SelectionColor =System.Drawing.Color.Black;
                                        break;
                                case "WARN":
                                        _richtextBox.SelectionColor =System.Drawing.Color.Blue;
                                        break;
                                case "ERROR":
                                        _richtextBox.SelectionColor =System.Drawing.Color.Red;
                                        break;
                                case "FATAL":
                                        _richtextBox.SelectionColor =System.Drawing.Color.DarkOrange;
                                        break;
                                case "DEBUG":
                                        _richtextBox.SelectionColor =System.Drawing.Color.DarkGreen;
                                        break;
                                default:
                                        _richtextBox.SelectionColor =System.Drawing.Color.Black;
                                        break;
                        }

                        using (System.IO.StringWriter sw = new System.IO.StringWriter())
                        {
                            Layout.Format(sw, loggingEvent);
                            _richtextBox.AppendText(sw.ToString());
                        }
                    
                }

                protected override void Append (LoggingEvent LoggingEvent)
                {
                        // prevent exceptions
                        //if (_richtextBox != null && _richtextBox.Created)
                        if (_richtextBox != null)
                        {
                                // make thread safe
                                if (_richtextBox.InvokeRequired)
                                {
                                        _richtextBox.Invoke( new UpdateControlDelegate(UpdateControl),LoggingEvent);
                                }                        
                                else 
                                {
                                        UpdateControl(LoggingEvent);
                                }
                        }
                }


                public RichTextBox RichTextBox
                {
                        set
                        {
                                _richtextBox = value;
                        }
                        get
                        {
                                return _richtextBox;
                        }
                }

                public static void SetRichTextBox(RichTextBox rtb)
                {
                        rtb.ReadOnly = true;
                        rtb.HideSelection = false;      // allows rtb to allways append at the end
                        rtb.Clear();

                        foreach(log4net.Appender.IAppender appender in GetAppenders())
                        {
                                if (appender is RichTextBoxAppender)
                                {
                                    ((RichTextBoxAppender)appender).RichTextBox = rtb;
                                }
                        }
                }

                private static IAppender[] GetAppenders()
                {
                        
                    ArrayList appenders = new ArrayList();
                    appenders.AddRange(((Hierarchy)LogManager.GetRepository()).Root.Appenders);

                        foreach(ILog log in LogManager.GetCurrentLoggers())
                        {
                            appenders.AddRange(((Logger)log.Logger).Appenders);
                        }

                        return (IAppender[])appenders.ToArray(typeof(IAppender));
                }

        }
}
