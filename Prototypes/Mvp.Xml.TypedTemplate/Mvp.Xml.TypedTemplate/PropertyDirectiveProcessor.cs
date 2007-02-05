using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell.Design;
using System.Diagnostics;
using Microsoft.VisualStudio.Shell.Interop;

namespace Mvp.Xml.TypedTemplate
{
	public class PropertyDirectiveProcessor : IDirectiveProcessor
	{
		public void ProcessDirective(IServiceProvider provider, CodeNamespace templateNamespace, CodeTypeDeclaration templateType, IDictionary<string, string> directiveAttributes)
		{
			if (!directiveAttributes.ContainsKey("name") ||
				!directiveAttributes.ContainsKey("type"))
			{
				throw new ArgumentException(Properties.Resources.Template_ProperyDirectiveMissingAttributes);
			}

			DynamicTypeService typeService = (DynamicTypeService)provider.GetService(typeof(DynamicTypeService));
			Debug.Assert(typeService != null, "No dynamic type service registered.");
			IVsHierarchy hier = VsHelper.GetCurrentHierarchy(provider);
			Debug.Assert(hier != null, "No active hierarchy is selected.");

			ITypeResolutionService resolver = (ITypeResolutionService)typeService.GetTypeResolutionService(hier);
			Type t = resolver.GetType(directiveAttributes["type"], true);

			CodeMemberProperty prop = new CodeMemberProperty();
			prop.Name = directiveAttributes["name"];
			prop.Type = new CodeTypeReference(t);
			prop.Attributes = MemberAttributes.Public | MemberAttributes.Final;

			CodeMemberField field = new CodeMemberField(t, "_" + directiveAttributes["name"].ToLower());
			prop.SetStatements.Add(
				new CodeAssignStatement(
					new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), field.Name),
					new CodeArgumentReferenceExpression("value")
				)
			);
			prop.GetStatements.Add(new CodeMethodReturnStatement(
				new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), field.Name)));

			templateType.Members.Insert(0, field);
			templateType.Members.Add(prop);
		}
	}
}
