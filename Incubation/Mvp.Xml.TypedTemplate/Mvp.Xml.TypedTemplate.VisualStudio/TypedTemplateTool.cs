using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Xml;
using Mvp.Xml.Template.VisualStudio;
using System.IO;
using System.CodeDom;
using System.CodeDom.Compiler;

namespace Mvp.Xml.Template
{
	[Guid("832AF400-51F5-4c8d-A515-A07C802309AD")]
	[ComVisible(true)]
	[CustomTool("XmlTemplate", ThisAssembly.Description, true)]
	[VersionSupport("8.0")]
	[CategorySupport(CategorySupportAttribute.CSharpCategory)]
	public class TypedTemplateTool : CustomTool
	{
		public override string GetDefaultExtension()
		{
			return ".Designer" + base.GetDefaultExtension();
		}

		protected override string OnGenerateCode(string inputFileName, string inputFileContent)
		{
			// TODO: Build list of processing instructions from config?
			XmlReaderSettings set = new XmlReaderSettings();
			set.IgnoreWhitespace = true;
			set.CheckCharacters = true;
			using (XmlReader reader = XmlReader.Create(inputFileName, set))
			{
				Dictionary<string, IInlineInstruction> inlines = new Dictionary<string, IInlineInstruction>();
				inlines.Add("foreach", new ForEachInstruction());
				inlines.Add("if", new IfInstruction());
				inlines.Add("elseif", new ElseIfInstruction());
				inlines.Add("else", new ElseInstruction());
				inlines.Add("end", new EndInstruction());

				Dictionary<string, ITypeInstruction> typeInst = new Dictionary<string, ITypeInstruction>();
				typeInst.Add("template", new TemplateInstruction());
				typeInst.Add("using", new UsingInstruction());
				typeInst.Add("property", new PropertyInstruction());

				XmlCodeGenerator generator = new XmlCodeGenerator(
					typeInst,
					inlines,
					FileNameSpace,
					Path.GetFileNameWithoutExtension(inputFileName));

				CodeNamespace ns = generator.GenerateCode(reader);
				return GenerateCode(ns);
			}
		}

		private string GenerateCode(CodeNamespace templateNamespace)
		{
			StringWriter output = new StringWriter();
			CodeGeneratorOptions codeOptions = new CodeGeneratorOptions();
			codeOptions.BracingStyle = "C";
			base.CodeProvider.GenerateCodeFromNamespace(templateNamespace, output, codeOptions);
			output.Flush();
			return output.ToString();
		}

		private static void EmitPragma(bool emitPragmas, CodeStatement statement, string fileName, int line)
		{
			if (emitPragmas)
			{
				statement.LinePragma = new CodeLinePragma(fileName, line);
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
		public static void UnregisterClass(Type type)
		{
			CustomTool.UnRegister(typeof(TypedTemplateTool));
		}

		#endregion
	}
}
