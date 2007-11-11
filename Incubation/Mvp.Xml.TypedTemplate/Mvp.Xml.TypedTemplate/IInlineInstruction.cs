using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom;

namespace Mvp.Xml.Template
{
	public interface IInlineInstruction
	{
		CodeStatementCollection Process(string instructionContent);
	}
}
