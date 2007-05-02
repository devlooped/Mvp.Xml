using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using System.IO;
using System.CodeDom;

namespace Mvp.Xml.TypedTemplate.Tests
{
	[TestClass]
	public class RegexTests
	{
		[TestMethod]
		public void ShouldRemoveMatchedText()
		{
			Regex re = new Regex("foo");
			string text = "hello foo world";

			Match m = re.Match(text);

			string result = text.Substring(0, m.Index);
			result += text.Substring(m.Index + m.Length);

			Assert.AreEqual("hello  world", result);
		}

		[TestMethod]
		public void GetKeywordFromCodeDom()
		{
			CodeStatement st = new CodeExpressionStatement(new CodeArgumentReferenceExpression("foo"));
			CodeExpression exp = new CodeArgumentReferenceExpression("foo");
			CodeIterationStatement it = new CodeIterationStatement(st, exp, st);

			CodeConditionStatement cond = new CodeConditionStatement(exp);

			new Microsoft.CSharp.CSharpCodeProvider().GenerateCodeFromStatement(
				it, Console.Out, new System.CodeDom.Compiler.CodeGeneratorOptions());
			new Microsoft.CSharp.CSharpCodeProvider().GenerateCodeFromStatement(
				cond, Console.Out, new System.CodeDom.Compiler.CodeGeneratorOptions());

			new Microsoft.VisualBasic.VBCodeProvider().GenerateCodeFromStatement(
				it, Console.Out, new System.CodeDom.Compiler.CodeGeneratorOptions());
			new Microsoft.VisualBasic.VBCodeProvider().GenerateCodeFromStatement(
				cond, Console.Out, new System.CodeDom.Compiler.CodeGeneratorOptions());
		}

		[Ignore]
		[TestMethod]
		public void TestRender()
		{
			TestTemplateTool tool = new TestTemplateTool();

			Console.WriteLine(tool.GenerateCode("Template1.ipe"));
		}

		class TestTemplateTool : TypedTemplateTool
		{
			public TestTemplateTool()
			{
				base.CodeProvider = new Microsoft.CSharp.CSharpCodeProvider();
			}

			public string GenerateCode(string inputFile)
			{
				return base.OnGenerateCode(inputFile, File.ReadAllText(inputFile));
			}
		}

		[TestMethod]
		public void TestLanguage()
		{
			Regex TemplateLanguageExpression = new Regex(@"
				# First match the full directives #
				^<\#\s*@\s+(?<directive>\w*)(?<attributes>.*?)\#>$ |
				# Match open tag #
				(?<open><\#)(?!@) |
				# Match close tag #
				(?<close>\#>) |
				# This is a simple expression that is output as-is (i.e. output.Write(<output>); #
				(?:=)(?<output>.*?)(?<badmultiple>;.*?)?(?=\#>) |
				# Anything previous or after a code tag. Serves to break parsing #
				(?<snippet1>[^\r\n]*?)(?=<\#|\#>) |
				# Finally, match everything else that is written as-is #
				(?<snippet2>[^\r\n]*?) |
				# NewLines
				(?<newline>[\r\n])", 
				RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture| RegexOptions.Compiled | RegexOptions.Multiline);

			string input = @"
<#@ Template Language=""VB"" #>
<#@ Import Namespace=""System"" #>
<# ' Retrieve parameters for use in the template
	Dim name As String           = ""foo""
	Dim ns As String             = ""bar""
#>#region info
//===============================================================================
// Microsoft Services Reference Architecture
// <#= name #>.cs
//
using System;
using Microsoft.ReferenceArchitecture.Services;

namespace <#= ns #>
{
      	
}";

			for (Match m = TemplateLanguageExpression.Match(input); m.Success; m = m.NextMatch())
			{
				Console.WriteLine(m.ToString());
			}
		}
	}
}
