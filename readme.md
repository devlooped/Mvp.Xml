![Icon](assets/img/logo.png)
============

[![Version](https://img.shields.io/nuget/vpre/Mvp.Xml.svg?color=royalblue)](https://www.nuget.org/packages/Mvp.Xml)
[![Downloads](https://img.shields.io/nuget/dt/Mvp.Xml.svg?color=green)](https://www.nuget.org/packages/Mvp.Xml)
[![License](https://img.shields.io/github/license/devlooped/Mvp.Xml.svg?color=blue)](https://github.com//devlooped/Mvp.Xml/blob/main/license.txt)
[![Build](https://github.com/devlooped/Mvp.Xml/workflows/build/badge.svg?branch=main)](https://github.com/devlooped/Mvp.Xml/actions)

<!-- #content -->
The original Mvp.Xml project, developed by Microsoft MVP's in XML technologies and XML Web Services worldwide. 
It is aimed at supplementing .NET XML processing. All the project's classes contain extensive tests to ensure 
its quality, as well as the peer review among this highly focused group of XML lovers.

Mvp.Xml project currently provides .NET implementations of [EXSLT](http://www.exslt.org/), [XML Base](http://www.w3.org/TR/xmlbase/), 
[XInclude](http://www.w3.org/TR/xinclude/), [XPointer](http://www.w3.org/TR/xptr-framework/) as well as a unique set of utility classes 
and tools making XML programming in .NET platform easier, more productive and effective.

## EXSLT

Example usage of [EXSLT](http://www.exslt.org/):

```csharp
var xslt = new MvpXslTransform();
xslt.Load("foo.xsl");
// Optionally enforce the output to be XHTML
xslt.EnforceXHTMLOutput = true;
xslt.Transform(new XmlInput("foo.xml"), new XmlOutput("result.html"));
```

Usage in XPath-only context:

```csharp
XPathExpression expr = nav.Compile("set:distinct(//author)");
expr.SetContext(new ExsltContext(doc.NameTable));
XPathNodeIterator authors = nav.Select(expr);
while (authors.MoveNext())
    Console.WriteLine(authors.Current.Value);
```

## XInclude

Example usage of [XInclude](http://www.w3.org/TR/xinclude/):

```csharp
var reader = new XIncludingReader(XmlReader.Create(uri));
var document = XDocument.Load(reader);
```

## Miscelaneous

Some other helper classes include:

```csharp
public XPathNodeIterator GetExpensiveBooks(IXPathNavigable doc, int minPrice)
{
 string expr = "//mvp:titles[mvp:price > $price]";
 
 // XPathCache optimally caches the compiled XPath expression and parameterizes it
 return XPathCache.Select(expr, doc.CreateNavigator(), mgr, new XPathVariable("price", minPrice));
}
```

```csharp
var xslt = new XslTransform();
xslt.Load("print_root.xsl");
var doc = new XPathDocument("library.xml");
 
var books = doc.CreateNavigator().Select("/library/book");
while (books.MoveNext())
{
    // Transform subtree for current node
    xslt.Transform(new SubtreeeXPathNavigator(books.Current), null, Console.Out, null);
}
```

<!-- include https://github.com/devlooped/sponsors/raw/main/footer.md -->