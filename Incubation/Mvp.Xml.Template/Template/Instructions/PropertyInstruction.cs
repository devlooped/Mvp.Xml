using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom;
using System.ComponentModel.Design;

namespace Mvp.Xml.Template
{
	public class PropertyInstruction : ITypeInstruction
	{
		public void Process(string instructionContent, CodeNamespace templateNamespace, 
			CodeTypeDeclaration templateType, CodeMemberMethod renderMethod)
		{
			Guard.ArgumentNotNull(instructionContent, "instructionContent");
			Guard.ArgumentNotNull(templateNamespace, "templateNamespace");
			Guard.ArgumentNotNull(templateType, "templateType");
			Guard.ArgumentNotNull(renderMethod, "renderMethod");

			IDictionary<string, string> props = KeyValueParser.Parse(instructionContent);
			if (!props.ContainsKey("name") && !props.ContainsKey("type"))
			{
				throw new ArgumentException(Properties.Resources.Template_InvalidPropertyDirective);
			}

			CodeMemberField fld = new CodeMemberField(props["type"], "_" + props["name"].ToLower());
			CodeMemberProperty cmp = new CodeMemberProperty();
			cmp.Name = props["name"];
			cmp.Attributes = MemberAttributes.Public | MemberAttributes.Final;
			cmp.Type = new CodeTypeReference(props["type"]);

			cmp.GetStatements.Add(new CodeMethodReturnStatement(
				new CodeFieldReferenceExpression(
					new CodeThisReferenceExpression(),
					fld.Name
				)
			));
			cmp.SetStatements.Add(new CodeAssignStatement(
				new CodeFieldReferenceExpression(
					new CodeThisReferenceExpression(),
					fld.Name
				),
				new CodeArgumentReferenceExpression("value")
			));

			templateType.Members.Add(fld);
			templateType.Members.Add(cmp);
		}
	}
}
