using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mvp.Xml.Exslt;

/// <summary>
/// Enumeration used to indicate an EXSLT function namespace. 
/// </summary>
[Flags]
public enum ExsltFunctionNamespace
{
    /// <summary>Nothing</summary>
    None = 0,
    /// <summary>Dates and Times module</summary>
    DatesAndTimes = 1,
    /// <summary>Math module</summary>
    Math = 2,
    /// <summary>RegExp module</summary>
    RegularExpressions = 4,
    /// <summary>Sets module</summary>
    Sets = 8,
    /// <summary>Strings module</summary>
    Strings = 16,
    /// <summary>GotDotNet Dates and Times module</summary>
    GdnDatesAndTimes = 32,
    /// <summary>GotDotNet Sets module</summary>
    GdnSets = 64,
    /// <summary>GotDotNet Math module</summary>
    GdnMath = 128,
    /// <summary>GotDotNet RegExp module</summary>
    GdnRegularExpressions = 256,
    /// <summary>GotDotNet Strings module</summary>
    GdnStrings = 512,
    /// <summary>Random module</summary>
    Random = 1024,
    /// <summary>GotDotNet Dynamic module</summary>
    GdnDynamic = 2056,
    /// <summary>All EXSLT modules</summary>
    AllExslt = DatesAndTimes | Math | Random | RegularExpressions | Sets | Strings,
    /// <summary>All modules</summary>
    All = DatesAndTimes | Math | Random | RegularExpressions | Sets | Strings |
        GdnDatesAndTimes | GdnSets | GdnMath | GdnRegularExpressions | GdnStrings | GdnDynamic
}

/// <summary>
/// Transforms XML data using an XSLT stylesheet. Supports a number of EXSLT as 
/// defined at http://www.exslt.org
/// </summary>
/// <remarks>
/// XslCompiledTransform supports the XSLT 1.0 syntax. The XSLT stylesheet must use the 
/// namespace http://www.w3.org/1999/XSL/Transform. Additional arguments can also be 
/// added to the stylesheet using the XsltArgumentList class. 
/// This class contains input parameters for the stylesheet and extension objects which can be called from the stylesheet.
/// This class also recognizes functions from the following namespaces:<br/>
/// * http://exslt.org/common<br/>
/// * http://exslt.org/dates-and-times<br/>
/// * http://exslt.org/math<br/>
/// * http://exslt.org/random<br/>
/// * http://exslt.org/regular-expressions<br/>
/// * http://exslt.org/sets<br/>
/// * http://exslt.org/strings<br/>
/// * http://gotdotnet.com/exslt/dates-and-times<br/>
/// * http://gotdotnet.com/exslt/math<br/>
/// * http://gotdotnet.com/exslt/regular-expressions<br/>
/// * http://gotdotnet.com/exslt/sets<br/>
/// * http://gotdotnet.com/exslt/strings<br/>
/// * http://gotdotnet.com/exslt/dynamic<br/>
/// </remarks>
[Obsolete("This class has been deprecated. Please use Mvp.Xml.Common.Xsl.MvpXslTransform instead.")]
public class ExsltTransform
{
    /// <summary>
    /// Sync object.
    /// </summary>
    readonly object sync = new();

    /// <summary>
    /// The XslTransform object wrapped by this class. 
    /// </summary>
    readonly XslCompiledTransform xslTransform;

    /// <summary>
    /// Bitwise enumeration used to specify which EXSLT functions should be accessible to 
    /// the ExsltTransform object. The default value is ExsltFunctionNamespace.All 
    /// </summary>
    ExsltFunctionNamespace supportedFunctions = ExsltFunctionNamespace.All;

    /// <summary>
    /// Extension object which implements the functions in the http://exslt.org/math namespace
    /// </summary>
    readonly ExsltMath exsltMath = new();

    /// <summary>
    /// Extension object which implements the functions in the http://exslt.org/random namespace
    /// </summary>
    readonly ExsltRandom exsltRandom = new();

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
    /// Boolean flag used to specify whether multiple output is supported.
    /// </summary>
    bool multiOutput = false;

    /// <summary>
    /// Bitwise enumeration used to specify which EXSLT functions should be accessible to 
    /// the ExsltTransform object. The default value is ExsltFunctionNamespace.All 
    /// </summary>
    public ExsltFunctionNamespace SupportedFunctions
    {
        set
        {
            if (Enum.IsDefined(typeof(ExsltFunctionNamespace), value))
                supportedFunctions = value;
        }
        get => supportedFunctions;
    }

    /// <summary>
    /// Boolean flag used to specify whether multiple output (via exsl:document) is 
    /// supported.
    /// Note: This property is ignored (hence multiple output is not supported) when
    /// transformation is done to XmlReader or XmlWriter (use overloaded method, 
    /// which transforms to MultiXmlTextWriter instead).
    /// Note: Because of some restrictions and slight overhead this feature is
    /// disabled by default. If you need multiple output support, set this property to
    /// true before the Transform() call.
    /// </summary>
    public bool MultiOutput
    {
        get => multiOutput;
        set => multiOutput = value;
    }

    /// <summary>
    /// Gets an <see cref="XmlWriterSettings"/> object that contains the output 
    /// information derived from the xsl:output element of the style sheet.
    /// </summary>
    public XmlWriterSettings OutputSettings => xslTransform.OutputSettings; 

    ///// <summary>
    ///// Gets the TempFileCollection that contains the temporary files generated 
    ///// on disk after a successful call to the Load method. 
    ///// </summary>
    //public TempFileCollection TemporaryFiles
    //{
    //	get { return xslTransform.TemporaryFiles; }
    //}

    /// <summary>
    /// Initializes a new instance of the ExsltTransform class. 
    /// </summary>
    public ExsltTransform() => xslTransform = new XslCompiledTransform();

    /// <summary>
    /// Initializes a new instance of the ExsltTransform 
    /// class with the specified debug setting.  
    /// </summary>
    public ExsltTransform(bool debug) => xslTransform = new XslCompiledTransform(debug);

    /// <summary>
    /// Loads and compiles the style sheet contained in the <see cref="IXPathNavigable"/> object.
    /// See also <see cref="XslCompiledTransform.Load(IXPathNavigable)"/>.
    /// </summary>
    /// <param name="stylesheet">An object implementing the <see cref="IXPathNavigable"/> interface. 
    /// In the Microsoft .NET Framework, this can be either an <see cref="XmlNode"/> (typically an <see cref="XmlDocument"/>), 
    /// or an <see cref="XPathDocument"/> containing the style sheet.</param>
    public void Load(IXPathNavigable stylesheet) => xslTransform.Load(stylesheet);

    /// <summary>
    /// Loads and compiles the style sheet located at the specified URI.
    /// See also <see cref="XslCompiledTransform.Load(String)"/>.
    /// </summary>
    /// <param name="stylesheetUri">The URI of the style sheet.</param>
    public void Load(string stylesheetUri) => xslTransform.Load(stylesheetUri);

    /// <summary>
    /// Compiles the style sheet contained in the <see cref="XmlReader"/>.
    /// See also <see cref="XslCompiledTransform.Load(XmlReader)"/>.
    /// </summary>
    /// <param name="stylesheet">An <see cref="XmlReader"/> containing the style sheet.</param>
    public void Load(XmlReader stylesheet) => xslTransform.Load(stylesheet);

    /// <summary>
    /// Compiles the XSLT style sheet contained in the <see cref="IXPathNavigable"/>. 
    /// The <see cref="XmlResolver"/> resolves any XSLT <c>import</c> or <c>include</c> elements and the 
    /// XSLT settings determine the permissions for the style sheet. 
    /// See also <see cref="XslCompiledTransform.Load(IXPathNavigable, XsltSettings, XmlResolver)"/>.
    /// </summary>                     
    /// <param name="stylesheet">An object implementing the <see cref="IXPathNavigable"/> interface. 
    /// In the Microsoft .NET Framework, this can be either an <see cref="XmlNode"/> (typically an <see cref="XmlDocument"/>), 
    /// or an <see cref="XPathDocument"/> containing the style sheet.</param>
    /// <param name="settings">The <see cref="XsltSettings"/> to apply to the style sheet. 
    /// If this is a null reference (Nothing in Visual Basic), the <see cref="XsltSettings.Default"/> setting is applied.</param>
    /// <param name="stylesheetResolver">The <see cref="XmlResolver"/> used to resolve any 
    /// style sheets referenced in XSLT <c>import</c> and <c>include</c> elements. If this is a 
    /// null reference (Nothing in Visual Basic), external resources are not resolved.</param>        
    public void Load(IXPathNavigable stylesheet, XsltSettings settings, XmlResolver stylesheetResolver) => xslTransform.Load(stylesheet, settings, stylesheetResolver);

    /// <summary>
    /// Loads and compiles the XSLT style sheet specified by the URI. 
    /// The <see cref="XmlResolver"/> resolves any XSLT <c>import</c> or <c>include</c> elements and the 
    /// XSLT settings determine the permissions for the style sheet. 
    /// See also <see cref="XslCompiledTransform.Load(string, XsltSettings, XmlResolver)"/>.
    /// </summary>           
    /// <param name="stylesheetUri">The URI of the style sheet.</param>
    /// <param name="settings">The <see cref="XsltSettings"/> to apply to the style sheet. 
    /// If this is a null reference (Nothing in Visual Basic), the <see cref="XsltSettings.Default"/> setting is applied.</param>
    /// <param name="stylesheetResolver">The <see cref="XmlResolver"/> used to resolve any 
    /// style sheets referenced in XSLT <c>import</c> and <c>include</c> elements. If this is a 
    /// null reference (Nothing in Visual Basic), external resources are not resolved.</param>        
    public void Load(string stylesheetUri, XsltSettings settings, XmlResolver stylesheetResolver) => xslTransform.Load(stylesheetUri, settings, stylesheetResolver);

    /// <summary>
    /// Compiles the XSLT style sheet contained in the <see cref="XmlReader"/>. 
    /// The <see cref="XmlResolver"/> resolves any XSLT <c>import</c> or <c>include</c> elements and the 
    /// XSLT settings determine the permissions for the style sheet. 
    /// See also <see cref="XslCompiledTransform.Load(XmlReader, XsltSettings, XmlResolver)"/>.
    /// </summary>                
    /// <param name="stylesheet">The <see cref="XmlReader"/> containing the style sheet.</param>
    /// <param name="settings">The <see cref="XsltSettings"/> to apply to the style sheet. 
    /// If this is a null reference (Nothing in Visual Basic), the <see cref="XsltSettings.Default"/> setting is applied.</param>
    /// <param name="stylesheetResolver">The <see cref="XmlResolver"/> used to resolve any 
    /// style sheets referenced in XSLT <c>import</c> and <c>include</c> elements. If this is a 
    /// null reference (Nothing in Visual Basic), external resources are not resolved.</param>
    public void Load(XmlReader stylesheet, XsltSettings settings, XmlResolver stylesheetResolver) => xslTransform.Load(stylesheet, settings, stylesheetResolver);

    /// <summary>
    /// Executes the transform using the input document specified by the 
    /// IXPathNavigable object and outputs the results to an XmlWriter. 
    /// </summary>        
    public void Transform(IXPathNavigable input, XmlWriter results) => xslTransform.Transform(input, AddExsltExtensionObjects(null), results);

    /// <summary>
    /// Executes the transform using the input document specified by the URI 
    /// and outputs the results to a file. 
    /// </summary>        
    public void Transform(string inputUri, string resultsFile)
    {
        // Use using so that the file is not held open after the call
        using var outStream = File.OpenWrite(resultsFile);
        if (multiOutput)
        {
            xslTransform.Transform(new XPathDocument(inputUri),
                AddExsltExtensionObjects(null),
                new MultiXmlTextWriter(outStream, OutputSettings.Encoding));
        }
        else
        {
            xslTransform.Transform(new XPathDocument(inputUri),
                AddExsltExtensionObjects(null),
                outStream);
        }
    }

    /// <summary>
    /// Executes the transform using the input document specified by the URI 
    /// and outputs the results to an XmlWriter.
    /// </summary>        
    public void Transform(string inputUri, XmlWriter results)
        => xslTransform.Transform(inputUri, AddExsltExtensionObjects(null), results);

    /// <summary>
    /// Executes the transform using the input document specified by the 
    /// XmlReader object and outputs the results to an XmlWriter. 
    /// </summary>        
    public void Transform(XmlReader input, XmlWriter results) 
        => xslTransform.Transform(input, AddExsltExtensionObjects(null), results);

    /// <summary>
    /// Executes the transform using the input document specified by the 
    /// IXPathNavigable object and outputs the results to a stream. 
    /// The XsltArgumentList provides additional runtime arguments. 
    /// </summary>        
    public void Transform(IXPathNavigable input, XsltArgumentList arguments, Stream results)
    {
        if (multiOutput)
        {
            xslTransform.Transform(input,
                AddExsltExtensionObjects(arguments),
                new MultiXmlTextWriter(results, OutputSettings.Encoding));
        }
        else
        {
            xslTransform.Transform(input,
                AddExsltExtensionObjects(arguments), results);
        }
    }

    /// <summary>
    /// Executes the transform using the input document specified by the 
    /// IXPathNavigable object and outputs the results to an TextWriter. 
    /// The XsltArgumentList provides additional run-time arguments.
    /// </summary>        
    public void Transform(IXPathNavigable input, XsltArgumentList arguments, TextWriter results)
    {
        if (multiOutput)
        {
            xslTransform.Transform(input,
                AddExsltExtensionObjects(arguments),
                new MultiXmlTextWriter(results));
        }
        else
        {
            xslTransform.Transform(input,
                AddExsltExtensionObjects(arguments), results);
        }
    }


    /// <summary>
    /// Executes the transform using the input document specified by the 
    /// IXPathNavigable object and outputs the results to an XmlWriter. 
    /// The XsltArgumentList provides additional run-time arguments. 
    /// </summary>        
    public void Transform(IXPathNavigable input, XsltArgumentList arguments, XmlWriter results) 
        => xslTransform.Transform(input, AddExsltExtensionObjects(arguments), results);

    /// <summary>
    /// Executes the transform using the input document specified by the URI 
    /// and outputs the results to stream. 
    /// The XsltArgumentList provides additional run-time arguments. 
    /// </summary>        
    public void Transform(string inputUri, XsltArgumentList arguments, Stream results)
    {
        if (multiOutput)
        {
            xslTransform.Transform(inputUri,
               AddExsltExtensionObjects(arguments),
               new MultiXmlTextWriter(results, OutputSettings.Encoding));
        }
        else
        {
            xslTransform.Transform(inputUri,
                AddExsltExtensionObjects(arguments), results);
        }
    }

    /// <summary>
    /// Executes the transform using the input document specified by the URI and 
    /// outputs the results to a TextWriter.
    /// </summary>        
    public void Transform(string inputUri, XsltArgumentList arguments, TextWriter results)
    {
        if (multiOutput)
        {
            xslTransform.Transform(inputUri,
               AddExsltExtensionObjects(arguments),
               new MultiXmlTextWriter(results));
        }
        else
        {
            xslTransform.Transform(inputUri,
                AddExsltExtensionObjects(arguments), results);
        }
    }

    /// <summary>
    /// Executes the transform using the input document specified by the URI 
    /// and outputs the results to an XmlWriter. 
    /// The XsltArgumentList provides additional run-time arguments.
    /// </summary>        
    public void Transform(string inputUri, XsltArgumentList arguments, XmlWriter results)
        => xslTransform.Transform(inputUri, AddExsltExtensionObjects(arguments), results);

    /// <summary>
    /// Executes the transform using the input document specified by the XmlReader 
    /// object and outputs the results to a stream. 
    /// The XsltArgumentList provides additional run-time arguments. 
    /// </summary>        
    public void Transform(XmlReader input, XsltArgumentList arguments, Stream results)
    {
        if (multiOutput)
        {
            xslTransform.Transform(input,
                AddExsltExtensionObjects(arguments),
                new MultiXmlTextWriter(results, OutputSettings.Encoding));
        }
        else
        {
            xslTransform.Transform(input,
                AddExsltExtensionObjects(arguments), results);
        }
    }

    /// <summary>
    /// Executes the transform using the input document specified by the XmlReader 
    /// object and outputs the results to a TextWriter. 
    /// The XsltArgumentList provides additional run-time arguments.
    /// </summary>        
    public void Transform(XmlReader input, XsltArgumentList arguments, TextWriter results)
    {
        if (multiOutput)
        {
            xslTransform.Transform(input,
                AddExsltExtensionObjects(arguments),
                new MultiXmlTextWriter(results));
        }
        else
        {
            xslTransform.Transform(input,
                AddExsltExtensionObjects(arguments), results);
        }
    }

    /// <summary>
    /// Executes the transform using the input document specified by the XmlReader 
    /// object and outputs the results to an XmlWriter. 
    /// The XsltArgumentList provides additional run-time arguments. 
    /// </summary>        
    public void Transform(XmlReader input, XsltArgumentList arguments, XmlWriter results)
        => xslTransform.Transform(input, AddExsltExtensionObjects(arguments), results);


    /// <summary>
    /// Executes the transform using the input document specified by the XmlReader 
    /// object and outputs the results to an XmlWriter. 
    /// The XsltArgumentList provides additional run-time arguments and the 
    /// XmlResolver resolves the XSLT document() function. 
    /// </summary>        
    public void Transform(XmlReader input, XsltArgumentList arguments, XmlWriter results, XmlResolver documentResolver) 
        => xslTransform.Transform(input, AddExsltExtensionObjects(arguments), results, documentResolver);

    /// <summary>
    /// Adds the objects that implement the EXSLT extensions to the provided argument 
    /// list. The extension objects added depend on the value of the SupportedFunctions
    /// property.
    /// </summary>
    /// <param name="list">The argument list</param>
    /// <returns>An XsltArgumentList containing the contents of the list passed in 
    /// and objects that implement the EXSLT. </returns>
    /// <remarks>If null is passed in then a new XsltArgumentList is constructed. </remarks>
    XsltArgumentList AddExsltExtensionObjects(XsltArgumentList list)
    {
        list ??= new XsltArgumentList();

        lock (sync)
        {
            //remove all our extension objects in case the XSLT argument list is being reused                
            list.RemoveExtensionObject(ExsltNamespaces.Math);
            list.RemoveExtensionObject(ExsltNamespaces.Random);
            list.RemoveExtensionObject(ExsltNamespaces.DatesAndTimes);
            list.RemoveExtensionObject(ExsltNamespaces.RegularExpressions);
            list.RemoveExtensionObject(ExsltNamespaces.Strings);
            list.RemoveExtensionObject(ExsltNamespaces.Sets);
            list.RemoveExtensionObject(ExsltNamespaces.GdnDatesAndTimes);
            list.RemoveExtensionObject(ExsltNamespaces.GdnMath);
            list.RemoveExtensionObject(ExsltNamespaces.GdnRegularExpressions);
            list.RemoveExtensionObject(ExsltNamespaces.GdnSets);
            list.RemoveExtensionObject(ExsltNamespaces.GdnStrings);
            list.RemoveExtensionObject(ExsltNamespaces.GdnDynamic);

            //add extension objects as specified by SupportedFunctions                

            if ((SupportedFunctions & ExsltFunctionNamespace.Math) > 0)
            {
                list.AddExtensionObject(ExsltNamespaces.Math, exsltMath);
            }

            if ((SupportedFunctions & ExsltFunctionNamespace.Random) > 0)
            {
                list.AddExtensionObject(ExsltNamespaces.Random, exsltRandom);
            }

            if ((SupportedFunctions & ExsltFunctionNamespace.DatesAndTimes) > 0)
            {
                list.AddExtensionObject(ExsltNamespaces.DatesAndTimes, exsltDatesAndTimes);
            }

            if ((SupportedFunctions & ExsltFunctionNamespace.RegularExpressions) > 0)
            {
                list.AddExtensionObject(ExsltNamespaces.RegularExpressions, exsltRegularExpressions);
            }

            if ((SupportedFunctions & ExsltFunctionNamespace.Strings) > 0)
            {
                list.AddExtensionObject(ExsltNamespaces.Strings, exsltStrings);
            }

            if ((SupportedFunctions & ExsltFunctionNamespace.Sets) > 0)
            {
                list.AddExtensionObject(ExsltNamespaces.Sets, exsltSets);
            }

            if ((SupportedFunctions & ExsltFunctionNamespace.GdnDatesAndTimes) > 0)
            {
                list.AddExtensionObject(ExsltNamespaces.GdnDatesAndTimes, gdnDatesAndTimes);
            }

            if ((SupportedFunctions & ExsltFunctionNamespace.GdnMath) > 0)
            {
                list.AddExtensionObject(ExsltNamespaces.GdnMath, gdnMath);
            }

            if ((SupportedFunctions & ExsltFunctionNamespace.GdnRegularExpressions) > 0)
            {
                list.AddExtensionObject(ExsltNamespaces.GdnRegularExpressions, gdnRegularExpressions);
            }

            if ((SupportedFunctions & ExsltFunctionNamespace.GdnSets) > 0)
            {
                list.AddExtensionObject(ExsltNamespaces.GdnSets, gdnSets);
            }

            if ((SupportedFunctions & ExsltFunctionNamespace.GdnStrings) > 0)
            {
                list.AddExtensionObject(ExsltNamespaces.GdnStrings, gdnStrings);
            }

            if ((SupportedFunctions & ExsltFunctionNamespace.GdnDynamic) > 0)
            {
                list.AddExtensionObject(ExsltNamespaces.GdnDynamic, gdnDynamic);
            }
        }

        return list;
    }
}
