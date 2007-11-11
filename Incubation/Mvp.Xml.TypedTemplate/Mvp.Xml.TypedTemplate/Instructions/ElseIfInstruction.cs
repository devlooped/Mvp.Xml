using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom;

namespace Mvp.Xml.Template
{
	public class ElseIfInstruction : IInlineInstruction
	{
		public CodeStatementCollection Process(string instructionContent)
		{
			Guard.ArgumentNotNull(instructionContent, "instructionContent");

			string value = instructionContent;
			if (!value.EndsWith("{")) value += " {";
			value += " //";

			return new CodeStatementCollection(
				new CodeStatement[] {
					new CodeExpressionStatement(
						new CodeSnippetExpression("}\r\n\t\t\telse if " + value)
					)
				}
			);
		}
	}
}
