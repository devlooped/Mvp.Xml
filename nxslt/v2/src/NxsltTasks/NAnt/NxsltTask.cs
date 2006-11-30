using System;
using NAnt.Core;
using NAnt.Core.Attributes;
using XmlLab.nxslt;
using System.IO;

namespace XmlLab.NxsltTasks.NAnt
{
    [TaskName("nxslt")]
    public class NxsltTask : Task
    {
        #region privates
        private NXsltOptions nxsltOptions = new NXsltOptions();                       
        #endregion        

        #region Properties
        /// <summary>Source XML document to be transformed.</summary>
        [TaskAttribute("doc", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public string Doc
        {
            get { return nxsltOptions.Source; }
            set { nxsltOptions.Source = Project.ExpandProperties(value, Location); }
        }

        /// <summary>XSLT stylesheet file.</summary>
        [TaskAttribute("stylesheet", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public string Stylesheet
        {
            get { return nxsltOptions.Stylesheet; }
            set { nxsltOptions.Stylesheet = Project.ExpandProperties(value, Location); }
        }

        /// <summary>Principal output file.</summary>
        [TaskAttribute("output", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public string Output
        {
            get { return nxsltOptions.OutFile; }
            set { nxsltOptions.OutFile = Project.ExpandProperties(value, Location); }
        }

        /// <summary>Strip non-significant whitespace from source and stylesheet.</summary>
        [TaskAttribute("strip-whitespace")]
        [BooleanValidator()]
        public bool StripWhitespace
        {
            get { return nxsltOptions.StripWhiteSpace; }
            set { nxsltOptions.StripWhiteSpace = value; }
        }

        /// <summary>Resolve external definitions during parse phase.</summary>
        [TaskAttribute("resolve-externals")]
        [BooleanValidator()]
        public bool ResolveExternals
        {
            get { return nxsltOptions.ResolveExternals; }
            set { nxsltOptions.ResolveExternals = value; }
        }

        /// <summary>Process XInclude during parse phase.</summary>
        [TaskAttribute("resolve-xinclude")]
        [BooleanValidator()]
        public bool ResolveXInclude
        {
            get { return nxsltOptions.ProcessXInclude; }
            set { nxsltOptions.ProcessXInclude = value; }
        }

        /// <summary>Validate documents during parse phase.</summary>
        [TaskAttribute("validate")]
        [BooleanValidator()]
        public bool Validate
        {
            get { return nxsltOptions.ValidateDocs; }
            set { nxsltOptions.ValidateDocs = value; }
        }

        /// <summary>Show load and transformation timings.</summary>
        [TaskAttribute("show-timings")]
        [BooleanValidator()]
        public bool ShowTimings
        {
            get { return nxsltOptions.ShowTiming; }
            set { nxsltOptions.ShowTiming = value; }
        }

        /// <summary>Pretty-print source document.</summary>
        [TaskAttribute("pretty-print")]
        [BooleanValidator()]
        public bool PrettyPrint
        {
            get { return nxsltOptions.PrettyPrintMode; }
            set { nxsltOptions.PrettyPrintMode = value; }
        }

        /// <summary>Get stylesheet URL from xml-stylesheet PI in source document.</summary>
        [TaskAttribute("get-stylesheet-from-pi")]
        [BooleanValidator()]
        public bool GetStylesheetFromPI
        {
            get { return nxsltOptions.GetStylesheetFromPI; }
            set { nxsltOptions.GetStylesheetFromPI = value; }
        }

        /// <summary>Use named URI resolver class.</summary>
        [TaskAttribute("resolver")]
        public string Resolver
        {
            get { return nxsltOptions.ResolverTypeName; }
            set { nxsltOptions.ResolverTypeName = Project.ExpandProperties(value, Location); }
        }

        /// <summary>Assembly file name to look up URI resolver class.</summary>
        [TaskAttribute("assembly-file")]
        public string AssemblyFile
        {
            get { return nxsltOptions.AssemblyFileName; }
            set { nxsltOptions.AssemblyFileName = Project.ExpandProperties(value, Location); }
        }

        /// <summary>Assembly full or partial name to look up URI resolver class.</summary>
        [TaskAttribute("assembly-name")]
        public string AssemblyName
        {
            get { return nxsltOptions.AssemblyName; }
            set { nxsltOptions.AssemblyName = Project.ExpandProperties(value, Location); }
        }

        /// <summary>Allow multiple output documents.</summary>
        [TaskAttribute("multi-output")]
        [BooleanValidator()]
        public bool MultiOutput
        {
            get { return nxsltOptions.MultiOutput; }
            set { nxsltOptions.MultiOutput = value; }
        }

        /// <summary>Comma-separated list of extension object class names.</summary>
        [TaskAttribute("extentions")]
        public string Extentions
        {
            set { nxsltOptions.ExtClasses = Project.ExpandProperties(value, Location).Split(','); }
        }

        /// <summary>
        /// Credentials in username:password@domain format to be
        /// used in Web request authentications when loading source XML.</summary>
        [TaskAttribute("xml-credentials")]
        public string XmlCredentials
        {
            set { nxsltOptions.SourceCredential = NXsltArgumentsParser.ParseCredentials(Project.ExpandProperties(value, Location)); }
        }

        /// <summary>
        /// Credentials in username:password@domain format to be
        /// used in Web request authentications when loading XSLT stylesheet.
        /// </summary>
        [TaskAttribute("xslt-credentials")]
        public string XsltCredentials
        {
            set { nxsltOptions.XSLTCredential = NXsltArgumentsParser.ParseCredentials(Project.ExpandProperties(value, Location)); }
        } 
        #endregion

        protected override void ExecuteTask()
        {
            StringWriter stdout = new StringWriter();
            StringWriter stderr = new StringWriter();
            Reporter reporter = new Reporter(stdout, stderr);
            try
            {
                NXsltMain nxslt = new NXsltMain();
                nxslt.setReporter(reporter);
                nxslt.options = nxsltOptions;                                
                int rc = nxslt.Process();
                if (rc != NXsltMain.RETURN_CODE_OK)
                {
                    throw new Exception();
                }
            }
            catch (NXsltCommandLineParsingException clpe)
            {
                //There was an exception while parsing command line
                reporter.ReportCommandLineParsingError(Reporter.GetFullMessage(clpe));
                throw new Exception();
            }
            catch (NXsltException ne)
            {
                reporter.ReportError(Reporter.GetFullMessage(ne));
                throw;
            }
            catch (Exception e)
            {
                //Some other exception
                reporter.ReportError(NXsltStrings.Error, Reporter.GetFullMessage(e));
                throw;
            }
        }
    }
}
