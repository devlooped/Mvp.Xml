using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mvp.Xml.XPath;

/// <summary>
/// Provides the evaluation context for fast execution and custom 
/// variables resolution.
/// </summary>
/// <remarks>
/// This class is responsible for resolving variables during dynamic expression execution.
/// <para>Discussed in http://weblogs.asp.net/cazzu/archive/2003/10/07/30888.aspx</para>
/// <para>Author: Daniel Cazzulino, <a href="https://cazzulino.com">blog</a></para>
/// </remarks>
public class DynamicContext : XsltContext
{
    readonly Dictionary<string, IXsltContextVariable> variables = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="DynamicContext"/> class.
    /// </summary>
    public DynamicContext() : base(new NameTable()) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DynamicContext"/> 
    /// class with the specified <see cref="NameTable"/>.
    /// </summary>
    /// <param name="table">The NameTable to use.</param>
    public DynamicContext(NameTable table) : base(table) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DynamicContext"/> class.
    /// </summary>
    /// <param name="context">A previously filled context with the namespaces to use.</param>
    public DynamicContext(XmlNamespaceManager context) : this(context, new NameTable()) { }

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
                var uri = context.LookupNamespace(prefix);
                // Use fast object reference comparison to omit forbidden namespace declarations.
                if (Equals(uri, xml) || Equals(uri, xmlns))
                    continue;

                base.AddNamespace(prefix, uri);
            }
        }
    }

    /// <summary>
    /// Implementation equal to <see cref="XsltContext"/>.
    /// </summary>
    public override int CompareDocument(string baseUri, string nextbaseUri)
        => string.Compare(baseUri, nextbaseUri, false, System.Globalization.CultureInfo.InvariantCulture);

    /// <summary>
    /// Same as <see cref="XmlNamespaceManager"/>.
    /// </summary>
    public override string LookupNamespace(string prefix)
    {
        var key = NameTable.Get(prefix);
        if (key == null)
            return null;

        return base.LookupNamespace(key);
    }

    /// <summary>
    /// Same as <see cref="XmlNamespaceManager"/>.
    /// </summary>
    public override string LookupPrefix(string uri)
    {
        var key = NameTable.Get(uri);
        if (key == null)
            return null;

        return base.LookupPrefix(key);
    }

    /// <summary>
    /// Same as <see cref="XsltContext"/>.
    /// </summary>
    public override bool PreserveWhitespace(XPathNavigator node) => true;

    /// <summary>
    /// Same as <see cref="XsltContext"/>.
    /// </summary>
    public override bool Whitespace => true;

    /// <summary>
    /// Shortcut method that compiles an expression using an empty navigator.
    /// </summary>
    /// <param name="xpath">The expression to compile</param>
    /// <returns>A compiled <see cref="XPathExpression"/>.</returns>
    public static XPathExpression Compile(string xpath) => new XmlDocument().CreateNavigator().Compile(xpath);

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
        if (value == null)
            throw new ArgumentNullException("value");

        variables[name] = new DynamicVariable(name, value);
    }

    /// <summary>
    /// See <see cref="XsltContext"/>. Not used in our implementation.
    /// </summary>
    public override IXsltContextFunction ResolveFunction(string prefix, string name, XPathResultType[] argTypes) => null;

    /// <summary>
    /// Resolves the dynamic variables added to the context. See <see cref="XsltContext"/>. 
    /// </summary>
    public override IXsltContextVariable ResolveVariable(string prefix, string name)
    {
        variables.TryGetValue(name, out var var);
        return var;
    }

    /// <summary>
    /// Represents a variable during dynamic expression execution.
    /// </summary>
    class DynamicVariable : IXsltContextVariable
    {
        readonly string name;
        readonly object value;
        readonly XPathResultType type;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <param name="value">The value of the variable.</param>
        public DynamicVariable(string name, object value)
        {

            this.name = name;
            this.value = value;

            if (value is string)
            {
                type = XPathResultType.String;
            }
            else if (value is bool)
            {
                type = XPathResultType.Boolean;
            }
            else if (value is XPathNavigator)
            {
                type = XPathResultType.Navigator;
            }
            else if (value is XPathNodeIterator)
            {
                type = XPathResultType.NodeSet;
            }
            else
            {
                // Try to convert to double (native XPath numeric type)
                if (value is double)
                {
                    type = XPathResultType.Number;
                }
                else
                {
                    if (value is IConvertible)
                    {
                        try
                        {
                            this.value = Convert.ToDouble(value);
                            // We suceeded, so it's a number.
                            type = XPathResultType.Number;
                        }
                        catch (FormatException)
                        {
                            type = XPathResultType.Any;
                        }
                        catch (OverflowException)
                        {
                            type = XPathResultType.Any;
                        }
                    }
                    else
                    {
                        type = XPathResultType.Any;
                    }
                }
            }
        }

        XPathResultType IXsltContextVariable.VariableType => type;

        object IXsltContextVariable.Evaluate(XsltContext context) => value;

        bool IXsltContextVariable.IsLocal => false;

        bool IXsltContextVariable.IsParam => false;
    }
}
