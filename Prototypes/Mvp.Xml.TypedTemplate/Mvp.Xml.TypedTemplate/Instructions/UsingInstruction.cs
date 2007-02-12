using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom;

namespace Mvp.Xml.TypedTemplate
{
	public class UsingInstruction : ITypeInstruction
	{
		public void Process(string instructionContent, CodeNamespace templateNamespace, 
			CodeTypeDeclaration templateType, CodeMemberMethod renderMethod)
		{
			Guard.ArgumentNotNull(instructionContent, "instructionContent");
			Guard.ArgumentNotNull(templateNamespace, "templateNamespace");
			Guard.ArgumentNotNull(templateType, "templateType");
			Guard.ArgumentNotNull(renderMethod, "renderMethod");

			IDictionary<string, string> props = KeyValueParser.Parse(instructionContent);
			// TODO: throw?
			if (props.ContainsKey("namespace"))
			{
				templateNamespace.Imports.Add(
					new CodeNamespaceImport(props["namespace"]));
			}
		}
	}
}
