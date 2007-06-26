using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Xml;
using System.IO;
using System.Xml.XPath;
using System.Collections;
using Mvp.Xml.Synchronization;

namespace CustomerLibrary.Tests
{
	public abstract class TestFixtureBase
	{
		[Conditional("DEBUG")]
		protected void WriteIfDebugging(string message)
		{
			if (Debugger.IsAttached)
			{
				Debug.WriteLine(message);
			}
		}

		protected static XPathNavigator GetNavigator(string xml)
		{
			return new XPathDocument(GetReader(xml)).CreateNavigator();
		}

		protected static string NormalizeFormat(string xml)
		{
			return ReadToEnd(GetReader(xml));
		}

		protected static XmlReader GetReader(string xml)
		{
			XmlReaderSettings settings = new XmlReaderSettings();
			settings.IgnoreWhitespace = true;
			settings.CheckCharacters = true;
			settings.ConformanceLevel = ConformanceLevel.Auto;

			return XmlReader.Create(new StringReader(xml), settings);
		}

		protected static string ReadToEnd(XmlReader reader)
		{
			StringWriter sw = new StringWriter();
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.OmitXmlDeclaration = true;
			settings.Indent = true;
			settings.ConformanceLevel = ConformanceLevel.Fragment;
			XmlWriter writer = XmlWriter.Create(sw, settings);
			writer.WriteNode(reader, false);
			writer.Close();
			return sw.ToString();
		}

		protected static object Evaluate(XPathNavigator navigator, string xpath)
		{
			XmlNamespaceManager ns = new XmlNamespaceManager(navigator.NameTable);
			ns.AddNamespace(Schema.DefaultPrefix, Schema.Namespace);

			return navigator.Evaluate(xpath, ns);
		}

		protected static string EvaluateString(XPathNavigator navigator, string xpath)
		{
			XmlNamespaceManager ns = new XmlNamespaceManager(navigator.NameTable);
			ns.AddNamespace(Schema.DefaultPrefix, Schema.Namespace);

			return (string)navigator.Evaluate("string(" + xpath + ")", ns);
		}

		protected static T GetFirst<T>(IEnumerable<T> items)
		{
			foreach (T item in items)
			{
				return item;
			}

			return default(T);
		}

		protected static int Count(IEnumerable items)
		{
			int count = 0;
			foreach (object item in items)
			{
				count++;
			}

			return count;
		}
	}
}
