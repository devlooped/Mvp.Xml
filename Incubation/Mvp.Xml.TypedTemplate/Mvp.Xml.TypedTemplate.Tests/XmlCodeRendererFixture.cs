using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using System.CodeDom;
using System.IO;

namespace Mvp.Xml.Template.Tests
{
	[TestClass]
	public class XmlCodeRendererFixture
	{
		[TestMethod]
		public void CanRenderCode()
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

			XmlReader reader = XmlReader.Create(
				new StringReader(@"<?xml version='1.0' standalone='yes' ?>
				<?template namespace=""foo"" classname=""bar"" ?>
				<?using namespace='System.IO' ?>
				<Element Id='Foo&amp;Bar'>
				<?if (foo) ?>
				<Root />
				<?end?>
				<?foreach (object o in ""hello"") ?>
				<Bar/>
				<?end?>
				</Element>"));

			XmlCodeGenerator renderer = new XmlCodeGenerator(
				typeInst, 
				inlines, 
				"Foo", 
				"Bar"
				);
			CodeNamespace ns = renderer.GenerateCode(reader);

			RenderStatements(ns);
			Console.WriteLine();
		}

		[TestMethod]
		public void ShouldRenderElement()
		{
			XmlReader reader = XmlReader.Create(
				new StringReader(@"<?xml version='1.0' standalone='yes' ?><?foo bar ?><Element Id='Foo&amp;Bar'><![CDATA[foo
				bar
				baz]]>
				<!--Foo-->
				Significant whitespace <span/> here.
				</Element>"));

			//XmlCodeGenerator renderer = new XmlCodeGenerator();
			//CodeStatementCollection statements = renderer.GenerateCode(reader, new CodeArgumentReferenceExpression("writer"));

			//RenderStatements(statements);
			//Console.WriteLine();
		}

		private static void RenderStatements(CodeNamespace ns)
		{
			Microsoft.CSharp.CSharpCodeProvider provider = new Microsoft.CSharp.CSharpCodeProvider();
			provider.GenerateCodeFromNamespace(ns, Console.Out, null);
		}
	}
}
