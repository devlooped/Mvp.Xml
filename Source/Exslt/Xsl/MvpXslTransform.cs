#region using
using System;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Reflection;
using System.CodeDom.Compiler;
using System.Collections.Generic;

using Mvp.Xml.Exslt;

#endregion

namespace Mvp.Xml.Common.Xsl
{
     
    /// <summary>
    /// <para>MvpXslTransform class extends capabilities of the <see cref="XslCompiledTransform"/>
    /// class by adding support for transforming into <see cref="XmlReader"/>, 
    /// built-in vast collection of EXSLT extention functions, multiple outputs, XSLT 2.0 character
    /// maps and transforming of <see cref="IXPathNavigable"/> along with <see cref="XmlResolver"/>.
    /// Also MvpXslTransform class provides new improved XSL transformation API 
    /// by introducing concepts of <see cref="IXmlTransform"/> interface, <see cref="XmlInput"/>
    /// and <see cref="XmlOutput"/>.</para>    
    /// </summary>
    /// <remarks><para>MvpXslTransform class is thread-safe for Transorm() methods. I.e.
    /// once MvpXslTransform object is loaded, you can safely call its Transform() methods
    /// in multiple threads simultaneously.</para>
    /// <para>MvpXslTransform supports EXSLT extension functions from the following namespaces:<br/> 
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
    /// * http://gotdotnet.com/exslt/dynamic</para>
    /// <para>Multioutput (&lt;exsl:document&gt; element) is turned off by default and can 
    /// be turned on using <see cref="MvpXslTransform.MultiOutput"/> property. Note, that multioutput is not supported
    /// when transfomation is done to <see cref="XmlWriter"/> or <see cref="XmlReader"/>.</para>
    /// <para>MvpXslTransform uses XSLT extension objects and reflection and so using
    /// it requires FullTrust security level.</para>
    /// <para>Author: Sergey Dubinets, Microsoft XML Team.</para>
    /// <para>Contributors: Oleg Tkachenko, <a href="http://www.xmllab.net">http://www.xmllab.net</a>.</para>
    /// </remarks>

    public class MvpXslTransform : IXmlTransform
    {
        /// <summary>
        /// Real transformation engine
        /// </summary>
        private XslCompiledTransform compiledTransform;        

        private object sync = new object();
        /// <summary>
        /// Supported EXSLT functions
        /// </summary>
        private ExsltFunctionNamespace supportedFunctions = ExsltFunctionNamespace.All;
        /// <summary>
        /// Multioutput support flag
        /// </summary>
        private bool multiOutput;
        /// <summary>
        /// XSLT 2.0 like character maps support flag.
        /// </summary>
        private bool supportCharacterMaps = true;
        private Dictionary<char, string> characterMap;
        private bool enforceXHTMLOutput;

        #region ctors

        /// <summary>
        /// Initializes a new instance of the MvpXslTransform class. 
        /// </summary>
        public MvpXslTransform()
        {
            this.compiledTransform = new XslCompiledTransform();
        }

        /// <summary>
        /// Initializes a new instance of the MvpXslTransform 
        /// class with the specified debug setting. 
        /// </summary>
        public MvpXslTransform(bool debug)
        {
            this.compiledTransform = new XslCompiledTransform(debug);
        }

        #endregion

        #region Load() method overloads

        /// <summary>
        /// Loads and compiles the style sheet contained in the <see cref="IXPathNavigable"/> object.
        /// See also <see cref="XslCompiledTransform.Load(IXPathNavigable)"/>.
        /// </summary>
        /// <param name="stylesheet">An object implementing the <see cref="IXPathNavigable"/> interface. 
        /// In the Microsoft .NET Framework, this can be either an <see cref="XmlNode"/> (typically an <see cref="XmlDocument"/>), 
        /// or an <see cref="XPathDocument"/> containing the style sheet.</param>
        public void Load(IXPathNavigable stylesheet)
        {
            LoadStylesheetFromReader(stylesheet.CreateNavigator().ReadSubtree());            
        }

        /// <summary>
        /// Loads and compiles the style sheet located at the specified URI.
        /// See also <see cref="XslCompiledTransform.Load(String)"/>.
        /// </summary>
        /// <param name="stylesheetUri">The URI of the style sheet.</param>
        public void Load(string stylesheetUri)
        {
            LoadStylesheetFromReader(XmlReader.Create(stylesheetUri));
        }

        /// <summary>
        /// Compiles the style sheet contained in the <see cref="XmlReader"/>.
        /// See also <see cref="XslCompiledTransform.Load(XmlReader)"/>.
        /// </summary>
        /// <param name="stylesheet">An <see cref="XmlReader"/> containing the style sheet.</param>
        public void Load(XmlReader stylesheet) 
        {
            LoadStylesheetFromReader(stylesheet);
        }

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
        public void Load(IXPathNavigable stylesheet, XsltSettings settings, XmlResolver stylesheetResolver)
        {
            LoadStylesheetFromReader(stylesheet.CreateNavigator().ReadSubtree(), settings, stylesheetResolver);
        }

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
        public void Load(string stylesheetUri, XsltSettings settings, XmlResolver stylesheetResolver)
        {
            LoadStylesheetFromReader(XmlReader.Create(stylesheetUri), settings, stylesheetResolver);
        }

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
        public void Load(XmlReader stylesheet, XsltSettings settings, XmlResolver stylesheetResolver)
        {
            LoadStylesheetFromReader(stylesheet, settings, stylesheetResolver);
        }

        #endregion

        #region public properties

        /// <summary>
        /// Bitwise enumeration used to specify which EXSLT functions should be accessible to 
        /// the MvpXslTransform object. The default value is ExsltFunctionNamespace.All 
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
            get { return multiOutput; }
            set { multiOutput = value; }
        }

        /// <summary>
        /// Enforces <see cref="MvpXslTransform"/> instance to output results in XHTML format.
        /// </summary>
        public bool EnforceXHTMLOutput
        {
            get
            {
                return enforceXHTMLOutput;
            }
            set
            {
                enforceXHTMLOutput = value;
            }
        }

        /// <summary>
        /// Boolean flag used to specify whether XSLT 2.0 character maps are
        /// supported.
        /// </summary>
        /// <remarks>Note: MvpXslTransform is XSLT 1.0 processor, so 
        /// XSLT 2.0 character maps must be defined in custom "http://www.xmllab.net/nxslt"
        /// namespace. Here is a sample:
        /// &lt;xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
        ///     xmlns:nxslt="http://www.xmllab.net/nxslt">
        ///     &lt;nxslt:output use-character-maps="testmap"/>
        ///     &lt;nxslt:character-map name="testmap">
        ///         &lt;nxslt:output-character character="&amp;#160;" string="&amp;amp;nbsp;" />
        ///     &lt;/nxslt:character-map>
        /// &lt;/xsl:stylesheet>
        /// </remarks>
        public bool SupportCharacterMaps 
        {
            get { return supportCharacterMaps; }
            set { supportCharacterMaps = value; }
        }

        /// <summary>
        /// Gets the TempFileCollection that contains the temporary files generated 
        /// on disk after a successful call to the Load method. 
        /// </summary>
        public TempFileCollection TemporaryFiles
        {
            get { return this.compiledTransform.TemporaryFiles; }
        }

        #endregion

        #region IXmlTransform impl

        /// <summary>
        /// Transforms given <see cref="XmlInput"/> into <see cref="XmlOutput"/>.
        /// The <see cref="XsltArgumentList"/> provides additional runtime arguments.
        /// </summary>
        /// <param name="input">Default input XML document.</param>
        /// <param name="arguments">An <see cref="XsltArgumentList"/> containing the namespace-qualified 
        /// arguments used as input to the transform. This value can be a null reference (Nothing in Visual Basic).</param>
        /// <param name="output">Represents the transformation's output.</param>
        /// <returns>Transformation output.</returns>
        public XmlOutput Transform(XmlInput input, XsltArgumentList arguments, XmlOutput output)
        {
            if (input == null) throw new ArgumentNullException("defaltDocument");
            XmlWriter xmlWriter = output.destination as XmlWriter;
            bool closeWriter = false;
            if (xmlWriter == null)
            {
                closeWriter = true;
                while (true)
                {
                    TextWriter txtWriter = output.destination as TextWriter;
                    if (txtWriter != null)
                    {
                        if (multiOutput)
                        {
                            MultiXmlTextWriter mw = new MultiXmlTextWriter(txtWriter, output.XmlResolver);

                            if (this.compiledTransform.OutputSettings.Indent)
                            {
                                mw.Formatting = Formatting.Indented;
                            }
                            xmlWriter = mw;
                        }
                        else
                        {
                            xmlWriter = XmlWriter.Create(txtWriter, this.compiledTransform.OutputSettings);
                        }
                        break;
                    }
                    Stream strm = output.destination as Stream;
                    if (strm != null)
                    {
                        if (multiOutput)
                        {

                            MultiXmlTextWriter mw = new MultiXmlTextWriter(strm, this.compiledTransform.OutputSettings.Encoding, output.XmlResolver);
                            if (this.compiledTransform.OutputSettings.Indent)
                            {
                                mw.Formatting = Formatting.Indented;
                            }
                            xmlWriter = mw;
                        }
                        else
                        {
                            xmlWriter = XmlWriter.Create(strm, this.compiledTransform.OutputSettings);
                        }
                        break;
                    }
                    String str = output.destination as String;
                    if (str != null)
                    {
                        if (multiOutput)
                        {
                            MultiXmlTextWriter mw = new MultiXmlTextWriter(str, this.compiledTransform.OutputSettings.Encoding);
                            if (this.compiledTransform.OutputSettings.Indent)
                            {
                                mw.Formatting = Formatting.Indented;
                            }
                            xmlWriter = mw;
                        }
                        else
                        {
                            XmlWriterSettings outputSettings = this.compiledTransform.OutputSettings.Clone();
                            outputSettings.CloseOutput = true;
                            // BugBug: We should read doc before creating output file in case they are the same
                            xmlWriter = XmlWriter.Create(str, outputSettings);
                        }
                        break;
                    }
                    throw new Exception("Unexpected XmlOutput");
                }
            }
            try
            {
                TransformToWriter(input, arguments, xmlWriter);
            }
            finally
            {
                if (closeWriter)
                {
                    xmlWriter.Close();
                }
            }
            return output;
        }

        /// <summary>
        /// Gets an <see cref="XmlWriterSettings"/> object that contains the output 
        /// information derived from the xsl:output element of the style sheet.
        /// </summary>
        public XmlWriterSettings OutputSettings
        {
            get
            {
                if (this.compiledTransform != null)
                {
                    return this.compiledTransform.OutputSettings;
                }
                else
                {
                    return new XmlWriterSettings();
                }
            }
        }

        #endregion

        #region additional Transform() methods

        /// <summary>
        /// Transforms given <see cref="XmlInput"/> into <see cref="XmlReader"/>.
        /// </summary>
        /// <param name="input">Default input XML document</param>
        /// <param name="arguments">An <see cref="XsltArgumentList"/> containing the namespace-qualified 
        /// arguments used as input to the transform. This value can be a null reference (Nothing in Visual Basic).</param>
        public XmlReader Transform(XmlInput input, XsltArgumentList arguments)
        {
            XslReader r = new XslReader(this.compiledTransform);
            r.StartTransform(input, AddExsltExtensionObjectsSync(arguments));
            return r;
        }

        /// <summary>
        /// Transforms given <see cref="XmlInput"/> into <see cref="XmlReader"/>.
        /// </summary>
        /// <param name="input">Default input XML document</param>
        /// <param name="arguments">An <see cref="XsltArgumentList"/> containing the namespace-qualified 
        /// arguments used as input to the transform. This value can be a null reference (Nothing in Visual Basic).</param>
        /// <param name="multiThread">Defines in which mode (multithreaded or singlethreaded)
        /// this instance of XslReader will operate</param>
        /// <param name="initialBufferSize">Initial buffer size (number of nodes, not bytes)</param>
        public XmlReader Transform(XmlInput input, XsltArgumentList arguments, bool multiThread, int initialBufferSize)
        {
            XslReader r = new XslReader(this.compiledTransform, multiThread, initialBufferSize);
            r.StartTransform(input, AddExsltExtensionObjectsSync(arguments));
            return r;
        }

        #endregion

        #region private stuff

        /// <summary>
        /// Core XSLT engine.
        /// </summary>
        internal XslCompiledTransform CompiledTransform
        {
            get { return compiledTransform; }
        }        

        /// <summary>
        /// Default XML Reader settings
        /// </summary>
        protected static XmlReaderSettings DefaultReaderSettings;
        static MvpXslTransform()
        {
            DefaultReaderSettings = new XmlReaderSettings();
            DefaultReaderSettings.ProhibitDtd = false;
        }

        /// <summary>
        /// Gets XML Reader settings (customized if there is custom XML resolver)
        /// </summary>        
        /// <returns></returns>
        protected XmlReaderSettings GetReaderSettings(XmlInput defaultDocument)
        {
            if (defaultDocument.resolver is DefaultXmlResolver)
            {
                return DefaultReaderSettings;
            }
            else
            {
                XmlReaderSettings settings = DefaultReaderSettings.Clone();
                settings.XmlResolver = defaultDocument.resolver;
                return settings;
            }
        }

        /// <summary>
        /// Transforms to XmlWriter.
        /// </summary>        
        protected void TransformToWriter(XmlInput defaultDocument, XsltArgumentList xsltArgs, XmlWriter targetWriter)
        {
            XmlWriter xmlWriter;            
            if (this.supportCharacterMaps && this.characterMap != null && this.characterMap.Count > 0)
            {
                xmlWriter = new CharacterMappingXmlWriter(targetWriter, this.characterMap);
            }
            else
            {
                xmlWriter = targetWriter;
            }
            if (this.enforceXHTMLOutput)
            {
                xmlWriter = new XhtmlWriter(xmlWriter);
            }
            XsltArgumentList args = AddExsltExtensionObjectsSync(xsltArgs);
            XmlReader xmlReader = defaultDocument.source as XmlReader;
            if (xmlReader != null)
            {
                this.compiledTransform.Transform(xmlReader, args, xmlWriter, defaultDocument.resolver);
                return;
            }
            IXPathNavigable nav = defaultDocument.source as IXPathNavigable;
            if (nav != null)
            {
                if (defaultDocument.resolver is DefaultXmlResolver)
                {
                    this.compiledTransform.Transform(nav, args, xmlWriter);
                }
                else
                {
                    TransformIXPathNavigable(nav, args, xmlWriter, defaultDocument.resolver);
                }
                return;
            }
            string str = defaultDocument.source as string;
            if (str != null)
            {
                using (XmlReader reader = XmlReader.Create(str, GetReaderSettings(defaultDocument)))
                {
                    this.compiledTransform.Transform(reader, args, xmlWriter, defaultDocument.resolver);
                }
                return;
            }
            Stream strm = defaultDocument.source as Stream;
            if (strm != null)
            {
                using (XmlReader reader = XmlReader.Create(strm, GetReaderSettings(defaultDocument)))
                {
                    this.compiledTransform.Transform(reader, args, xmlWriter, defaultDocument.resolver);
                }
                return;
            }
            TextReader txtReader = defaultDocument.source as TextReader;
            if (txtReader != null)
            {
                using (XmlReader reader = XmlReader.Create(txtReader, GetReaderSettings(defaultDocument)))
                {
                    this.compiledTransform.Transform(reader, args, xmlWriter, defaultDocument.resolver);
                }
                return;
            }
            throw new Exception("Unexpected XmlInput");
        }

        /// <summary>
        /// Transforms to IXPathNavigable.
        /// </summary>        
        protected void TransformIXPathNavigable(IXPathNavigable nav, XsltArgumentList args, XmlWriter xmlWriter, XmlResolver resolver)
        {
            object command = this.compiledTransform.GetType().GetField(
                "command", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(this.compiledTransform);
            MethodInfo executeMethod = command.GetType().GetMethod("Execute", BindingFlags.Instance | BindingFlags.Public,
                null, new Type[] { typeof(IXPathNavigable), typeof(XmlResolver), typeof(XsltArgumentList), typeof(XmlWriter) }, null);
            executeMethod.Invoke(command,
                new object[] { nav, resolver, AddExsltExtensionObjectsSync(args), xmlWriter });
        }

        /// <summary>
        /// Loads XSLT stylesheet from <see cref="XmlReader"/>.
        /// Wraps the reader if character maps are supported.
        /// </summary>        
        protected void LoadStylesheetFromReader(XmlReader reader)
        {
            LoadStylesheetFromReader(reader, XsltSettings.Default, new XmlUrlResolver());
        }

        /// <summary>
        /// Loads XSLT stylesheet from <see cref="XmlReader"/>, with given settings and resolver.
        /// Wraps the reader if character maps are supported.
        /// </summary>        
        protected void LoadStylesheetFromReader(XmlReader reader, XsltSettings settings, XmlResolver resolver)
        {            
            if (supportCharacterMaps)
            {
                CharacterMappingXmlReader cmr = new CharacterMappingXmlReader(reader);
                this.compiledTransform.Load(cmr, settings, resolver);
                this.characterMap = cmr.CompileCharacterMapping();
            }
            else
            {
                this.compiledTransform.Load(reader, settings, resolver);
            }            
        }

        /// <summary>
        /// Adds the objects that implement the EXSLT extensions to the provided argument 
        /// list. The extension objects added depend on the value of the SupportedFunctions
        /// property.
        /// </summary>
        /// <param name="list">The argument list</param>
        /// <returns>An XsltArgumentList containing the contents of the list passed in 
        /// and objects that implement the EXSLT. </returns>
        /// <remarks>If null is passed in then a new XsltArgumentList is constructed. </remarks>
        protected XsltArgumentList AddExsltExtensionObjectsSync(XsltArgumentList list)
        {
            lock (sync)
            {
                list = AddExsltExtensionObjects(list, this.SupportedFunctions);
            }
            return list;
        }

        /// <summary>
        /// Adds the extension objects that implement all EXSLT functions to the provided 
        /// argument list.
        /// </summary>
        /// <param name="list">The argument list</param>        
        /// <returns>An XsltArgumentList containing required EXSLT extension objects. </returns>
        /// <remarks>If null is passed in then a new XsltArgumentList is constructed. </remarks>
        public static XsltArgumentList AddExsltExtensionObjects(XsltArgumentList list)
        {
            return AddExsltExtensionObjects(list, ExsltFunctionNamespace.All);
        }

        /// <summary>
        /// Adds the extension objects that implement all EXSLT functions to the provided 
        /// argument list. The extension objects added depend on the value of the supportedFunctions
        /// argument.
        /// </summary>
        /// <param name="list">The argument list</param>
        /// <param name="supportedFunctions">Required EXSLT modules</param>
        /// <returns>An XsltArgumentList containing required EXSLT extension objects. </returns>
        /// <remarks>If null is passed in then a new XsltArgumentList is constructed. </remarks>
        public static XsltArgumentList AddExsltExtensionObjects(XsltArgumentList list, ExsltFunctionNamespace supportedFunctions)
        {
            if (list == null)
            {
                list = new XsltArgumentList();
            }

            //remove all our extension objects in case the XSLT argument list is being reused                
            list.RemoveExtensionObject(ExsltNamespaces.Math);
            list.RemoveExtensionObject(ExsltNamespaces.Random);
            list.RemoveExtensionObject(ExsltNamespaces.DatesAndTimes);
            list.RemoveExtensionObject(ExsltNamespaces.RegularExpressions);
            list.RemoveExtensionObject(ExsltNamespaces.Strings);
            list.RemoveExtensionObject(ExsltNamespaces.Sets);
            list.RemoveExtensionObject(ExsltNamespaces.GDNDatesAndTimes);
            list.RemoveExtensionObject(ExsltNamespaces.GDNMath);
            list.RemoveExtensionObject(ExsltNamespaces.GDNRegularExpressions);
            list.RemoveExtensionObject(ExsltNamespaces.GDNSets);
            list.RemoveExtensionObject(ExsltNamespaces.GDNStrings);
            list.RemoveExtensionObject(ExsltNamespaces.GDNDynamic);

            //add extension objects as specified by SupportedFunctions                

            if ((supportedFunctions & ExsltFunctionNamespace.Math) > 0)
            {
                list.AddExtensionObject(ExsltNamespaces.Math, new ExsltMath());
            }

            if ((supportedFunctions & ExsltFunctionNamespace.Random) > 0)
            {
                list.AddExtensionObject(ExsltNamespaces.Random, new ExsltRandom());
            }

            if ((supportedFunctions & ExsltFunctionNamespace.DatesAndTimes) > 0)
            {
                list.AddExtensionObject(ExsltNamespaces.DatesAndTimes, new ExsltDatesAndTimes());
            }

            if ((supportedFunctions & ExsltFunctionNamespace.RegularExpressions) > 0)
            {
                list.AddExtensionObject(ExsltNamespaces.RegularExpressions, new ExsltRegularExpressions());
            }

            if ((supportedFunctions & ExsltFunctionNamespace.Strings) > 0)
            {
                list.AddExtensionObject(ExsltNamespaces.Strings, new ExsltStrings());
            }

            if ((supportedFunctions & ExsltFunctionNamespace.Sets) > 0)
            {
                list.AddExtensionObject(ExsltNamespaces.Sets, new ExsltSets());
            }

            if ((supportedFunctions & ExsltFunctionNamespace.GDNDatesAndTimes) > 0)
            {
                list.AddExtensionObject(ExsltNamespaces.GDNDatesAndTimes, new GDNDatesAndTimes());
            }

            if ((supportedFunctions & ExsltFunctionNamespace.GDNMath) > 0)
            {
                list.AddExtensionObject(ExsltNamespaces.GDNMath, new GDNMath());
            }

            if ((supportedFunctions & ExsltFunctionNamespace.GDNRegularExpressions) > 0)
            {
                list.AddExtensionObject(ExsltNamespaces.GDNRegularExpressions, new GDNRegularExpressions());
            }

            if ((supportedFunctions & ExsltFunctionNamespace.GDNSets) > 0)
            {
                list.AddExtensionObject(ExsltNamespaces.GDNSets, new GDNSets());
            }

            if ((supportedFunctions & ExsltFunctionNamespace.GDNStrings) > 0)
            {
                list.AddExtensionObject(ExsltNamespaces.GDNStrings, new GDNStrings());
            }

            if ((supportedFunctions & ExsltFunctionNamespace.GDNDynamic) > 0)
            {
                list.AddExtensionObject(ExsltNamespaces.GDNDynamic, new GDNDynamic());
            }

            return list;
        }
        #endregion
    }
}
