

namespace Dev.Future.Logging.Log4Net
{
    using System;
    using System.Collections;
    using System.Windows.Forms;
    using System.Drawing;
    using log4net;
    using log4net.Appender;
    using log4net.Repository.Hierarchy;
    using System.Collections.Generic;
    using log4net.Core;

    
        /// <summary> 
        /// Description of RichTextBoxAppender. 
        /// </summary> 
        public class RichTextBoxAppender : AppenderSkeleton
        {
            private System.Windows.Forms.RichTextBox _richtextBox;
            public RichTextBoxAppender() { }
            private delegate void UpdateControlDelegate(log4net.Core.LoggingEvent loggingEvent);
            private void UpdateControl(log4net.Core.LoggingEvent loggingEvent)
            {
                // I looked at the TortoiseCVS code to figure out how 
                // to use the RTB as a colored logger. It noted a performance 
                // problem when the buffer got long, so it cleared it every 100K. 
                if (_richtextBox.TextLength > 100000)
                {
                    _richtextBox.Clear();
                    _richtextBox.SelectionColor = Color.Gray;
                    _richtextBox.AppendText("达到限制长度100K个字符)\r\n");
                }
                switch (loggingEvent.Level.ToString())
                {
                    case "INFO": _richtextBox.SelectionColor = System.Drawing.Color.Black; break;
                    case "WARN": _richtextBox.SelectionColor = System.Drawing.Color.Blue; break;
                    case "ERROR": _richtextBox.SelectionColor = System.Drawing.Color.Red; break;
                    case "FATAL": _richtextBox.SelectionColor = System.Drawing.Color.DarkOrange; break;
                    case "DEBUG": _richtextBox.SelectionColor = System.Drawing.Color.DarkGreen; break;
                    default: _richtextBox.SelectionColor = System.Drawing.Color.Black; break;
                }
                 System.IO.StringWriter stringWriter = new System.IO.StringWriter(System.Globalization.CultureInfo.InvariantCulture);
                 Layout.Format(stringWriter, loggingEvent);
                 _richtextBox.AppendText(stringWriter.ToString());
            }

            protected override void Append(log4net.Core.LoggingEvent LoggingEvent)
            {

                if (_richtextBox != null)
                {
                    // make thread safe 
                    if (_richtextBox.InvokeRequired)
                    {
                        _richtextBox.Invoke(new UpdateControlDelegate(UpdateControl), new object[] { LoggingEvent });
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
                rtb.HideSelection = false; // allows rtb to allways append at the end rtb.Clear();
                foreach (log4net.Appender.IAppender appender in GetAppenders())
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
                return (IAppender[])appenders.ToArray(typeof(IAppender)); }

        }
   
}
