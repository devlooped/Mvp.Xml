using System;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mvp.Xml.Exslt;


/// <summary>
/// Custom <see cref="XsltContext"/> implementation providing support for EXSLT 
/// functions in XPath-only environment.
/// </summary>
public class ExsltContext : XsltContext
{
    readonly XmlNameTable nt;

    /// <summary>
    /// Bitwise enumeration used to specify which EXSLT functions should be accessible to 
    /// in the ExsltContext object. The default value is ExsltFunctionNamespace.All 
    /// </summary>
    ExsltFunctionNamespace supportedFunctions = ExsltFunctionNamespace.All;

    /// <summary>
    /// Extension object which implements the functions in the http://exslt.org/math namespace
    /// </summary>
    readonly ExsltMath exsltMath = new();

    /// <summary>
    /// Extension object which implements the functions in the http://exslt.org/dates-and-times namespace
    /// </summary>
    readonly ExsltDatesAndTimes exsltDatesAndTimes = new();

    /// <summary>
    /// Extension object which implements the functions in the http://exslt.org/regular-expressions namespace
    /// </summary>
    readonly ExsltRegularExpressions exsltRegularExpressions = new();

    /// <summary>
    /// Extension object which implements the functions in the http://exslt.org/strings namespace
    /// </summary>
    readonly ExsltStrings exsltStrings = new();

    /// <summary>
    /// Extension object which implements the functions in the http://exslt.org/sets namespace
    /// </summary>
    readonly ExsltSets exsltSets = new();

    /// <summary>
    /// Extension object which implements the functions in the http://exslt.org/random namespace
    /// </summary>
    readonly ExsltRandom exsltRandom = new();

    /// <summary>
    /// Extension object which implements the functions in the http://gotdotnet.com/exslt/dates-and-times namespace
    /// </summary>
    readonly GdnDatesAndTimes gdnDatesAndTimes = new();

    /// <summary>
    /// Extension object which implements the functions in the http://gotdotnet.com/exslt/regular-expressions namespace
    /// </summary>
    readonly GdnRegularExpressions gdnRegularExpressions = new();

    /// <summary>
    /// Extension object which implements the functions in the http://gotdotnet.com/exslt/math namespace
    /// </summary>
    readonly GdnMath gdnMath = new();

    /// <summary>
    /// Extension object which implements the functions in the http://gotdotnet.com/exslt/sets namespace
    /// </summary>
    readonly GdnSets gdnSets = new();

    /// <summary>
    /// Extension object which implements the functions in the http://gotdotnet.com/exslt/strings namespace
    /// </summary>
    readonly GdnStrings gdnStrings = new();

    /// <summary>
    /// Extension object which implements the functions in the http://gotdotnet.com/exslt/dynamic namespace
    /// </summary>
    readonly GdnDynamic gdnDynamic = new();

    /// <summary>
    /// Creates new ExsltContext instance.
    /// </summary>        
    public ExsltContext(XmlNameTable nt) : base((NameTable)nt)
    {
        this.nt = nt;
        AddExtensionNamespaces();
    }

    /// <summary>
    /// Creates new ExsltContext instance.
    /// </summary>        
    public ExsltContext(NameTable nt, ExsltFunctionNamespace supportedFunctions) : this(nt)
        => SupportedFunctions = supportedFunctions;

    void AddExtensionNamespaces()
    {
        //remove all our extension objects in case the ExsltContext is being reused            
        RemoveNamespace("math", ExsltNamespaces.Math);
        RemoveNamespace("date", ExsltNamespaces.DatesAndTimes);
        RemoveNamespace("regexp", ExsltNamespaces.RegularExpressions);
        RemoveNamespace("str", ExsltNamespaces.Strings);
        RemoveNamespace("set", ExsltNamespaces.Sets);
        RemoveNamespace("random", ExsltNamespaces.Random);
        RemoveNamespace("date2", ExsltNamespaces.GdnDatesAndTimes);
        RemoveNamespace("math2", ExsltNamespaces.GdnMath);
        RemoveNamespace("regexp2", ExsltNamespaces.GdnRegularExpressions);
        RemoveNamespace("set2", ExsltNamespaces.GdnSets);
        RemoveNamespace("str2", ExsltNamespaces.GdnStrings);
        RemoveNamespace("dyn2", ExsltNamespaces.GdnDynamic);

        //add extension objects as specified by SupportedFunctions            
        if ((SupportedFunctions & ExsltFunctionNamespace.Math) > 0)
        {
            AddNamespace("math", ExsltNamespaces.Math);
        }

        if ((SupportedFunctions & ExsltFunctionNamespace.DatesAndTimes) > 0)
        {
            AddNamespace("date", ExsltNamespaces.DatesAndTimes);
        }

        if ((SupportedFunctions & ExsltFunctionNamespace.RegularExpressions) > 0)
        {
            AddNamespace("regexp", ExsltNamespaces.RegularExpressions);
        }

        if ((SupportedFunctions & ExsltFunctionNamespace.Strings) > 0)
        {
            AddNamespace("str", ExsltNamespaces.Strings);
        }

        if ((SupportedFunctions & ExsltFunctionNamespace.Sets) > 0)
        {
            AddNamespace("set", ExsltNamespaces.Sets);
        }

        if ((SupportedFunctions & ExsltFunctionNamespace.Random) > 0)
        {
            AddNamespace("random", ExsltNamespaces.Random);
        }

        if ((SupportedFunctions & ExsltFunctionNamespace.GdnDatesAndTimes) > 0)
        {
            AddNamespace("date2", ExsltNamespaces.GdnDatesAndTimes);
        }

        if ((SupportedFunctions & ExsltFunctionNamespace.GdnMath) > 0)
        {
            AddNamespace("math2", ExsltNamespaces.GdnMath);
        }

        if ((SupportedFunctions & ExsltFunctionNamespace.GdnRegularExpressions) > 0)
        {
            AddNamespace("regexp2", ExsltNamespaces.GdnRegularExpressions);
        }

        if ((SupportedFunctions & ExsltFunctionNamespace.GdnSets) > 0)
        {
            AddNamespace("set2", ExsltNamespaces.GdnSets);
        }

        if ((SupportedFunctions & ExsltFunctionNamespace.GdnStrings) > 0)
        {
            AddNamespace("str2", ExsltNamespaces.GdnStrings);
        }

        if ((SupportedFunctions & ExsltFunctionNamespace.GdnDynamic) > 0)
        {
            AddNamespace("dyn2", ExsltNamespaces.GdnDynamic);
        }
    }

    /// <summary>
    /// Bitwise enumeration used to specify which EXSLT functions should be accessible to 
    /// in the ExsltContext. The default value is ExsltFunctionNamespace.All 
    /// </summary>
    public ExsltFunctionNamespace SupportedFunctions
    {
        set
        {
            if (Enum.IsDefined(typeof(ExsltFunctionNamespace), value))
                supportedFunctions = value;
        }
        get { return supportedFunctions; }
    }

    /// <summary>
    /// See <see cref="XsltContext.CompareDocument"/>
    /// </summary>        
    public override int CompareDocument(string baseUri, string nextbaseUri) => 0;

    /// <summary>
    /// See <see cref="XsltContext.PreserveWhitespace"/>
    /// </summary>
    public override bool PreserveWhitespace(XPathNavigator node) => true;

    /// <summary>
    /// See <see cref="XsltContext.Whitespace"/>
    /// </summary>
    public override bool Whitespace => true;

    /// <summary>
    /// Resolves variables.
    /// </summary>
    /// <param name="prefix">The variable's prefix</param>
    /// <param name="name">The variable's name</param>
    /// <returns></returns>
    public override IXsltContextVariable ResolveVariable(string prefix, string name) => null;

    /// <summary>
    /// Resolves custom function in XPath expression.
    /// </summary>
    /// <param name="prefix">The prefix of the function as it appears in the XPath expression.</param>
    /// <param name="name">The name of the function.</param>
    /// <param name="argTypes">An array of argument types for the function being resolved. 
    /// This allows you to select between methods with the same name (for example, overloaded 
    /// methods). </param>
    /// <returns>An IXsltContextFunction representing the function.</returns>
    public override IXsltContextFunction ResolveFunction(string prefix, string name, XPathResultType[] argTypes) => LookupNamespace(nt.Get(prefix)) switch
    {
        ExsltNamespaces.DatesAndTimes => GetExtensionFunctionImplementation(exsltDatesAndTimes, name, argTypes),
        ExsltNamespaces.Math => GetExtensionFunctionImplementation(exsltMath, name, argTypes),
        ExsltNamespaces.RegularExpressions => GetExtensionFunctionImplementation(exsltRegularExpressions, name, argTypes),
        ExsltNamespaces.Sets => GetExtensionFunctionImplementation(exsltSets, name, argTypes),
        ExsltNamespaces.Strings => GetExtensionFunctionImplementation(exsltStrings, name, argTypes),
        ExsltNamespaces.Random => GetExtensionFunctionImplementation(exsltRandom, name, argTypes),
        ExsltNamespaces.GdnDatesAndTimes => GetExtensionFunctionImplementation(gdnDatesAndTimes, name, argTypes),
        ExsltNamespaces.GdnMath => GetExtensionFunctionImplementation(gdnMath, name, argTypes),
        ExsltNamespaces.GdnRegularExpressions => GetExtensionFunctionImplementation(gdnRegularExpressions, name, argTypes),
        ExsltNamespaces.GdnSets => GetExtensionFunctionImplementation(gdnSets, name, argTypes),
        ExsltNamespaces.GdnStrings => GetExtensionFunctionImplementation(gdnStrings, name, argTypes),
        ExsltNamespaces.GdnDynamic => GetExtensionFunctionImplementation(gdnDynamic, name, argTypes),
        _ => throw new XPathException(string.Format(
            "Unrecognized extension function namespace: prefix='{0}', namespace URI='{1}'",
            prefix, LookupNamespace(nt.Get(prefix))), null),
    };

    /// <summary>
    /// Finds appropriate implementation for an extension function - public 
    /// method with the same number of arguments and compatible argument types.
    /// </summary>
    /// <param name="obj">Extension object</param>
    /// <param name="name">Function name</param>
    /// <param name="argTypes">Types of arguments</param>
    /// <returns></returns>
    ExsltContextFunction GetExtensionFunctionImplementation(object obj, string name, XPathResultType[] argTypes)
    {
        //For each method in object's type
        foreach (var mi in obj.GetType().GetMethods())
        {
            //We are interested in methods with given name
            if (mi.Name == name)
            {
                var parameters = mi.GetParameters();
                ////We are interested in methods with given number of arguments
                if (parameters.Length == argTypes.Length)
                {
                    var mismatch = false;
                    //Now let's check out if parameter types are compatible with actual ones
                    for (var i = 0; i < parameters.Length; i++)
                    {
                        var pi = parameters[i];
                        var paramType = ConvertToXPathType(pi.ParameterType);
                        if (paramType == XPathResultType.Any || paramType == argTypes[i])
                        {
                            continue;
                        }
                        else
                        {
                            mismatch = true;
                            break;
                        }
                    }
                    if (!mismatch)
                    //Create lightweight wrapper around method info
                    {
                        return new ExsltContextFunction(mi, argTypes, obj);
                    }
                }
            }
        }
        throw new XPathException("Extension function not found: " + name, null);
    }

    /// <summary>
    /// Converts CLI type to XPathResultType type.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static XPathResultType ConvertToXPathType(Type type) => Type.GetTypeCode(type) switch
    {
        TypeCode.Boolean => XPathResultType.Boolean,
        TypeCode.String => XPathResultType.String,
        TypeCode.Object =>
            (typeof(IXPathNavigable).IsAssignableFrom(type) || typeof(XPathNavigator).IsAssignableFrom(type))
            ? XPathResultType.Navigator
            : (typeof(XPathNodeIterator).IsAssignableFrom(type))
            ? XPathResultType.NodeSet
            : XPathResultType.Any,
        TypeCode.DateTime => XPathResultType.Error,
        TypeCode.DBNull => XPathResultType.Error,
        TypeCode.Empty => XPathResultType.Error,
        _ => XPathResultType.Number
    };

    //TODO: test it

    ///// <summary>
    ///// This is a workaround for some problem, see
    ///// http://www.tkachenko.com/blog/archives/000042.html for more 
    ///// details.
    ///// </summary>
    ///// <param name="prefix">Prefix to be resolved</param>
    ///// <returns>Resolved namespace</returns>
    //public override string LookupNamespace(string prefix)
    //{
    //  if (prefix == String.Empty)
    //    return prefix;
    //  string uri = base.LookupNamespace(NameTable.Get(prefix));
    //  if (uri == null)
    //    throw new XsltException("Undeclared namespace prefix - " + prefix, null);

    //  return uri;
    //}
}
// namespace GotDotNet.Exslt