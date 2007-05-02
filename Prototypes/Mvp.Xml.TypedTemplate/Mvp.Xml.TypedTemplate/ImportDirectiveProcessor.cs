using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.CodeDom.Compiler;
using System.CodeDom;

namespace Mvp.Xml.TypedTemplate
{
	public class ImportDirectiveProcessor : IDirectiveProcessor
	{
		public void ProcessDirective(IServiceProvider provider, CodeNamespace templateNamespace, CodeTypeDeclaration templateType, IDictionary<string, string> directiveAttributes)
		{
			if (!directiveAttributes.ContainsKey("namespace"))
			{
				throw new ArgumentException(Properties.Resources.Template_ImportDirectiveRequiresNamespace);
			}

			templateNamespace.Imports.Add(new CodeNamespaceImport(directiveAttributes["namespace"]));
		}
	}
}
