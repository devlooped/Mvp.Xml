using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace Microsoft.Practices.Mobile.GuiTestRunner
{
	internal class TextBoxTraceListener : TraceListener
	{
		TextBox messagesBox;

		public TextBoxTraceListener(TextBox messagesBox)
		{
			this.messagesBox = messagesBox;
		}

		public override void Write(string message)
		{
			this.messagesBox.Text += message;
		}

		public override void WriteLine(string message)
		{
			this.messagesBox.Text += message + "\r\n";
		}
	}
}
