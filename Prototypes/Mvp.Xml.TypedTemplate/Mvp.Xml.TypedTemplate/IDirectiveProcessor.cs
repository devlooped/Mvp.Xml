using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.CodeDom.Compiler;
using System.CodeDom;

namespace Mvp.Xml.TypedTemplate
{
	public interface IDirectiveProcessor
	{
		void ProcessDirective(IServiceProvider provider, CodeNamespace templateNamespace, CodeTypeDeclaration templateType, IDictionary<string, string> directiveAttributes);
	}
}
