using System;
using System.Collections.Generic;
using System.Text;

namespace Siasun.Gui.Components
{
    public class TextBoxTraceListener : System.Diagnostics.TraceListener
    {
        private System.Windows.Forms.TextBox traceEdit;  // Text box that displays the trace messages.

        public TextBoxTraceListener(System.Windows.Forms.TextBox traceEdit_)
        {
            traceEdit = traceEdit_;
        }

 
        public override void Write(string message)    // You define these and put the message into your textbox (traceEdit).
        {
            if (traceEdit.Lines.Length > 3000) traceEdit.Clear();
            //traceEdit.AppendText(DateTime.Now.ToShortTimeString());
            //traceEdit.AppendText(":");
            traceEdit.AppendText(message);           
        }
        public override void WriteLine(string message)   // You define these and put the message into your textbox (traceEdit).
        {
            Write(message);
            traceEdit.AppendText(Environment.NewLine);           
        }

       
    } 
}
