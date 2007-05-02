using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.CodeDom;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Mvp.Xml.TypedTemplate
{
	public class XmlCodeGenerator
	{
		delegate CodeStatementCollection GenerateFunction(XmlReader reader, CodeExpression writerExpression);

		Regex stripWhitespace = new Regex("[\r\n\t]*$", RegexOptions.Compiled);
		Regex outputExpression = new Regex("(?<!\\$)\\${(?<expression>.+?)}", RegexOptions.Compiled);
		IDictionary<XmlNodeType, GenerateFunction> generationFunctions;
		IDictionary<string, ITypeInstruction> typeInstructions;
		IDictionary<string, IInlineInstruction> inlineInstructions;
		string defaultTargetNamespace;
		string defaultTypeName;

		public XmlCodeGenerator(
			IDictionary<string, ITypeInstruction> typeInstructions,
			IDictionary<string, IInlineInstruction> inlineInstructions,
			string defaultTargetNamespace, string defaultTypeName)
		{
			this.typeInstructions = typeInstructions;
			this.inlineInstructions = inlineInstructions;
			this.defaultTargetNamespace = defaultTargetNamespace;
			this.defaultTypeName = defaultTypeName;

			generationFunctions = new Dictionary<XmlNodeType, GenerateFunction>();
			generationFunctions.Add(XmlNodeType.CDATA, GenerateCDATAImpl);
			generationFunctions.Add(XmlNodeType.Comment, GenerateCommentImpl);
			generationFunctions.Add(XmlNodeType.Element, GenerateElementImpl);
			generationFunctions.Add(XmlNodeType.EndElement, GenerateEndElementImpl);
			generationFunctions.Add(XmlNodeType.SignificantWhitespace, GenerateWhitespaceImpl);
			generationFunctions.Add(XmlNodeType.Whitespace, GenerateWhitespaceImpl);
			generationFunctions.Add(XmlNodeType.Text, GenerateTextImpl);
			generationFunctions.Add(XmlNodeType.XmlDeclaration, GenerateXmlDeclarationImpl);
		
			generationFunctions.Add(XmlNodeType.Document, UnsupportedNodeTypeImpl);
			generationFunctions.Add(XmlNodeType.DocumentFragment, UnsupportedNodeTypeImpl);
			generationFunctions.Add(XmlNodeType.DocumentType, UnsupportedNodeTypeImpl);
			generationFunctions.Add(XmlNodeType.EndEntity, UnsupportedNodeTypeImpl);
			generationFunctions.Add(XmlNodeType.Entity, UnsupportedNodeTypeImpl);
			generationFunctions.Add(XmlNodeType.EntityReference, UnsupportedNodeTypeImpl);
			generationFunctions.Add(XmlNodeType.None, UnsupportedNodeTypeImpl);
			generationFunctions.Add(XmlNodeType.Notation, UnsupportedNodeTypeImpl);
		}

		public CodeNamespace GenerateCode(XmlReader reader)
		{
			CodeTypeDeclaration templateType = new CodeTypeDeclaration(defaultTypeName);
			templateType.Attributes = MemberAttributes.Family;
			templateType.IsPartial = true;
			CodeNamespace templateNamespace = new CodeNamespace(defaultTargetNamespace);
			templateNamespace.Types.Add(templateType);
			AddCommonImports(templateNamespace);

			CodeMemberMethod renderMethod = new CodeMemberMethod();
			renderMethod.Name = "Render";
			renderMethod.Attributes = MemberAttributes.Public | MemberAttributes.Final;
			renderMethod.Parameters.Add(new CodeParameterDeclarationExpression(
				typeof(XmlWriter), "writer"));

			CodeArgumentReferenceExpression writerExpression = new CodeArgumentReferenceExpression("writer");
			templateType.Members.Add(renderMethod);

			templateType.Members.Add(BuildConverter());

			while (reader.Read())
			{
				if (reader.NodeType == XmlNodeType.ProcessingInstruction)
				{
					ThrowIfUnknownInstruction(reader);

					if (typeInstructions.ContainsKey(reader.LocalName))
					{
						ITypeInstruction ti = typeInstructions[reader.LocalName];
						ti.Process(reader.Value, templateNamespace, templateType, renderMethod);
					}
					if (inlineInstructions.ContainsKey(reader.LocalName))
					{
						IInlineInstruction ii = inlineInstructions[reader.LocalName];
						CodeStatementCollection col = ii.Process(reader.Value);
						renderMethod.Statements.AddRange(col);
					}
				}
				else
				{
					renderMethod.Statements.AddRange(
						generationFunctions[reader.NodeType](reader, writerExpression));
				}
			}

			return templateNamespace;
		}

		private CodeTypeDeclaration BuildConverter()
		{
			// This inner private class is required because the 
			// XmlConvert .NET class does not receive a string 
			// as argument, and we cannot know the type 
			// of a code snippet, therefore we need to 
			// add support for that signature overload 
			// and let the compiler do the work. The overhead 
			// is minimal.

			//class Converter : XmlConvert
			//{
			//    public static string ToString(string value)
			//    {
			//        return value;
			//    }
			//}

			CodeTypeDeclaration conv = new CodeTypeDeclaration("Converter");
			conv.Attributes = MemberAttributes.Private | MemberAttributes.Static;
			conv.BaseTypes.Add(typeof(XmlConvert));
			
			CodeMemberMethod method = new CodeMemberMethod();
			method.Attributes = MemberAttributes.Public | MemberAttributes.Static;
			method.Name = "ToString";
			method.ReturnType = new CodeTypeReference(typeof(string));
			method.Parameters.Add(new CodeParameterDeclarationExpression(
				typeof(string), "value"));
			method.Statements.Add(new CodeMethodReturnStatement(
				new CodeArgumentReferenceExpression("value")));
			conv.Members.Add(method);

			return conv;
		}

		//private void StripWhitespaceFromPreviousText(CodeMemberMethod renderMethod)
		//{
		//    if (renderMethod.Statements.Count == 0) return;
		//    CodeStatement st = renderMethod.Statements[renderMethod.Statements.Count - 1];
		//    CodeExpressionStatement exp = st as CodeExpressionStatement;
		//    if (exp != null)
		//    {
		//        CodeMethodInvokeExpression mi = exp.Expression as CodeMethodInvokeExpression;
		//        if (mi != null)
		//        {
		//            if (mi.Method.MethodName == "WriteString")
		//            {
		//                CodePrimitiveExpression prim = mi.Parameters[0] as CodePrimitiveExpression;
		//                if (prim != null)
		//                {
		//                    prim.Value = stripWhitespace.Replace(prim.Value.ToString(), "");
		//                }
		//            }
		//        }
		//    }
		//}

		private void ThrowIfUnknownInstruction(XmlReader reader)
		{
			if (!typeInstructions.ContainsKey(reader.LocalName) &&
				!inlineInstructions.ContainsKey(reader.LocalName))
			{
				IXmlLineInfo lineInfo = reader as IXmlLineInfo ?? new NullLineInfo();
				throw new TemplateException(
					String.Format(
						CultureInfo.CurrentCulture,
						Properties.Resources.Template_UnknownInstruction,
						reader.LocalName),
					lineInfo.LineNumber,
					lineInfo.LinePosition);
			}
		}

		private void AddCommonImports(CodeNamespace templateNamespace)
		{
			templateNamespace.Imports.Add(new CodeNamespaceImport("System"));
			templateNamespace.Imports.Add(new CodeNamespaceImport("System.Xml"));
		}

		private CodeStatementCollection GenerateCDATAImpl(XmlReader reader, CodeExpression writerExpression)
		{
			return new CodeStatementCollection(new CodeStatement[] {
				new CodeExpressionStatement(new CodeMethodInvokeExpression(
					writerExpression,
					"WriteCData",
					new CodePrimitiveExpression(reader.Value)))
			});
		}

		private CodeStatementCollection GenerateCommentImpl(XmlReader reader, CodeExpression writerExpression)
		{
			return new CodeStatementCollection(new CodeStatement[] {
				new CodeExpressionStatement(new CodeMethodInvokeExpression(
					writerExpression,
					"WriteComment",
					new CodePrimitiveExpression(reader.Value)))
			});
		}

		private CodeStatementCollection GenerateElementImpl(XmlReader reader, CodeExpression writerExpression)
		{
			CodeStatementCollection statements = new CodeStatementCollection();
			statements.Add(new CodeMethodInvokeExpression(
				writerExpression,
				"WriteStartElement",
				new CodePrimitiveExpression(reader.Prefix),
				new CodePrimitiveExpression(reader.LocalName),
				new CodePrimitiveExpression(reader.NamespaceURI)));
			// Add code for attributes.
			for (bool go = reader.MoveToFirstAttribute(); go; go = reader.MoveToNextAttribute())
			{
				statements.Add(new CodeMethodInvokeExpression(
					writerExpression,
					"WriteAttributeString",
					new CodePrimitiveExpression(reader.Prefix),
					new CodePrimitiveExpression(reader.LocalName),
					new CodePrimitiveExpression(reader.NamespaceURI),
					BuildValueExpression(reader.Value)));
			}

			if (reader.HasAttributes) reader.MoveToElement();

			if (reader.IsEmptyElement)
			{
				statements.AddRange(GenerateEndElementImpl(reader, writerExpression));
			}

			return statements;
		}

		private CodeExpression BuildValueExpression(string xmlString)
		{
			// Build concatenating expression for each occurrence of 
			// the ${} keyword.
			Match m = outputExpression.Match(xmlString);

			if (!m.Success)
			{
				// No match, it's just a literal.
				return new CodePrimitiveExpression(xmlString);
			}
			else if (m.Length == xmlString.Length)
			{
				// Simplified match where the entire string is the expression: ${foo}
				return ExpressionToString(m.Groups["expression"].Value);
			}

			// We need to concatenate all the snippet expressions with the 
			// literals.
			int lastIndex = 0;
			
			CodeBinaryOperatorExpression result = new CodeBinaryOperatorExpression();
			result.Left = new CodePrimitiveExpression(xmlString.Substring(lastIndex, m.Index - lastIndex));
			result.Operator = CodeBinaryOperatorType.Add;
			result.Right = ExpressionToString(m.Groups["expression"].Value);
			lastIndex = m.Index + m.Length;

			CodeBinaryOperatorExpression current = result;
			for (m = m.NextMatch(); m.Success; m = m.NextMatch())
			{
				CodeBinaryOperatorExpression exp = new CodeBinaryOperatorExpression(
					new CodePrimitiveExpression(xmlString.Substring(lastIndex, m.Index - lastIndex)),
					CodeBinaryOperatorType.Add,
					ExpressionToString(m.Groups["expression"].Value));

				current.Right = new CodeBinaryOperatorExpression(
					current.Right,
					CodeBinaryOperatorType.Add,
					exp);
				current = exp;
				lastIndex = m.Index + m.Length;
			}

			current.Right = new CodeBinaryOperatorExpression(
				current.Right,
				CodeBinaryOperatorType.Add,
				new CodePrimitiveExpression(xmlString.Substring(lastIndex)));

			return result;
		}

		private static CodeExpression ExpressionToString(string value)
		{
			// Calls the private inner class generated on BuildConverter.
			return new CodeMethodInvokeExpression(
				new CodeTypeReferenceExpression("Converter"),
				"ToString",
				new CodeSnippetExpression(value));
		}

		private CodeStatementCollection GenerateEndElementImpl(XmlReader reader, CodeExpression writerExpression)
		{
			return new CodeStatementCollection(new CodeStatement[] {
				new CodeExpressionStatement(new CodeMethodInvokeExpression(
					writerExpression,
					"WriteEndElement"))
			});
		}

		private CodeStatementCollection GenerateWhitespaceImpl(XmlReader reader, CodeExpression writerExpression)
		{
			return new CodeStatementCollection(new CodeStatement[] {
				new CodeExpressionStatement(new CodeMethodInvokeExpression(
					writerExpression,
					"WriteWhitespace", 
					new CodePrimitiveExpression(reader.Value)))
			});
		}

		private CodeStatementCollection GenerateTextImpl(XmlReader reader, CodeExpression writerExpression)
		{
			return new CodeStatementCollection(new CodeStatement[] {
				new CodeExpressionStatement(new CodeMethodInvokeExpression(
					writerExpression,
					"WriteString",
					BuildValueExpression(reader.Value)))
			});
		}

		private CodeStatementCollection GenerateXmlDeclarationImpl(XmlReader reader, CodeExpression writerExpression)
		{
			string attr = reader["standalone"];
			bool standalone = String.Equals(attr, "yes", StringComparison.Ordinal);
			CodeExpression expr;
			if (!String.IsNullOrEmpty(attr))
			{
				expr = new CodeMethodInvokeExpression(
					writerExpression,
					"WriteStartDocument",
					new CodePrimitiveExpression(standalone));
			}
			else
			{
				expr = new CodeMethodInvokeExpression(
					writerExpression,
					"WriteStartDocument");
			}

			return new CodeStatementCollection(new CodeStatement[] { 
				new CodeExpressionStatement(expr) 
			});
		}

		private CodeStatementCollection UnsupportedNodeTypeImpl(XmlReader reader, CodeExpression writerExpression)
		{
			return new CodeStatementCollection(new CodeStatement[] {
				new CodeCommentStatement(
					String.Format(
						"Calling PI: {0}, {1}", 
						reader.LocalName, reader.Value))
			});
		}

		class NullLineInfo : IXmlLineInfo
		{
			public bool HasLineInfo()
			{
				return true;
			}

			public int LineNumber
			{
				get { return 0; }
			}

			public int LinePosition
			{
				get { return 0; }
			}
		}
	}
}