#region using

using System;
using System.Collections;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

#endregion using

namespace Mvp.Xml.Common.XPath
{
	/// <summary>
	/// Provides the evaluation context for fast execution and custom 
	/// variables resolution.
	/// </summary>
	/// <remarks>
	/// This class is responsible for resolving variables during dynamic expression execution.
	/// <para>Discussed in http://weblogs.asp.net/cazzu/archive/2003/10/07/30888.aspx</para>
	/// <para>Author: Daniel Cazzulino, kzu.net@gmail.com</para>
	/// </remarks>
	public class DynamicContext : XsltContext
	{
		#region Private vars

		Hashtable _variables = new Hashtable();
	
		#endregion Private

		#region Constructors & Initialization

		/// <summary>
		/// Initializes a new instance of the <see cref="DynamicContext"/> class.
		/// </summary>
		public DynamicContext() : base(new NameTable())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DynamicContext"/> 
		/// class with the specified <see cref="NameTable"/>.
		/// </summary>
		/// <param name="table">The NameTable to use.</param>
		public DynamicContext(NameTable table) : base(table) 
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DynamicContext"/> class.
		/// </summary>
		/// <param name="context">A previously filled context with the namespaces to use.</param>
		public DynamicContext(XmlNamespaceManager context) : this(context, new NameTable())
		{			
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DynamicContext"/> class.
		/// </summary>
		/// <param name="context">A previously filled context with the namespaces to use.</param>
		/// <param name="table">The NameTable to use.</param>
		public DynamicContext(XmlNamespaceManager context, NameTable table) : base(table)
		{
			object xml = table.Add(XmlNamespaces.Xml);
			object xmlns = table.Add(XmlNamespaces.XmlNs);

			if (context != null)
			{
				foreach (string prefix in context)
				{
					string uri = context.LookupNamespace(prefix);
					// Use fast object reference comparison to omit forbidden namespace declarations.
					if (Object.Equals(uri, xml) || Object.Equals(uri, xmlns))
						continue;

					base.AddNamespace(prefix, uri);
				}
			}
		}

		#endregion Constructors & Initialization

		#region Common Overrides

		/// <summary>
		/// Implementation equal to <see cref="XsltCompileContext"/>.
		/// </summary>
		public override int CompareDocument(string baseUri, string nextbaseUri) 
		{
			return String.Compare(baseUri, nextbaseUri, false, System.Globalization.CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Same as <see cref="XmlNamespaceManager"/>.
		/// </summary>
		public override string LookupNamespace(string prefix)
		{
			string key = NameTable.Get(prefix);
			if (key == null) 
				return null;
			else
				return base.LookupNamespace(key);
		}

		/// <summary>
		/// Same as <see cref="XmlNamespaceManager"/>.
		/// </summary>
		public override string LookupPrefix(string uri)
		{
			string key = NameTable.Get(uri);
			if (key == null) 
				return null;
			else
				return base.LookupPrefix(key);
		}

		/// <summary>
		/// Same as <see cref="XsltCompileContext"/>.
		/// </summary>
		public override bool PreserveWhitespace(XPathNavigator node)
		{
			return true;
		}

		/// <summary>
		/// Same as <see cref="XsltCompileContext"/>.
		/// </summary>
		public override bool Whitespace 
		{ 
			get { return true; } 
		}

		#endregion Common Overrides

		#region Public Members

		/// <summary>
		/// Shortcut method that compiles an expression using an empty navigator.
		/// </summary>
		/// <param name="xpath">The expression to compile</param>
		/// <returns>A compiled <see cref="XPathExpression"/>.</returns>
		public static XPathExpression Compile(string xpath)
		{
			return new XmlDocument().CreateNavigator().Compile(xpath);
		}

		#endregion Public Members

		#region Variable Handling Code

		/// <summary>
		/// Adds the variable to the dynamic evaluation context.
		/// </summary>
		/// <param name="name">The name of the variable to add to the context.</param>
		/// <param name="value">The value of the variable to add to the context.</param>
		/// <remarks>
		/// Value type conversion for XPath evaluation is as follows:
		/// <list type="table">
		///		<listheader>
		///			<term>CLR Type</term>
		///			<description>XPath type</description>
		///		</listheader>
		///		<item>
		///			<term>System.String</term>
		///			<description>XPathResultType.String</description>
		///		</item>
		///		<item>
		///			<term>System.Double (or types that can be converted to)</term>
		///			<description>XPathResultType.Number</description>
		///		</item>
		///		<item>
		///			<term>System.Boolean</term>
		///			<description>XPathResultType.Boolean</description>
		///		</item>
		///		<item>
		///			<term>System.Xml.XPath.XPathNavigator</term>
		///			<description>XPathResultType.Navigator</description>
		///		</item>
		///		<item>
		///			<term>System.Xml.XPath.XPathNodeIterator</term>
		///			<description>XPathResultType.NodeSet</description>
		///		</item>
		///		<item>
		///			<term>Others</term>
		///			<description>XPathResultType.Any</description>
		///		</item>
		/// </list>
		/// <note type="note">See the topic "Compile, Select, Evaluate, and Matches with 
		/// XPath and XPathExpressions" in MSDN documentation for additional information.</note>
		/// </remarks>
		/// <exception cref="ArgumentNullException">The <paramref name="value"/> is null.</exception>
		public void AddVariable(string name, object value)
		{
			if (value == null) throw new ArgumentNullException("value");
			_variables[name] = new DynamicVariable(name, value);
		}

		/// <summary>
		/// See <see cref="XsltContext"/>. Not used in our implementation.
		/// </summary>
		public override IXsltContextFunction ResolveFunction(string prefix, string name, XPathResultType[] ArgTypes)
		{
			return null;
		}

		/// <summary>
		/// Resolves the dynamic variables added to the context. See <see cref="XsltContext"/>. 
		/// </summary>
		public override IXsltContextVariable ResolveVariable(string prefix, string name)
		{
			return _variables[name] as IXsltContextVariable;
		}

		#endregion Variable Handling Code

		#region Internal DynamicVariable class

		/// <summary>
		/// Represents a variable during dynamic expression execution.
		/// </summary>
		internal class DynamicVariable : IXsltContextVariable
		{
			string _name;
			object _value;

			#region Public Members

			/// <summary>
			/// Initializes a new instance of the class.
			/// </summary>
			/// <param name="name">The name of the variable.</param>
			/// <param name="value">The value of the variable.</param>
			public DynamicVariable(string name, object value)
			{

				_name = name;
				_value = value;

				if (value is String)
					_type = XPathResultType.String;
				else if (value is bool)
					_type = XPathResultType.Boolean;
				else if (value is XPathNavigator)
					_type = XPathResultType.Navigator;
				else if (value is XPathNodeIterator)
					_type = XPathResultType.NodeSet;
				else
				{
					// Try to convert to double (native XPath numeric type)
					if (value is double)
					{
						_type = XPathResultType.Number;
					}
					else
					{
						if (value is IConvertible)
						{
							try
							{
								_value = Convert.ToDouble(value);
								// We suceeded, so it's a number.
								_type = XPathResultType.Number;
							}
							catch (FormatException)
							{
								_type = XPathResultType.Any;
							}
							catch (OverflowException)
							{
								_type = XPathResultType.Any;
							}
						}
						else
						{
							_type = XPathResultType.Any;
						}
					}
				}
			}

			#endregion Public Members

			#region IXsltContextVariable Implementation

			XPathResultType IXsltContextVariable.VariableType
			{
				get { return _type; }
			} XPathResultType _type;

			object IXsltContextVariable.Evaluate(XsltContext context) 
			{
				return _value;
			}

			bool IXsltContextVariable.IsLocal
			{
				get { return false; }
			}

			bool IXsltContextVariable.IsParam
			{
				get  { return false; }
			}

			#endregion IXsltContextVariable Implementation
		}

		#endregion Internal DynamicVariable class
	}
}
