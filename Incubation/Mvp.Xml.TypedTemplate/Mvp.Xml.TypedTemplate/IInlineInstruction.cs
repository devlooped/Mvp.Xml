using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom;

namespace Mvp.Xml.TypedTemplate
{
	public interface IInlineInstruction
	{
		CodeStatementCollection Process(string instructionContent);
	}
}
