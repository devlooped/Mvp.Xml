using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;

namespace Mvp.Xml.XPath;

/// <summary>
/// Implements a cache of XPath queries, for faster execution.
/// </summary>
/// <remarks>
/// Discussed at http://weblogs.asp.net/cazzu/archive/2004/04/02/106667.aspx
/// <para>Author: Daniel Cazzulino, <a href="https://cazzulino.com">blog</a></para>
/// </remarks>
public static class XPathCache
{
    /// <summary>
    /// Initially a simple hashtable. In the future should 
    /// implement sliding expiration of unused expressions.
    /// </summary>
    static IDictionary<string, XPathExpression> Cache { get; } = new Dictionary<string, XPathExpression>();

    /// <summary>
    /// Retrieves a cached compiled expression, or a newly compiled one.
    /// </summary>
    static XPathExpression GetCompiledExpression(string expression, XPathNavigator source)
    {
        if (!Cache.TryGetValue(expression, out var expr))
        {
            // No double checks. At most we will compile twice. No big deal.			  
            expr = source.Compile(expression);
            Cache[expression] = expr;
        }

        return expr.Clone();
    }

    /// <summary>
    /// Sets up the context for expression execution.
    /// </summary>
    static XmlNamespaceManager PrepareContext(XPathNavigator source,
        XmlNamespaceManager context, XmlPrefix[] prefixes, XPathVariable[] variables)
    {
        var ctx = context;

        // If we have variables, we need the dynamic context. 
        if (variables != null)
        {
            var dyn = ctx != null ? new DynamicContext(ctx) : new DynamicContext();

            // Add the variables we received.
            foreach (var var in variables)
            {
                dyn.AddVariable(var.Name, var.Value);
            }

            ctx = dyn;
        }

        // If prefixes were added, append them to context.
        if (prefixes != null)
        {
            ctx ??= new XmlNamespaceManager(source.NameTable);

            foreach (var prefix in prefixes)
            {
                ctx.AddNamespace(prefix.Prefix, prefix.NamespaceUri);
            }
        }

        return ctx;
    }

    static void PrepareSort(XPathExpression expression, XPathNavigator source,
        object sortExpression, XmlSortOrder order, XmlCaseOrder caseOrder, string lang, XmlDataType dataType)
    {
        if (sortExpression is string s)
        {
            expression.AddSort(
                GetCompiledExpression(s, source),
                order, caseOrder, lang, dataType);
        }
        else if (sortExpression is XPathExpression)
        {
            expression.AddSort(sortExpression, order, caseOrder, lang, dataType);
        }
        else
        {
            throw new XPathException(Properties.Resources.XPathCache_BadSortObject, null);
        }
    }

    static void PrepareSort(XPathExpression expression, XPathNavigator source, object sortExpression,
        XmlSortOrder order, XmlCaseOrder caseOrder, string lang, XmlDataType dataType,
        XmlNamespaceManager context)
    {
        XPathExpression se;

        if (sortExpression is string s)
        {
            se = GetCompiledExpression(s, source);
        }
        else if (sortExpression is XPathExpression expr)
        {
            se = expr;
        }
        else
        {
            throw new XPathException(Properties.Resources.XPathCache_BadSortObject, null);
        }

        se.SetContext(context);
        expression.AddSort(se, order, caseOrder, lang, dataType);
    }

    static void PrepareSort(XPathExpression expression, XPathNavigator source,
        object sortExpression, IComparer comparer)
    {
        if (sortExpression is string s)
        {
            expression.AddSort(
                GetCompiledExpression(s, source), comparer);
        }
        else if (sortExpression is XPathExpression)
        {
            expression.AddSort(sortExpression, comparer);
        }
        else
        {
            throw new XPathException(Properties.Resources.XPathCache_BadSortObject, null);
        }
    }

    static void PrepareSort(XPathExpression expression, XPathNavigator source,
        object sortExpression, IComparer comparer, XmlNamespaceManager context)
    {
        XPathExpression se;

        if (sortExpression is string s)
        {
            se = GetCompiledExpression(s, source);
        }
        else if (sortExpression is XPathExpression expr)
        {
            se = expr;
        }
        else
        {
            throw new XPathException(Properties.Resources.XPathCache_BadSortObject, null);
        }

        se.SetContext(context);
        expression.AddSort(se, comparer);
    }

    /// <summary>
    /// Evaluates the given expression and returns the typed result.
    /// </summary>
    public static object Evaluate(string expression, XPathNavigator source) => source.Evaluate(GetCompiledExpression(expression, source));

    /// <summary>
    /// Evaluates the given expression and returns the typed result.
    /// </summary>
    public static object Evaluate(string expression, XPathNavigator source, params XPathVariable[] variables)
    {
        var expr = GetCompiledExpression(expression, source);
        expr.SetContext(PrepareContext(source, null, null, variables));
        return source.Evaluate(expr);
    }

    /// <summary>
    /// Evaluates the given expression and returns the typed result.
    /// </summary>
    public static object Evaluate(string expression, XPathNavigator source, XmlNamespaceManager context)
    {
        var expr = GetCompiledExpression(expression, source);
        expr.SetContext(context);
        return source.Evaluate(expr);
    }

    /// <summary>
    /// Evaluates the given expression and returns the typed result.
    /// </summary>
    public static object Evaluate(string expression, XPathNavigator source, params XmlPrefix[] prefixes)
    {
        var expr = GetCompiledExpression(expression, source);
        expr.SetContext(PrepareContext(source, null, prefixes, null));
        return source.Evaluate(expr);
    }

    /// <summary>
    /// Evaluates the given expression and returns the typed result.
    /// </summary>
    public static object Evaluate(string expression, XPathNavigator source, XmlNamespaceManager context, params XPathVariable[] variables)
    {
        var expr = GetCompiledExpression(expression, source);
        expr.SetContext(PrepareContext(source, context, null, variables));
        return source.Evaluate(expr);
    }

    /// <summary>
    /// Evaluates the given expression and returns the typed result.
    /// </summary>
    public static object Evaluate(string expression, XPathNavigator source, XmlPrefix[] prefixes, params XPathVariable[] variables)
    {
        var expr = GetCompiledExpression(expression, source);
        expr.SetContext(PrepareContext(source, null, prefixes, variables));
        return source.Evaluate(expr);
    }

    /// <summary>
    /// Selects a node set using the specified XPath expression.
    /// </summary>
    public static XPathNodeIterator Select(string expression, XPathNavigator source) => source.Select(GetCompiledExpression(expression, source));

    /// <summary>
    /// Selects a node set using the specified XPath expression.
    /// </summary>
    public static XPathNodeIterator Select(string expression, XPathNavigator source, params XPathVariable[] variables)
    {
        var expr = GetCompiledExpression(expression, source);
        expr.SetContext(PrepareContext(source, null, null, variables));
        return source.Select(expr);
    }

    /// <summary>
    /// Selects a node set using the specified XPath expression.
    /// </summary>
    public static XPathNodeIterator Select(string expression, XPathNavigator source, XmlNamespaceManager context)
    {
        var expr = GetCompiledExpression(expression, source);
        expr.SetContext(context);
        return source.Select(expr);
    }

    /// <summary>
    /// Selects a node set using the specified XPath expression.
    /// </summary>
    public static XPathNodeIterator Select(string expression, XPathNavigator source, params XmlPrefix[] prefixes)
    {
        var expr = GetCompiledExpression(expression, source);
        expr.SetContext(PrepareContext(source, null, prefixes, null));
        return source.Select(expr);
    }

    /// <summary>
    /// Selects a node set using the specified XPath expression.
    /// </summary>
    public static XPathNodeIterator Select(string expression, XPathNavigator source,
        XmlNamespaceManager context, params XPathVariable[] variables)
    {
        var expr = GetCompiledExpression(expression, source);
        expr.SetContext(PrepareContext(source, context, null, variables));
        return source.Select(expr);
    }

    /// <summary>
    /// Selects a node set using the specified XPath expression.
    /// </summary>
    public static XPathNodeIterator Select(string expression, XPathNavigator source,
        XmlPrefix[] prefixes, params XPathVariable[] variables)
    {
        var expr = GetCompiledExpression(expression, source);
        expr.SetContext(PrepareContext(source, null, prefixes, variables));
        return source.Select(expr);
    }

    /// <summary>
    /// Selects a node set using the specified XPath expression and sort.
    /// </summary>
    /// <remarks>
    /// See <see cref="XPathExpression.AddSort(object, IComparer)"/>.
    /// </remarks>
    public static XPathNodeIterator SelectSorted(string expression, XPathNavigator source,
        object sortExpression, IComparer comparer)
    {
        var expr = GetCompiledExpression(expression, source);
        PrepareSort(expr, source, sortExpression, comparer);
        return source.Select(expr);
    }

    /// <summary>
    /// Selects a node set using the specified XPath expression and sort.
    /// </summary>
    /// <remarks>
    /// See <see cref="XPathExpression.AddSort(object, IComparer)"/>.
    /// </remarks>
    public static XPathNodeIterator SelectSorted(string expression, XPathNavigator source,
        object sortExpression, XmlSortOrder order, XmlCaseOrder caseOrder, string lang, XmlDataType dataType)
    {
        var expr = GetCompiledExpression(expression, source);
        PrepareSort(expr, source, sortExpression, order, caseOrder, lang, dataType);
        return source.Select(expr);
    }

    /// <summary>
    /// Selects a node set using the specified XPath expression.
    /// </summary>
    public static XPathNodeIterator SelectSorted(string expression, XPathNavigator source,
        object sortExpression, IComparer comparer, params XPathVariable[] variables)
    {
        var expr = GetCompiledExpression(expression, source);
        expr.SetContext(PrepareContext(source, null, null, variables));
        PrepareSort(expr, source, sortExpression, comparer);
        return source.Select(expr);
    }

    /// <summary>
    /// Selects a node set using the specified XPath expression.
    /// </summary>
    public static XPathNodeIterator SelectSorted(string expression, XPathNavigator source,
        object sortExpression, XmlSortOrder order, XmlCaseOrder caseOrder, string lang, XmlDataType dataType,
        params XPathVariable[] variables)
    {
        var expr = GetCompiledExpression(expression, source);
        expr.SetContext(PrepareContext(source, null, null, variables));
        PrepareSort(expr, source, sortExpression, order, caseOrder, lang, dataType);
        return source.Select(expr);
    }

    /// <summary>
    /// Selects a node set using the specified XPath expression.
    /// </summary>
    public static XPathNodeIterator SelectSorted(string expression, XPathNavigator source,
        object sortExpression, XmlSortOrder order, XmlCaseOrder caseOrder, string lang, XmlDataType dataType,
        XmlNamespaceManager context)
    {
        var expr = GetCompiledExpression(expression, source);
        expr.SetContext(context);
        PrepareSort(expr, source, sortExpression, order, caseOrder, lang, dataType, context);
        return source.Select(expr);
    }

    /// <summary>
    /// Selects a node set using the specified XPath expression.
    /// </summary>
    public static XPathNodeIterator SelectSorted(string expression, XPathNavigator source,
        object sortExpression, IComparer comparer,
        params XmlPrefix[] prefixes)
    {
        var expr = GetCompiledExpression(expression, source);
        var ctx = PrepareContext(source, null, prefixes, null);
        expr.SetContext(ctx);
        PrepareSort(expr, source, sortExpression, comparer, ctx);
        return source.Select(expr);
    }

    /// <summary>
    /// Selects a node set using the specified XPath expression.
    /// </summary>
    public static XPathNodeIterator SelectSorted(string expression, XPathNavigator source,
        object sortExpression, XmlSortOrder order, XmlCaseOrder caseOrder, string lang, XmlDataType dataType,
        params XmlPrefix[] prefixes)
    {
        var expr = GetCompiledExpression(expression, source);
        var ctx = PrepareContext(source, null, prefixes, null);
        expr.SetContext(ctx);
        PrepareSort(expr, source, sortExpression, order, caseOrder, lang, dataType, ctx);
        return source.Select(expr);
    }

    /// <summary>
    /// Selects a node set using the specified XPath expression.
    /// </summary>
    public static XPathNodeIterator SelectSorted(string expression, XPathNavigator source,
        object sortExpression, XmlSortOrder order, XmlCaseOrder caseOrder, string lang, XmlDataType dataType,
        XmlNamespaceManager context, params XPathVariable[] variables)
    {
        var expr = GetCompiledExpression(expression, source);
        var ctx = PrepareContext(source, context, null, variables);
        expr.SetContext(ctx);
        PrepareSort(expr, source, sortExpression, order, caseOrder, lang, dataType, ctx);
        return source.Select(expr);
    }

    /// <summary>
    /// Selects a node set using the specified XPath expression.
    /// </summary>
    public static XPathNodeIterator SelectSorted(string expression, XPathNavigator source,
        object sortExpression, IComparer comparer,
        XmlNamespaceManager context, params XPathVariable[] variables)
    {
        var expr = GetCompiledExpression(expression, source);
        var ctx = PrepareContext(source, context, null, variables);
        expr.SetContext(ctx);
        PrepareSort(expr, source, sortExpression, comparer, ctx);
        return source.Select(expr);
    }

    /// <summary>
    /// Selects a node set using the specified XPath expression.
    /// </summary>
    public static XPathNodeIterator SelectSorted(string expression, XPathNavigator source,
        object sortExpression, XmlSortOrder order, XmlCaseOrder caseOrder, string lang, XmlDataType dataType,
        XmlPrefix[] prefixes, params XPathVariable[] variables)
    {
        var expr = GetCompiledExpression(expression, source);
        var ctx = PrepareContext(source, null, prefixes, variables);
        expr.SetContext(ctx);
        PrepareSort(expr, source, sortExpression, order, caseOrder, lang, dataType, ctx);
        return source.Select(expr);
    }

    /// <summary>
    /// Selects a node set using the specified XPath expression.
    /// </summary>
    public static XPathNodeIterator SelectSorted(string expression, XPathNavigator source,
        object sortExpression, IComparer comparer,
        XmlPrefix[] prefixes, params XPathVariable[] variables)
    {
        var expr = GetCompiledExpression(expression, source);
        var ctx = PrepareContext(source, null, prefixes, variables);
        expr.SetContext(ctx);
        PrepareSort(expr, source, sortExpression, comparer, ctx);
        return source.Select(expr);
    }

    /// <summary>
    /// Selects a list of nodes matching the XPath expression.
    /// </summary>
    public static XmlNodeList SelectNodes(string expression, XmlNode source)
    {
        var it = Select(expression, source.CreateNavigator());
        return XmlNodeListFactory.CreateNodeList(it);
    }

    /// <summary>
    /// Selects a list of nodes matching the XPath expression.
    /// </summary>
    public static XmlNodeList SelectNodes(string expression, XmlNode source, params XPathVariable[] variables)
    {
        var it = Select(expression, source.CreateNavigator(), variables);
        return XmlNodeListFactory.CreateNodeList(it);
    }

    /// <summary>
    /// Selects a list of nodes matching the XPath expression.
    /// </summary>
    public static XmlNodeList SelectNodes(string expression, XmlNode source, XmlNamespaceManager context)
    {
        var it = Select(expression, source.CreateNavigator(), context);
        return XmlNodeListFactory.CreateNodeList(it);
    }

    /// <summary>
    /// Selects a list of nodes matching the XPath expression.
    /// </summary>
    public static XmlNodeList SelectNodes(string expression, XmlNode source, params XmlPrefix[] prefixes)
    {
        var it = Select(expression, source.CreateNavigator(), prefixes);
        return XmlNodeListFactory.CreateNodeList(it);
    }

    /// <summary>
    /// Selects a list of nodes matching the XPath expression.
    /// </summary>
    public static XmlNodeList SelectNodes(string expression, XmlNode source, XmlNamespaceManager context, params XPathVariable[] variables)
    {
        var it = Select(expression, source.CreateNavigator(), context, variables);
        return XmlNodeListFactory.CreateNodeList(it);
    }

    /// <summary>
    /// Selects a list of nodes matching the XPath expression.
    /// </summary>
    public static XmlNodeList SelectNodes(string expression, XmlNode source, XmlPrefix[] prefixes, params XPathVariable[] variables)
    {
        var it = Select(expression, source.CreateNavigator(), prefixes, variables);
        return XmlNodeListFactory.CreateNodeList(it);
    }

    /// <summary>
    /// Selects a node set using the specified XPath expression and sort.
    /// </summary>
    /// <remarks>
    /// See <see cref="XPathExpression.AddSort(object, IComparer)"/>.
    /// </remarks>
    public static XmlNodeList SelectNodesSorted(string expression, XmlNode source, object sortExpression, IComparer comparer)
        => XmlNodeListFactory.CreateNodeList(SelectSorted(expression, source.CreateNavigator(), sortExpression, comparer));

    /// <summary>
    /// Selects a node set using the specified XPath expression and sort.
    /// </summary>
    /// <remarks>(object, IComparer)
    /// See <see cref="XPathExpression.AddSort(object, IComparer)"/>.
    /// </remarks>
    public static XmlNodeList SelectNodesSorted(string expression, XmlNode source, object sortExpression, XmlSortOrder order, XmlCaseOrder caseOrder, string lang, XmlDataType dataType)
        => XmlNodeListFactory.CreateNodeList(SelectSorted(expression, source.CreateNavigator(), sortExpression, order, caseOrder, lang, dataType));

    /// <summary>
    /// Selects a node set using the specified XPath expression.
    /// </summary>
    public static XmlNodeList SelectNodesSorted(string expression, XmlNode source, object sortExpression, IComparer comparer, params XPathVariable[] variables)
        => XmlNodeListFactory.CreateNodeList(SelectSorted(expression, source.CreateNavigator(), sortExpression, comparer, variables));

    /// <summary>
    /// Selects a node set using the specified XPath expression.
    /// </summary>
    public static XmlNodeList SelectNodesSorted(string expression, XmlNode source, object sortExpression, XmlSortOrder order, XmlCaseOrder caseOrder, string lang, XmlDataType dataType, params XPathVariable[] variables)
        => XmlNodeListFactory.CreateNodeList(SelectSorted(expression, source.CreateNavigator(), sortExpression, order, caseOrder, lang, dataType, variables));

    /// <summary>
    /// Selects a node set using the specified XPath expression.
    /// </summary>
    public static XmlNodeList SelectNodesSorted(string expression, XmlNode source, object sortExpression, XmlSortOrder order, XmlCaseOrder caseOrder, string lang, XmlDataType dataType, XmlNamespaceManager context)
        => XmlNodeListFactory.CreateNodeList(SelectSorted(expression, source.CreateNavigator(), sortExpression, order, caseOrder, lang, dataType, context));

    /// <summary>
    /// Selects a node set using the specified XPath expression.
    /// </summary>
    public static XmlNodeList SelectNodesSorted(string expression, XmlNode source, object sortExpression, IComparer comparer, params XmlPrefix[] prefixes)
        => XmlNodeListFactory.CreateNodeList(SelectSorted(expression, source.CreateNavigator(), sortExpression, comparer, prefixes));

    /// <summary>
    /// Selects a node set using the specified XPath expression.
    /// </summary>
    public static XmlNodeList SelectNodesSorted(string expression, XmlNode source, object sortExpression, XmlSortOrder order, XmlCaseOrder caseOrder, string lang, XmlDataType dataType, params XmlPrefix[] prefixes)
        => XmlNodeListFactory.CreateNodeList(SelectSorted(expression, source.CreateNavigator(), sortExpression, order, caseOrder, lang, dataType, prefixes));

    /// <summary>
    /// Selects a node set using the specified XPath expression.
    /// </summary>
    public static XmlNodeList SelectNodesSorted(string expression, XmlNode source, object sortExpression, XmlSortOrder order, XmlCaseOrder caseOrder, string lang, XmlDataType dataType, XmlNamespaceManager context, params XPathVariable[] variables)
        => XmlNodeListFactory.CreateNodeList(SelectSorted(expression, source.CreateNavigator(), sortExpression, order, caseOrder, lang, dataType, context, variables));

    /// <summary>
    /// Selects a node set using the specified XPath expression.
    /// </summary>
    public static XmlNodeList SelectNodesSorted(string expression, XmlNode source, object sortExpression, IComparer comparer, XmlNamespaceManager context, params XPathVariable[] variables)
        => XmlNodeListFactory.CreateNodeList(SelectSorted(expression, source.CreateNavigator(), sortExpression, comparer, context, variables));

    /// <summary>
    /// Selects a node set using the specified XPath expression.
    /// </summary>
    public static XmlNodeList SelectNodesSorted(string expression, XmlNode source, object sortExpression, XmlSortOrder order, XmlCaseOrder caseOrder, string lang, XmlDataType dataType, XmlPrefix[] prefixes, params XPathVariable[] variables)
        => XmlNodeListFactory.CreateNodeList(SelectSorted(expression, source.CreateNavigator(), sortExpression, order, caseOrder, lang, dataType, prefixes, variables));

    /// <summary>
    /// Selects a node set using the specified XPath expression.
    /// </summary>
    public static XmlNodeList SelectNodesSorted(string expression, XmlNode source, object sortExpression, IComparer comparer, XmlPrefix[] prefixes, params XPathVariable[] variables)
        => XmlNodeListFactory.CreateNodeList(SelectSorted(expression, source.CreateNavigator(), sortExpression, comparer, prefixes, variables));

    /// <summary>
    /// Selects the first XmlNode that matches the XPath expression.
    /// </summary>
    public static XmlNode SelectSingleNode(string expression, XmlNode source)
    {
        foreach (XmlNode node in SelectNodes(expression, source))
            return node;

        return null;
    }

    /// <summary>
    /// Selects the first XmlNode that matches the XPath expression.
    /// </summary>
    public static XmlNode SelectSingleNode(string expression, XmlNode source, params XPathVariable[] variables)
    {
        foreach (XmlNode node in SelectNodes(expression, source, variables))
            return node;

        return null;
    }

    /// <summary>
    /// Selects the first XmlNode that matches the XPath expression.
    /// </summary>
    public static XmlNode SelectSingleNode(string expression, XmlNode source, XmlNamespaceManager context)
    {
        foreach (XmlNode node in SelectNodes(expression, source, context))
            return node;

        return null;
    }

    /// <summary>
    /// Selects the first XmlNode that matches the XPath expression.
    /// </summary>
    public static XmlNode SelectSingleNode(string expression, XmlNode source, params XmlPrefix[] prefixes)
    {
        foreach (XmlNode node in SelectNodes(expression, source, prefixes))
            return node;

        return null;
    }

    /// <summary>
    /// Selects the first XmlNode that matches the XPath expression.
    /// </summary>
    public static XmlNode SelectSingleNode(string expression, XmlNode source, XmlNamespaceManager context, params XPathVariable[] variables)
    {
        foreach (XmlNode node in SelectNodes(expression, source, context, variables))
            return node;

        return null;
    }

    /// <summary>
    /// Selects the first XmlNode that matches the XPath expression.
    /// </summary>
    public static XmlNode SelectSingleNode(string expression, XmlNode source, XmlPrefix[] prefixes, params XPathVariable[] variables)
    {
        foreach (XmlNode node in SelectNodes(expression, source, prefixes, variables))
            return node;

        return null;
    }
}
