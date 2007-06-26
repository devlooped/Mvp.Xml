#if PocketPC
using Microsoft.Practices.Mobile.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Xml;
using System.IO;
using System.Xml.XPath;
using System.Collections;
using System.Reflection;

namespace Mvp.Xml.Synchronization.Tests
{
	public abstract class TestFixtureBase
	{
		/// <summary>
		/// Runs a test on all property setters and 
		/// getters.
		/// </summary>
		protected void TestProperties(object obj)
		{
			foreach (PropertyInfo prop in obj.GetType().GetProperties())
			{
				if (prop.CanRead && prop.CanWrite)
				{
					TypeCode code = Type.GetTypeCode(prop.PropertyType);
					switch (code)
					{
						case TypeCode.Boolean:
							prop.SetValue(obj, true, null);
							Assert.IsTrue((bool)prop.GetValue(obj, null));
							prop.SetValue(obj, false, null);
							Assert.IsFalse((bool)prop.GetValue(obj, null));
							break;
						case TypeCode.Byte:
							prop.SetValue(obj, 1, null);
							Assert.AreEqual((byte)1, prop.GetValue(obj, null));
							break;
						case TypeCode.Char:
							prop.SetValue(obj, 'c', null);
							Assert.AreEqual('c', (char)prop.GetValue(obj, null));
							break;
						case TypeCode.DBNull:
							prop.SetValue(obj, DBNull.Value, null);
							Assert.AreEqual(DBNull.Value, (DBNull)prop.GetValue(obj, null));
							break;
						case TypeCode.DateTime:
							DateTime now = DateTime.Now;
							prop.SetValue(obj, now, null);
							Assert.AreEqual(now, (DateTime)prop.GetValue(obj, null));
							break;
						case TypeCode.Decimal:
							prop.SetValue(obj, (decimal)25.25, null);
							Assert.AreEqual((decimal)25.25, (decimal)prop.GetValue(obj, null));
							break;
						case TypeCode.Double:
							prop.SetValue(obj, (double)25, null);
							Assert.AreEqual((double)25, (double)prop.GetValue(obj, null));
							break;
						case TypeCode.Empty:
							break;
						case TypeCode.Int16:
							prop.SetValue(obj, (Int16)25, null);
							Assert.AreEqual((Int16)25, (Int16)prop.GetValue(obj, null));
							break;
						case TypeCode.Int32:
							prop.SetValue(obj, (Int32)25, null);
							Assert.AreEqual((Int32)25, (Int32)prop.GetValue(obj, null));
							break;
						case TypeCode.Int64:
							prop.SetValue(obj, (Int64)25, null);
							Assert.AreEqual((Int64)25, (Int64)prop.GetValue(obj, null));
							break;
						case TypeCode.Object:
							try
							{
								object value = Activator.CreateInstance(prop.PropertyType);
								prop.SetValue(obj, value, null);
								Assert.AreEqual(value, prop.GetValue(obj, null));
							}
							catch
							{
							}
							break;
						case TypeCode.SByte:
							prop.SetValue(obj, (sbyte)8, null);
							Assert.AreEqual((sbyte)8, (sbyte)prop.GetValue(obj, null));
							break;
						case TypeCode.Single:
							prop.SetValue(obj, (Single)8, null);
							Assert.AreEqual((Single)8, (Single)prop.GetValue(obj, null));
							break;
						case TypeCode.String:
							prop.SetValue(obj, "foo", null);
							Assert.AreEqual("foo", prop.GetValue(obj, null));
							break;
						case TypeCode.UInt16:
							prop.SetValue(obj, (UInt16)8, null);
							Assert.AreEqual((UInt16)8, (UInt16)prop.GetValue(obj, null));
							break;
						case TypeCode.UInt32:
							prop.SetValue(obj, (UInt32)8, null);
							Assert.AreEqual((UInt32)8, (UInt32)prop.GetValue(obj, null));
							break;
						case TypeCode.UInt64:
							prop.SetValue(obj, (UInt64)8, null);
							Assert.AreEqual((UInt64)8, (UInt64)prop.GetValue(obj, null));
							break;
						default:
							break;
					}
				}
			}
		}

		[Conditional("DEBUG")]
		protected void WriteIfDebugging(string message)
		{
			if (Debugger.IsAttached)
			{
				Debug.WriteLine(message);
			}
		}

		protected static XmlElement GetElement(string xml)
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(GetReader(xml));

			return doc.DocumentElement;
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

		protected static int EvaluateCount(XmlNode navigator, string xpath)
		{
			XmlNamespaceManager ns = new XmlNamespaceManager(navigator.OwnerDocument.NameTable);
			ns.AddNamespace(Schema.DefaultPrefix, Schema.Namespace);

			return navigator.SelectNodes(xpath, ns).Count;
		}

		protected static string EvaluateString(XmlNode navigator, string xpath)
		{
			XmlNamespaceManager ns = new XmlNamespaceManager(navigator.OwnerDocument.NameTable);
			ns.AddNamespace(Schema.DefaultPrefix, Schema.Namespace);

			return navigator.SelectSingleNode(xpath, ns).Value;
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
