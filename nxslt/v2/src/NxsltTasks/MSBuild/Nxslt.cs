using System;
using XmlLab.nxslt;
using System.IO;
using System.Globalization;
using System.Xml.Xsl;
using System.Collections.Specialized;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;

namespace XmlLab.NxsltTasks.MSBuild
{
    

    public class Nxslt : Task
    {
        #region privates
        private NXsltOptions nxsltOptions = new NXsltOptions();
        private Parameter[] xsltParameters;
        //private XsltExtensionObjectCollection xsltExtensions = new XsltExtensionObjectCollection();
        //private FileSet inFiles = new FileSet();
        private string inFile = null;
        private string outFile = null;
        private string extension = "html";
        private DirectoryInfo destDir;
        private string style;
        #endregion


        #region Properties
        /// <summary>Source XML document to be transformed.</summary>        
        public string In
        {
            get { return inFile; }
            set { inFile = value; }
        }

        /// <summary>XSLT stylesheet file. If given as path, it can
        /// be relative to the project's basedir or absolute.</summary>        
        public string Style
        {
            get { return style; }
            set { style = value; }
        }

        /// <summary>Principal output file.</summary>        
        public string Out
        {
            get { return outFile; }
            set { outFile = value; }
        }

        /// <summary>Strip non-significant whitespace from source and stylesheet.</summary>                
        public bool StripWhitespace
        {
            get { return nxsltOptions.StripWhiteSpace; }
            set { nxsltOptions.StripWhiteSpace = value; }
        }

        /// <summary>Resolve external definitions during parse phase.</summary>        
        public bool ResolveExternals
        {
            get { return nxsltOptions.ResolveExternals; }
            set { nxsltOptions.ResolveExternals = value; }
        }

        /// <summary>Process XInclude during parse phase.</summary>        
        public bool ResolveXInclude
        {
            get { return nxsltOptions.ProcessXInclude; }
            set { nxsltOptions.ProcessXInclude = value; }
        }

        /// <summary>Validate documents during parse phase.</summary>        
        public bool Validate
        {
            get { return nxsltOptions.ValidateDocs; }
            set { nxsltOptions.ValidateDocs = value; }
        }

        /// <summary>Show load and transformation timings.</summary>        
        public bool ShowTimings
        {
            get { return nxsltOptions.ShowTiming; }
            set { nxsltOptions.ShowTiming = value; }
        }

        /// <summary>Pretty-print source document.</summary>        
        public bool PrettyPrint
        {
            get { return nxsltOptions.PrettyPrintMode; }
            set { nxsltOptions.PrettyPrintMode = value; }
        }

        /// <summary>Get stylesheet URL from xml-stylesheet PI in source document.</summary>        
        public bool GetStylesheetFromPI
        {
            get { return nxsltOptions.GetStylesheetFromPI; }
            set { nxsltOptions.GetStylesheetFromPI = value; }
        }

        /// <summary>Use named URI resolver class.</summary>        
        public string Resolver
        {
            get { return nxsltOptions.ResolverTypeName; }
            set { nxsltOptions.ResolverTypeName = value; }
        }

        /// <summary>Assembly file name to look up URI resolver class.</summary>        
        public string AssemblyFile
        {
            get { return nxsltOptions.AssemblyFileName; }
            set { nxsltOptions.AssemblyFileName = value; }
        }

        /// <summary>Assembly full or partial name to look up URI resolver class.</summary>        
        public string AssemblyName
        {
            get { return nxsltOptions.AssemblyName; }
            set { nxsltOptions.AssemblyName = value; }
        }

        /// <summary>Allow multiple output documents.</summary>        
        public bool MultiOutput
        {
            get { return nxsltOptions.MultiOutput; }
            set { nxsltOptions.MultiOutput = value; }
        }

        /// <summary>
        /// Credentials in username:password@domain format to be
        /// used in Web request authentications when loading source XML.</summary>        
        public string XmlCredentials
        {
            set { nxsltOptions.SourceCredential = NXsltArgumentsParser.ParseCredentials(value); }
        }

        /// <summary>
        /// Credentials in username:password@domain format to be
        /// used in Web request authentications when loading XSLT stylesheet.
        /// </summary>        
        public string XsltCredentials
        {
            set { nxsltOptions.XSLTCredential = NXsltArgumentsParser.ParseCredentials(value); }
        }

        /// <summary>XSLT parameters to be passed to the XSLT transformation.</summary>        
        public Parameter[] Parameters
        {
            get { return xsltParameters; }
            set { xsltParameters = value; }
        }

        ///// <summary>XSLT extension objects to be passed to the XSLT transformation.</summary>        
        //public XsltExtensionObjectCollection ExtensionObjects
        //{
        //    get { return xsltExtensions; }
        //}

        ///// <summary>Specifies a list of input files to be transformed.</summary>        
        //public FileSet InFiles
        //{
        //    get { return inFiles; }
        //}

        /// <summary>
        /// Desired file extension to be used for the targets. The default is 
        /// <c>html</c>.
        /// </summary>        
        public string Extension
        {
            get { return extension; }
            set { extension = value; }
        }

        /// <summary>
        /// Directory in which to store the results. The default is the project
        /// base directory.
        /// </summary>        
        //public DirectoryInfo DestDir
        //{
        //    get
        //    {
        //        if (destDir == null)
        //        {
        //            return new DirectoryInfo(Project.BaseDirectory);
        //        }
        //        return destDir;
        //    }
        //    set { destDir = value; }
        //}

        #endregion

        public override bool  Execute()
        {
            //if (inFiles.BaseDirectory == null)
            //{
            //    inFiles.BaseDirectory = new DirectoryInfo(Project.BaseDirectory);
            //}

            TaskReporter reporter = new TaskReporter(this);
            int rc = NXsltMain.RETURN_CODE_OK;
            try
            {
                try
                {
                    NXsltMain nxslt = new NXsltMain();
                    nxslt.setReporter(reporter);
                    //if (xsltParameters.Count > 0)
                    //{
                    //    if (nxsltOptions.XslArgList == null)
                    //    {
                    //        nxsltOptions.XslArgList = new XsltArgumentList();
                    //    }
                    //    foreach (XsltParameter param in xsltParameters)
                    //    {
                    //        if (param.IfDefined && !param.UnlessDefined)
                    //        {
                    //            nxsltOptions.XslArgList.AddParam(param.ParameterName,
                    //                param.NamespaceUri, param.Value);
                    //        }
                    //    }
                    //}
                    //if (xsltExtensions.Count > 0)
                    //{
                    //    if (nxsltOptions.XslArgList == null)
                    //    {
                    //        nxsltOptions.XslArgList = new XsltArgumentList();
                    //    }
                    //    foreach (XsltExtensionObject ext in xsltExtensions)
                    //    {
                    //        if (ext.IfDefined && !ext.UnlessDefined)
                    //        {
                    //            object extInstance = ext.CreateInstance();
                    //            nxsltOptions.XslArgList.AddExtensionObject(
                    //                ext.NamespaceUri, extInstance);
                    //        }
                    //    }
                    //}
                    nxslt.options = nxsltOptions;
                    if (style != null)
                    {
                        nxslt.options.Stylesheet = style;
                    }

                    StringCollection srcFiles = null;
                    if (inFile != null)
                    {
                        srcFiles = new StringCollection();
                        srcFiles.Add(inFile);
                    }
                    //else if (InFiles.FileNames.Count > 0)
                    //{

                    //    if (outFile != null)
                    //    {
                    //        throw new NxsltTaskException("The 'out' attribute cannot be used when <infiles> is specified.",
                    //            Location);
                    //    }
                    //    srcFiles = inFiles.FileNames;
                    //}

                    if (srcFiles == null || srcFiles.Count == 0)
                    {
                        throw new NxsltTaskException("No source files indicated; use 'in' or <infiles>.");
                    }

                    if (outFile == null && destDir == null)
                    {
                        throw new NxsltTaskException("'out' and 'destdir' cannot be both omitted.");
                    }

                    foreach (string file in srcFiles)
                    {
                        Log.LogMessage(MessageImportance.Normal, "Transforming " + file);
                        nxslt.options.Source = file;
                        if (outFile != null)
                        {
                            nxslt.options.OutFile = outFile;
                        }
                        else
                        {
                            string destFile = Path.GetFileNameWithoutExtension(file) + "." + extension;                            
                            nxslt.options.OutFile = Path.Combine(destDir.FullName, destFile);
                        }
                        rc = nxslt.Process();
                        if (rc != NXsltMain.RETURN_CODE_OK)
                        {
                            throw new NxsltTaskException(
                                string.Format(CultureInfo.InvariantCulture,
                                "nxslt task failed.", rc));
                        }
                    }
                }
                catch (NXsltCommandLineParsingException clpe)
                {
                    //There was an exception while parsing command line
                    reporter.ReportCommandLineParsingError(Reporter.GetFullMessage(clpe));
                    throw new NxsltTaskException(
                            "nxslt task failed to parse parameters.", clpe);
                }
                catch (NXsltException ne)
                {
                    reporter.ReportError(Reporter.GetFullMessage(ne));
                    throw new NxsltTaskException(
                            "nxslt task failed.", ne);
                }
            }
            catch (Exception e)
            {                
                reporter.ReportError(NXsltStrings.Error, Reporter.GetFullMessage(e));
                return false;
            }
            return true;
        }
    }

    public class Parameter
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
	
    }
}
