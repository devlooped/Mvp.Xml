using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Reflection;

namespace DocSite {
    public partial class Default : System.Web.UI.Page {
        public string ContentPath {
            get {
                return ContentFrame.Attributes["src"];
            }
            set {
                if (value == null)
                    throw new ArgumentNullException("value");

                ContentFrame.Attributes["src"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e) {
            SyncSidebar();

            // Encapsulating the following script in a function doesn't seem to work in IE7; access denied
            // is thrown even from within a try..catch, so the script has been hard-coded here.
            string script =
@"var docSiteContentUrl = document.getElementById('" + ContentUrl.ClientID + @"');
try { docSiteContentUrl.value = window.frames[0].location; }
catch (e) { }  // Access denied";

            Page.ClientScript.RegisterOnSubmitStatement(typeof(Default), "updateDocSitePath", script);
        }

        protected override void OnPreRender(EventArgs e) {
            ContentFrame.Attributes.Add("onload", "ContentFrame_onload(this);");

            base.OnPreRender(e);
        }

        private void SyncSidebar() {
            string topic = null, helpFile = null;

            if (!Page.IsPostBack) {
                if (Request.QueryString["filenotfound"] != null)
                    ContentPath = DocSiteNavigator.FileNotFoundPath;
                else {
                    topic = Request.QueryString["topic"];
                    helpFile = Request.QueryString["helpfile"];

                    if (!string.IsNullOrEmpty(topic))
                        topic = DocSiteNavigator.FormatTopic(topic, false);
                }
            } else {
                string topicPath = ContentUrl.Value;

                if (!string.IsNullOrEmpty(topicPath)) {
                    Uri topicUri;

                    if (Uri.TryCreate(topicPath, UriKind.RelativeOrAbsolute, out topicUri)
                        && (!topicUri.IsAbsoluteUri || topicUri.Host.Equals(Request.Url.Host, StringComparison.OrdinalIgnoreCase))) {
                        helpFile = topicUri.AbsolutePath;

                        if (helpFile.StartsWith("/") || helpFile.StartsWith(@"\"))
                            helpFile = helpFile.Substring(1);
                    }
                }
            }

            if (string.IsNullOrEmpty(helpFile))
                helpFile = DocSiteNavigator.ResolveTopicHelpFile(topic, false) ?? DocSiteNavigator.HelpFileNotFoundPath;

            if (string.IsNullOrEmpty(topic)) {
                topic = DocSiteNavigator.ResolveHelpFileTopic(helpFile, false);

                if (topic == null)
                    helpFile = DocSiteNavigator.HelpFileNotFoundPath;
            }

            ContentPath = helpFile;

            DocSiteSidebar sidebar = ((DocSite)Page.Master).Sidebar;

            if (!sidebar.TableOfContents.SelectedTopic.Equals(topic, StringComparison.Ordinal))
                sidebar.Initialize(topic, helpFile);
        }

        protected string GetSyncTocClientCallback() {
            return ((DocSite)Master).Sidebar.TableOfContents.GetSyncTocClientCallback();
        }
    }
}
