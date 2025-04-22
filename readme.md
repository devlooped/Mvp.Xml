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
# Sponsors 

<!-- sponsors.md -->
[![Clarius Org](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/clarius.png "Clarius Org")](https://github.com/clarius)
[![MFB Technologies, Inc.](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/MFB-Technologies-Inc.png "MFB Technologies, Inc.")](https://github.com/MFB-Technologies-Inc)
[![Torutek](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/torutek-gh.png "Torutek")](https://github.com/torutek-gh)
[![DRIVE.NET, Inc.](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/drivenet.png "DRIVE.NET, Inc.")](https://github.com/drivenet)
[![Keith Pickford](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/Keflon.png "Keith Pickford")](https://github.com/Keflon)
[![Thomas Bolon](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/tbolon.png "Thomas Bolon")](https://github.com/tbolon)
[![Kori Francis](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/kfrancis.png "Kori Francis")](https://github.com/kfrancis)
[![Toni Wenzel](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/twenzel.png "Toni Wenzel")](https://github.com/twenzel)
[![Uno Platform](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/unoplatform.png "Uno Platform")](https://github.com/unoplatform)
[![Dan Siegel](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/dansiegel.png "Dan Siegel")](https://github.com/dansiegel)
[![Reuben Swartz](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/rbnswartz.png "Reuben Swartz")](https://github.com/rbnswartz)
[![Jacob Foshee](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/jfoshee.png "Jacob Foshee")](https://github.com/jfoshee)
[![](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/Mrxx99.png "")](https://github.com/Mrxx99)
[![Eric Johnson](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/eajhnsn1.png "Eric Johnson")](https://github.com/eajhnsn1)
[![Ix Technologies B.V.](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/IxTechnologies.png "Ix Technologies B.V.")](https://github.com/IxTechnologies)
[![David JENNI](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/davidjenni.png "David JENNI")](https://github.com/davidjenni)
[![Jonathan ](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/Jonathan-Hickey.png "Jonathan ")](https://github.com/Jonathan-Hickey)
[![Charley Wu](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/akunzai.png "Charley Wu")](https://github.com/akunzai)
[![Ken Bonny](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/KenBonny.png "Ken Bonny")](https://github.com/KenBonny)
[![Simon Cropp](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/SimonCropp.png "Simon Cropp")](https://github.com/SimonCropp)
[![agileworks-eu](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/agileworks-eu.png "agileworks-eu")](https://github.com/agileworks-eu)
[![sorahex](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/sorahex.png "sorahex")](https://github.com/sorahex)
[![Zheyu Shen](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/arsdragonfly.png "Zheyu Shen")](https://github.com/arsdragonfly)
[![Vezel](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/vezel-dev.png "Vezel")](https://github.com/vezel-dev)
[![ChilliCream](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/ChilliCream.png "ChilliCream")](https://github.com/ChilliCream)
[![4OTC](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/4OTC.png "4OTC")](https://github.com/4OTC)
[![Vincent Limo](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/v-limo.png "Vincent Limo")](https://github.com/v-limo)
[![Jordan S. Jones](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/jordansjones.png "Jordan S. Jones")](https://github.com/jordansjones)
[![domischell](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/DominicSchell.png "domischell")](https://github.com/DominicSchell)


<!-- sponsors.md -->

[![Sponsor this project](https://raw.githubusercontent.com/devlooped/sponsors/main/sponsor.png "Sponsor this project")](https://github.com/sponsors/devlooped)
&nbsp;

[Learn more about GitHub Sponsors](https://github.com/sponsors)

<!-- https://github.com/devlooped/sponsors/raw/main/footer.md -->
