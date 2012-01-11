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
using System.IO;

namespace DocSite {
    public sealed class DocSiteContentsSiteMapProvider : SiteMapProvider {
        #region Public Properties
        #endregion

        #region Private / Protected
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs a new instance of the <see cref="DocSiteContentsSiteMapProvider" /> class.
        /// </summary>
        public DocSiteContentsSiteMapProvider() {
        }
        #endregion

        #region Methods
        private SiteMapNode CreateSiteMapNode(string topic) {
            return new SiteMapNode(this, topic, DocSiteNavigator.GetTopicUrl(topic, true),
                GetTopicName(topic), topic);
        }

        private static string GetTopicName(string topic) {
            topic = topic.Replace('\\', '/');

            int lastSeparatorIndex = topic.LastIndexOf('/');

            if (lastSeparatorIndex == -1)
                return topic;
            else if (lastSeparatorIndex < topic.Length - 1)
                return topic.Substring(lastSeparatorIndex + 1);
            else
                return string.Empty;
        }

        private static string GetParentTopic(string topic) {
            topic = topic.Replace('\\', '/');

            int lastSeparatorIndex = topic.LastIndexOf('/');

            if (lastSeparatorIndex == -1)
                return string.Empty;
            else
                return topic.Substring(0, lastSeparatorIndex);
        }

        public override SiteMapNode FindSiteMapNode(string rawUrl) {
            Default homePage = HttpContext.Current.CurrentHandler as Default;

            if (homePage == null)
                return null;

            DocSite master = (DocSite)homePage.Master;
            string topic = master.Sidebar.TableOfContents.SelectedTopic;

            return CreateSiteMapNode(topic);
        }

        public override SiteMapNodeCollection GetChildNodes(SiteMapNode node) {
            SiteMapNodeCollection nodes = new SiteMapNodeCollection();

            foreach (string topic in DocSiteNavigator.GetSubTopics(node.Key))
                nodes.Add(CreateSiteMapNode(topic));

            return nodes;
        }

        public override SiteMapNode GetParentNode(SiteMapNode node) {
            string topic = node.Key;

            if (string.IsNullOrEmpty(topic))
                return null;

            string parent = GetParentTopic(topic);

            if (string.IsNullOrEmpty(parent))
                return null;

            return CreateSiteMapNode(parent);
        }

        protected override SiteMapNode GetRootNodeCore() {
            Default homePage = HttpContext.Current.CurrentHandler as Default;

            if (homePage == null)
                return null;

            DocSite master = (DocSite)homePage.Master;
            string topic = master.Sidebar.TableOfContents.SelectedTopic;

            if (string.IsNullOrEmpty(topic))
                return null;

            string parent;

            while ((parent = GetParentTopic(topic)).Length > 0)
                topic = parent;

            return CreateSiteMapNode(topic);
        }
        #endregion
    }
}
