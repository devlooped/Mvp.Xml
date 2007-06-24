using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.XPath;
using System.Text;
using System.Collections.Generic;

namespace DocSite {
    public static class DocSiteNavigator {
        #region Public Properties
        public static readonly string HelpFileNotFoundPath = VirtualPathUtility.ToAbsolute(Properties.Settings.Default.HelpFileNotFoundPath);
        public static readonly string FileNotFoundPath = VirtualPathUtility.ToAbsolute(Properties.Settings.Default.FileNotFoundPath);
        public static readonly string VirtualHelpPath = VirtualPathUtility.ToAbsolute(Properties.Settings.Default.VirtualHelpPath);

        public static XPathDocument DocSiteContentsDocument {
            get {
                // If the DocSiteContents.xml file is quite large and its affect on memory consumption is unacceptable, then remove
                // the lazy initialization code from this property get accessor and replace it with code to create a new instance each 
                // time the property is read, but realize that you are then making a trade-off between memory consumption and performance: 
                // return new XPathDocument(HttpContext.Current.Server.MapPath(Properties.Settings.Default.DocSiteContentsXmlSource));

                if (docSiteContentsDocument == null) {
                    lock (sync) {
                        if (docSiteContentsDocument == null)
                            docSiteContentsDocument = new XPathDocument(HttpContext.Current.Server.MapPath(
                                Properties.Settings.Default.DocSiteContentsXmlSource));
                    }
                }

                return docSiteContentsDocument;
            }
        }
        #endregion

        #region Private / Protected
        private volatile static XPathDocument docSiteContentsDocument;
        private static readonly object sync = new object();
        #endregion

        #region Methods
        public static IEnumerable<string> GetSubTopics(string topic) {
            if (string.IsNullOrEmpty(topic))
                yield break;

            string xPath = ConvertTopicToXPath(topic, false);

            if (xPath == "//*[@name]")
                yield break;

            foreach (XPathNavigator node in GetDocSiteContentsNode(xPath).SelectChildren(XPathNodeType.Element)) {
                // GetAttribute calls must specify an empty string for the namespaceURI argument; null does not work.
                yield return node.GetAttribute("name", "");
            }
        }

        /// <summary>
        /// Redirects the home page IFRAME to the specified help file if the home page is handling the current request; otherwise, redirects the user to the home page for the specified help file.
        /// </summary>
        /// <remarks>
        /// The specified help file must have a corresponding topic entry in the DocSiteContents.xml file.
        /// </remarks>
        /// <param name="helpFile">Virtual path of the help file to which the user or home page IFRAME will be redirected.</param>
        public static void NavigateToHelpFile(string helpFile) {
            HttpContext context = HttpContext.Current;

            if (context == null)
                throw new InvalidOperationException("NavigateToHelpFile cannot operate without a request context.");

            Default homePage = context.CurrentHandler as Default;

            if (homePage != null)
                // redirect IFRAME
                homePage.ContentPath = helpFile ?? HelpFileNotFoundPath;
            else
                context.Response.Redirect(VirtualHelpPath + "?helpfile=" + helpFile, true);
        }

        /// <summary>
        /// Resolves the corresponding topic for the specified help file.
        /// </summary>
        /// <remarks>
        /// The specified help file must have a corresponding topic entry in the DocSiteContents.xml file.
        /// </remarks>
        /// <param name="urlEncode">Specifies whether each individual topic name should be url-encoded.</param>
        /// <param name="helpFile">Virtual path of the help file for which the topic will be returned.</param>
        /// <returns>The complete hierarchical path to the topic, which may be used as the <i>topic</i> argument to the <see cref="NavigateToTopic" /> and <see cref="ResolveTopicHelpFile" /> methods.</returns>
        public static string ResolveHelpFileTopic(string helpFile, bool urlEncode) {
            if (string.IsNullOrEmpty(helpFile))
                return null;

            XPathNavigator node = GetDocSiteContentsNode(
                string.Format("//*[@file=\"{0}\"]", helpFile.ToLowerInvariant()));

            if (node == null)
                return null;

            StringBuilder topic = new StringBuilder();

            do {
                // GetAttribute calls must specify an empty string for the namespaceURI argument; null does not work.
                string name = node.GetAttribute("name", "");

                if (urlEncode)
                    name = HttpUtility.UrlEncode(name);

                // do not trim name since leading and trailing spaces are valid
                topic.Insert(0, name + "/");
            }
            // NOTE: XPathNodeType.Root does not refer to the root element; just the "beginning" of the document
            while (node.MoveToParent() && node.NodeType != XPathNodeType.Root);

            if (topic.Length > 0)
                // remove trailing /
                topic.Remove(topic.Length - 1, 1);

            return topic.ToString();
        }

        /// <summary>
        /// Redirects the home page IFRAME to the specified help <paramref name="topic" /> if the home page is handling the current request; otherwise, redirects the user to the home page for the specified <paramref name="topic" />.
        /// </summary>
        /// <remarks>
        /// <para>The <paramref name="topic" /> parameter must include the names of the topics above it in its hierarchy with each topic name separated by a slash, like a file path.</para>
        /// <para>The following example illustrates the appropriate value for the <paramref name="topic" /> parameter when navigating to a topic named, "Form1 Constructor", which appears under a topic named, "Form1", which
        /// is located at the root of the help system named, "Namespaces": </para>
        /// <example>DocSiteNavigator.NavigateToTopic("Namespaces/Form1/Form1 Constructor");</example>
        /// <para>The specified topic names must correspond exactly to the name attributes of topics with the same hierarchy in the DocSiteContents.xml file.</para>
        /// </remarks>
        /// <param name="topic">Name of the topic that the user will be redirected to, including all topics in its hierarchy, with each topic separated by a slash.</param>
        public static void NavigateToTopic(string topic, bool urlDecode) {
            HttpContext context = HttpContext.Current;

            if (context == null)
                throw new InvalidOperationException("NavigateToTopic cannot operate without a request context.");

            Default homePage = context.CurrentHandler as Default;

            if (homePage != null)
                // redirect IFRAME
                homePage.ContentPath = ResolveTopicHelpFile(topic, urlDecode) ?? HelpFileNotFoundPath;
            else
                context.Response.Redirect(GetTopicUrl(topic, true), true);
        }

        /// <summary>
        /// Gets a virtual url that can be used to navigate to the specified <paramref name="topic" /> in a web browser.
        /// </summary>
        /// <remarks>
        /// <para>The <paramref name="topic" /> parameter must include the names of the topics above it in its hierarchy with each topic name separated by a slash, like a file path.</para>
        /// <para>The following example illustrates the appropriate value for the <paramref name="topic" /> parameter when retrieving an encoded URL to a topic named, "Form1 Constructor", which appears under a topic named, "Form1", which
        /// is located at the root of the help system named, "Namespaces": </para>
        /// <example>string topicUrl = DocSiteNavigator.GetTopicUrl("Namespaces/Form1/Form1 Constructor", true);</example>
        /// <para>The specified topic names must correspond exactly to the name attributes of topics with the same hierarchy in the DocSiteContents.xml file.</para>
        /// </remarks>
        /// <param name="topic">Name of the topic, including all topics in its hierarchy, with each topic separated by a slash.</param>
        /// <param name="urlEncode">Specifies whether each individual topic name should be url-encoded.</param>
        /// <returns>Virtual path of the specified <paramref name="topic" /> with each individual topic name, optionally, url-encoded.</returns>
        public static string GetTopicUrl(string topic, bool urlEncode) {
            return VirtualHelpPath + "?topic=" + FormatTopic(topic, urlEncode);
        }

        /// <summary>
        /// Formats the specified <paramref name="topic" /> path string by url-encoding or url-decoding each individual topic name and removing a leading and trailing / and \ character, if present.
        /// </summary>
        /// <param name="topic">Item path string to be formatted, which may include all topics in its hierarchy with each topic name separated by a slash.</param>
        /// <param name="urlEncode"><b>true</b> to have each individual topic name url-encoded; otherwise, <b>false</b> to have each individual topic name url-decoded.</param>
        /// <returns>Formatted <paramref name="topic" /> string with each individual topic name url-encoded or url-decoded, with a leading and trailing / or \ character removed.</returns>
        public static string FormatTopic(string topic, bool urlEncode) {
            if (string.IsNullOrEmpty(topic))
                return null;

            topic = topic.Replace('\\', '/');

            StringBuilder formatted = new StringBuilder();

            foreach (string name in topic.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries)) {
                formatted.Append("/");

                if (urlEncode)
                    formatted.Append(HttpUtility.UrlEncode(name));
                else
                    formatted.Append(HttpUtility.UrlDecode(name));
            }

            if (formatted.Length > 0) {
                // remove leading /
                formatted.Remove(0, 1);

                return formatted.ToString();
            } else
                return null;
        }

        /// <summary>
        /// Resolves the corresponding help file for the specified <paramref name="topic" /> name.
        /// </summary>
        /// <remarks>
        /// <para>The <paramref name="topic" /> parameter must include the names of the topics above it in its hierarchy with each topic name separated by a slash, like a file path.</para>
        /// <para>The following example illustrates the appropriate value for the <paramref name="topic" /> parameter when resolving the file for a topic named, "Form1 Constructor", which appears under a topic named, "Form1", which
        /// is located at the root of the help system named, "Namespaces": </para>
        /// <example>string topicFilePath = DocSiteNavigator.ResolveTopicHelpFile("Namespaces/Form1/Form1 Constructor");</example>
        /// <para>The specified topic names must correspond exactly to the name attributes of topics with the same hierarchy in the DocSiteContents.xml file.</para>
        /// </remarks>
        /// <param name="urlDecode">Specifies whether each individual topic name should be url-decoded.</param>
        /// <param name="topic">Item name for which its virtual file path will be returned.  The value must include all topics in its hierarchy, with each topic name separated by a slash.</param>
        /// <returns>Virtual path to the help file that corresponds to the specified <paramref name="topic" />, if found in the DocSiteContents.xml file; otherwise, null.</returns>
        public static string ResolveTopicHelpFile(string topic, bool urlDecode) {
            XPathNavigator node = GetDocSiteContentsNode(ConvertTopicToXPath(topic, urlDecode));

            if (node == null)
                return null;
            else
                // GetAttribute calls must specify an empty string for the namespaceURI argument; null does not work.
                return node.GetAttribute("file", "");
        }

        private static string ConvertTopicToXPath(string topic, bool urlDecode) {
            if (string.IsNullOrEmpty(topic))
                return "//*[@name]";	// default to root node

            // do not trim topic since leading and trailing spaces are valid
            topic = topic.Replace('\\', '/');

            StringBuilder xPath = new StringBuilder();

            foreach (string name in topic.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries))
                // do not trim name since leading and trailing spaces are valid
                xPath.AppendFormat("/*[@name=\"{0}\"]", (urlDecode) ? HttpUtility.UrlDecode(name) : name);

            if (xPath.Length > 0)
                return "/topics" + xPath.ToString();
            else
                return "//*[@name]";	// default to root node
        }

        private static XPathNavigator GetDocSiteContentsNode(string xPath) {
            XPathNavigator navigator = DocSiteContentsDocument.CreateNavigator();

            if (string.IsNullOrEmpty(xPath))
                // default to root node
                return navigator.SelectSingleNode("//*[@name]");
            else
                // allow null to be returned
                return navigator.SelectSingleNode(xPath);
        }
        #endregion
    }
}
