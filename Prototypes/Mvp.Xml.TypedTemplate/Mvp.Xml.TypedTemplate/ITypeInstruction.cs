using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom;

namespace Mvp.Xml.TypedTemplate
{
	public interface ITypeInstruction
	{
		void Process(string instructionContent, CodeNamespace templateNamespace, 
			CodeTypeDeclaration templateType, CodeMemberMethod renderMethod);
	}
}
