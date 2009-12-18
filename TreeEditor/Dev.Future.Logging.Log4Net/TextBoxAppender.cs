using System;
using System.Collections;
using System.Windows.Forms;
using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;
using log4net.Core;

namespace Dev.Future.Logging.Log4Net
{

    /// <summary>
    /// Implements a log4net appender to send output to a TextBox control.这个是非线程安全的
    /// </summary>
    public class TextBoxAppender : AppenderSkeleton
    {
        private delegate void UpdateControlDelegate(LoggingEvent loggingEvent);
        private void UpdateControl(LoggingEvent loggingEvent)
        {
            if (_textBox.TextLength > 100000)
            {
                _textBox.Clear();
                _textBox.AppendText("达到限制长度100K个字符)");
                _textBox.AppendText(Environment.NewLine);
            }
            // do control updating here
            using (System.IO.StringWriter stringWriter = new System.IO.StringWriter(System.Globalization.CultureInfo.InvariantCulture))
            {
                Layout.Format(stringWriter, loggingEvent);
                _textBox.AppendText(stringWriter.ToString());
                stringWriter.Close();
            }

        }

        private System.Windows.Forms.TextBox _textBox;
        public TextBoxAppender()
        {
        }
        protected override void Append(log4net.Core.LoggingEvent LoggingEvent)
        {
            if (_textBox != null && _textBox.Created)
            {
                if (_textBox.InvokeRequired)
                {
                    _textBox.Invoke(new UpdateControlDelegate(UpdateControl), new object[] { LoggingEvent });
                }
                else
                {
                    UpdateControl(LoggingEvent);
                }
            }


        }
        public TextBox TextBox
        {
            set
            {
                _textBox = value;
            }
            get
            {
                return _textBox;
            }
        }
        public static void SetTextBox(TextBox tb)
        {
            foreach (log4net.Appender.IAppender appender in
                GetAppenders())
            {
                if (appender is TextBoxAppender)
                {
                    ((TextBoxAppender)appender).TextBox
                        = tb;
                }
            }
        }
        private static IAppender[] GetAppenders()
        {
            ArrayList appenders = new ArrayList();
            appenders.AddRange(((Hierarchy)LogManager.GetRepository()).Root.Appenders);
            foreach (ILog log in LogManager.GetCurrentLoggers())
            {
                appenders.AddRange(((Logger)log.Logger).Appenders);
            }
            return
                (IAppender[])appenders.ToArray(typeof(IAppender));
        }
    }
}
