using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Globalization;
using System.IO;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Xml;
using System.ComponentModel.Design;
using Mvp.Xml.TypedTemplate.VisualStudio;

namespace Mvp.Xml.TypedTemplate
{
	[Guid("832AF400-51F5-4c8d-A515-A07C802309AD")]
	[ComVisible(true)]
	[CustomTool("Mvp.Xml.TypedTemplate", ThisAssembly.Description, true)]
	[VersionSupport("8.0")]
	[CategorySupport(CategorySupportAttribute.CSharpCategory)]
	[CategorySupport(CategorySupportAttribute.VBCategory)]
	public class TypedTemplateTool : CustomTool
	{
		#region Constants

		const string GroupOutputMethod = "method";
		const string GroupDirective = "directive";
		const string GroupAttributes = "attributes";
		const string GroupAttributeName = "attrname";
		const string GroupAttributeValue = "attrval";
		const string GroupOpen = "open";
		const string GroupClose = "close";
		const string GroupOutput = "output";
		const string GroupBadMultiple = "badmultiple";
		const string GroupSnippet1 = "snippet1";
		const string GroupSnippet2 = "snippet2";
		const string GroupNewLine1 = "newline1";
		const string GroupNewLine2 = "newline2";

		#endregion

		#region Regular Expressions

		static Regex TemplateExpression = new Regex(@"<\#\s*@\s+Template(?<attributes>.*?)\#>",
			RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);

		static Regex DirectiveAttributesExpression = new Regex(@"\s*(?<attrname>\w+(?=\W))\s*=\s*[""'](?<attrval>[^""']*)[""']",
			RegexOptions.Compiled | RegexOptions.Singleline);

		static Regex TemplateLanguageExpression = new Regex(@"
				# First match the full directives #
				<\#\s*@\s+(?<directive>\w*)(?<attributes>.*?)\#>(\r\n)?[\r\n]? |
				# NewLines
				(?<newline1>\r\n) | 
				(?<newline2>[\r\n]) |
				# Match open tag #
				(?<open><\#)(?!@) |
				# Match close tag #
				(?<close>\#>) |
				# This is a simple expression that is output as-is (i.e. output.Write(<output>); #
				(?:=)(?<output>.*?)(?<badmultiple>;.*?)?(?=\#>) |
				# Anything previous or after a code tag. Serves to break parsing #
				(?<snippet1>[^\r\n]*?)(?=<\#|\#>) |
				# Finally, match everything else that is written as-is #
				(?<snippet2>[^\r\n]*)", 
			RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture| RegexOptions.Compiled | RegexOptions.Multiline);

		static Regex NewLineExpression = new Regex(@"\n",
			RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled | RegexOptions.Multiline);

		#endregion

		protected override string OnGenerateCode(string inputFileName, string inputFileContent)
		{
			string fileName = Path.GetFileName(inputFileName);
			IDictionary<string, IDirectiveProcessor> processors = GetProcessors();
			IServiceProvider provider = new FallbackServiceProvider(
				base.ServiceProvider, VsHelper.GetProjectServiceProvider(base.CurrentProject.Project));

			bool isOpen = false;
			Match templateMatch = TemplateExpression.Match(inputFileContent);
			ThrowIfNoTemplateDirective(templateMatch, inputFileContent);

			Dictionary<string, string> templateAttributes = BuildDirectiveAttributes(inputFileContent, templateMatch, "Template");
			ThrowIfMissingTemplateOutputAttributes(templateAttributes, inputFileContent);
			bool isXml = templateAttributes["output"].ToLower() == "xml";

			bool emitPragmas = true;

			CodeTypeDeclaration templateType = new CodeTypeDeclaration(Path.GetFileNameWithoutExtension(inputFileName));
			templateType.Attributes = MemberAttributes.Family;
			CodeNamespace templateNamespace = new CodeNamespace(base.FileNameSpace);
			templateNamespace.Types.Add(templateType);
			AddCommonImports(templateNamespace);

			CodeMemberMethod renderMethod = new CodeMemberMethod();
			renderMethod.Name = "Render";
			renderMethod.Attributes = MemberAttributes.Public | MemberAttributes.Final;
			templateType.Members.Add(renderMethod);

			CodeExpression outputRef;
			if (isXml)
			{
				// Add StringWriter output variable;
				CodeVariableDeclarationStatement outputVar = new CodeVariableDeclarationStatement(typeof(StringWriter), "output");
				outputVar.InitExpression = new CodeObjectCreateExpression(typeof(StringWriter));
				if (emitPragmas) outputVar.LinePragma = new CodeLinePragma(fileName, 1);
				renderMethod.Statements.Add(outputVar);
				outputRef = new CodeVariableReferenceExpression(outputVar.Name);

				renderMethod.Parameters.Add(new CodeParameterDeclarationExpression(
					typeof(XmlWriter), "writer"));
			}
			else
			{
				renderMethod.Parameters.Add(new CodeParameterDeclarationExpression(
					typeof(TextWriter), "writer"));
				outputRef = new CodeArgumentReferenceExpression("writer");
			}
			
			for (Match m = TemplateLanguageExpression.Match(inputFileContent); m.Success; m = m.NextMatch())
			{
				if (m.Groups[GroupDirective].Success)
				{
					string directiveName = m.Groups[GroupDirective].Value;
					ThrowIfUnknownDirective(directiveName, processors, inputFileContent, m.Index);
					Dictionary<string, string> directiveAttributes = BuildDirectiveAttributes(inputFileContent, m, directiveName);

					processors[directiveName].ProcessDirective(provider, templateNamespace, templateType, directiveAttributes);
				}
				else if (m.Groups[GroupOpen].Success)
				{
					isOpen = true;
				}
				else if (m.Groups[GroupClose].Success)
				{
					isOpen = false;
				}
				else if (m.Groups[GroupSnippet1].Success || m.Groups[GroupSnippet2].Success)
				{
					string value = m.Groups[GroupSnippet1].Value + m.Groups[GroupSnippet2].Value;
					value = value.Replace("\t", "    ");
					if (isOpen)
					{
						// If the open tag has happened, this is template code.
						CodeSnippetStatement st = new CodeSnippetStatement(value);
						if (emitPragmas) st.LinePragma = new CodeLinePragma(fileName, GetLine(inputFileContent, m.Index));
						renderMethod.Statements.Add(st);
					}
					else
					{
						// Otherwise, it's snippet to be output as-is.
						CodeMethodInvokeExpression wl = new CodeMethodInvokeExpression(
							outputRef,
							"Write",
							new CodePrimitiveExpression(value));
						CodeExpressionStatement st = new CodeExpressionStatement(wl);
						if (emitPragmas) st.LinePragma = new CodeLinePragma(fileName, GetLine(inputFileContent, m.Index));
						renderMethod.Statements.Add(st);
					}
				}
				else if (m.Groups[GroupOutput].Success)
				{
					if (m.Groups[GroupBadMultiple].Success)
					{
						throw new TemplateException(Properties.Resources.Template_OutputMultipleStatements,
							inputFileContent, m.Index);
					}

					CodeMethodInvokeExpression write = new CodeMethodInvokeExpression(
						outputRef,
						"Write",
						new CodeSnippetExpression(m.Groups[GroupOutput].Value));
					CodeExpressionStatement st = new CodeExpressionStatement(write);
					if (emitPragmas) st.LinePragma = new CodeLinePragma(fileName, GetLine(inputFileContent, m.Index));
					renderMethod.Statements.Add(st);
				}
				else if (m.Groups[GroupNewLine1].Success || m.Groups[GroupNewLine2].Success)
				{
					if (isOpen)
					{
						// If the open tag has happened, this is template code.
						CodeSnippetStatement st = new CodeSnippetStatement(Environment.NewLine);
						if (emitPragmas) st.LinePragma = new CodeLinePragma(fileName, GetLine(inputFileContent, m.Index));
						renderMethod.Statements.Add(st);
					}
					else
					{
						// Otherwise, it's snippet newline
						CodeMethodInvokeExpression wl = new CodeMethodInvokeExpression(
							outputRef,
							"WriteLine");
						CodeExpressionStatement st = new CodeExpressionStatement(wl);
						if (emitPragmas) st.LinePragma = new CodeLinePragma(fileName, GetLine(inputFileContent, m.Index));
						renderMethod.Statements.Add(st);
					}
				}
				else
				{
					// Nothing matched??
					throw new TemplateException(Properties.Resources.Template_CantParse,
						inputFileContent, m.Index);
				}
			}

			if (isXml)
			{
				// Add rendering of XML to writer.
				// writer.WriteNode(XmlReader.Create(new StringReader(output.ToString())));
				renderMethod.Statements.Add(
					new CodeMethodInvokeExpression(
						new CodeArgumentReferenceExpression("writer"),
						"WriteNode",
						new CodeMethodInvokeExpression(
							new CodeTypeReferenceExpression(typeof(XmlReader)), 
							"Create",
							new CodeObjectCreateExpression(
								typeof(StringReader),
								new CodeMethodInvokeExpression(
									outputRef,
									"ToString"
								)
							)
						), 
						new CodePrimitiveExpression(false)
					)
				);
				// writer.Flush();
				renderMethod.Statements.Add(new CodeMethodInvokeExpression(
					new CodeArgumentReferenceExpression("writer"),
					"Flush"));
			}

			OptimizeWriteLines(renderMethod);

			StringWriter output = new StringWriter();
			CodeGeneratorOptions codeOptions = new CodeGeneratorOptions();
			codeOptions.BracingStyle = "C";
			base.CodeProvider.GenerateCodeFromNamespace(templateNamespace, output, codeOptions);
			output.Flush();
			return output.ToString();
		}

		private void OptimizeWriteLines(CodeMemberMethod renderMethod)
		{
			List<CodeStatement> toRemove = new List<CodeStatement>();

			for (int i = 0; i < renderMethod.Statements.Count; i++)
			{
				CodeExpressionStatement st = renderMethod.Statements[i] as CodeExpressionStatement;
				if (st != null)
				{
					CodeMethodInvokeExpression write = st.Expression as CodeMethodInvokeExpression;
					if (write != null && write.Method.MethodName == "Write" && i < renderMethod.Statements.Count - 1)
					{
						// Look ahead for a plain WriteLine.
						st = renderMethod.Statements[i + 1] as CodeExpressionStatement;
						if (st != null)
						{
							CodeMethodInvokeExpression writeLine = st.Expression as CodeMethodInvokeExpression;
							if (writeLine != null && writeLine.Method.MethodName == "WriteLine" &&
								writeLine.Parameters.Count == 0)
							{
								toRemove.Add(st);
								write.Method.MethodName = "WriteLine";
							}
						}
					}
				}
			}

			foreach (CodeStatement st in toRemove)
			{
				renderMethod.Statements.Remove(st);
			}
		}

		private static void AddCommonImports(CodeNamespace templateNamespace)
		{
			templateNamespace.Imports.Add(new CodeNamespaceImport("System"));
			templateNamespace.Imports.Add(new CodeNamespaceImport("System.IO"));
		}

		private IDictionary<string, IDirectiveProcessor> GetProcessors()
		{
			Dictionary<string, IDirectiveProcessor> processors = new Dictionary<string, IDirectiveProcessor>(StringComparer.CurrentCultureIgnoreCase);
			processors.Add("import", new ImportDirectiveProcessor());
			processors.Add("template", new TemplateDirectiveProcessor());
			processors.Add("property", new PropertyDirectiveProcessor());

			return processors;
		}

		private static Dictionary<string, string> BuildDirectiveAttributes(string inputFileContent, Match m, string directiveName)
		{
			// Directive attributes are case-insensitive. This is compatible with ASP.NET and DSL Tools T4 syntax.
			Dictionary<string, string> props = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
			if (m.Groups[GroupAttributes].Success)
			{
				for (Match attr = DirectiveAttributesExpression.Match(m.Groups[GroupAttributes].Value.Trim());
					attr.Success; attr = attr.NextMatch())
				{
					if (props.ContainsKey(attr.Groups[GroupAttributeName].Value))
					{
						throw new TemplateException(String.Format(
							CultureInfo.CurrentCulture,
							Properties.Resources.Template_DuplicateDirectiveAttribute,
							attr.Groups[GroupAttributeName].Value,
							directiveName), inputFileContent, attr.Index);
					}

					props.Add(
						attr.Groups[GroupAttributeName].Value,
						attr.Groups[GroupAttributeValue].Value);
				}
			}
			return props;
		}

		private void ThrowIfMissingTemplateOutputAttributes(Dictionary<string, string> templateAttributes, string templateContent)
		{
			if (!templateAttributes.ContainsKey("output"))
			{
				throw new TemplateException(Properties.Resources.Template_OutputMethodMissing, templateContent, 1);
			}
		}

		private void ThrowIfNoTemplateDirective(Match templateMatch, string templateContent)
		{
			if (!templateMatch.Success)
			{
				throw new TemplateException(Properties.Resources.Template_MainDirectiveMissing, templateContent, 1);
			}
		}

		private static void ThrowIfUnknownDirective(string directiveName, IDictionary<string, IDirectiveProcessor> processors, string inputFileContent, int parseIndex)
		{
			if (!processors.ContainsKey(directiveName))
			{
				throw new TemplateException(String.Format(
					CultureInfo.CurrentCulture,
					Properties.Resources.Template_UnknownDirective,
					directiveName),
					inputFileContent, parseIndex);
			}
		}

		private static int GetLine(string templateContent, int index)
		{
			return NewLineExpression.Matches(templateContent.Substring(0, index)).Count + 1;
		}

		public class TemplateException : InvalidOperationException
		{
			public TemplateException(string message, string templateContent, int failIndex)
				: base(BuildMessage(message, templateContent, failIndex))
			{
			}

			private static string BuildMessage(string message, string templateContent, int failIndex)
			{
				int line = GetLine(templateContent, failIndex);

				return String.Format(CultureInfo.CurrentCulture,
					Properties.Resources.Template_Exception,
					line,
					message);
			}
		}

		private class TemplateDirectiveProcessor : IDirectiveProcessor
		{
			public void ProcessDirective(IServiceProvider provider, CodeNamespace templateNamespace, CodeTypeDeclaration templateType, IDictionary<string, string> directiveAttributes)
			{
				// Processing for Template directive is special.
			}
		}

		#region Registration and Installation

		/// <summary>
		/// Registers the generator.
		/// </summary>
		[ComRegisterFunction]
		public static void RegisterClass(Type type)
		{
			CustomTool.Register(typeof(TypedTemplateTool));
		}

		/// <summary>
		/// Unregisters the generator.
		/// </summary>
		[ComUnregisterFunction]
		public static void UnregisterClass(Type t)
		{
			CustomTool.UnRegister(typeof(TypedTemplateTool));
		}

		#endregion
	}
}
