using System.Reflection;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mvp.Xml.Exslt;

/// <summary>
/// IXsltContextFunction wrapper around extension function.
/// </summary>
class ExsltContextFunction : IXsltContextFunction
{
    readonly MethodInfo method;
    readonly XPathResultType[] argTypes;
    readonly object ownerObj;

    public ExsltContextFunction(MethodInfo mi, XPathResultType[] argTypes, object owner)
    {
        method = mi;
        this.argTypes = argTypes;
        ownerObj = owner;
    }

    public int Minargs => argTypes.Length;

    public int Maxargs => argTypes.Length;

    public XPathResultType[] ArgTypes => argTypes;

    public XPathResultType ReturnType => ExsltContext.ConvertToXPathType(method.ReturnType);

    public object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext)
        => method.Invoke(ownerObj, args);
}

