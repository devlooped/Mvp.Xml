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
    public partial class DocSite : System.Web.UI.MasterPage {
        public DocSiteSidebar Sidebar {
            get {
                return sideBar;
            }
        }

        private string GetAssemblyCopyright() {
            Assembly assembly = Assembly.GetCallingAssembly();
            AssemblyCopyrightAttribute[] attributes = (AssemblyCopyrightAttribute[])assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);

            if (attributes == null || attributes.Length == 0)
                return null;
            else
                return attributes[0].Copyright;
        }

        private string GetAssemblyCompany() {
            Assembly assembly = Assembly.GetCallingAssembly();
            AssemblyCompanyAttribute[] attributes = (AssemblyCompanyAttribute[])assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);

            if (attributes == null || attributes.Length == 0)
                return null;
            else
                return attributes[0].Company;
        }

        protected void Page_Load(object sender, EventArgs e) {
            Company.InnerText = GetAssemblyCompany();
            Copyright.InnerText = GetAssemblyCopyright();
        }

        protected override void OnPreRender(EventArgs e) {
            string topic = sideBar.TableOfContents.SelectedTopic;

            if (string.IsNullOrEmpty(topic))
                topic = sideBar.TableOfContents.DefaultTopic;
            else {
                int topicNameIndex = topic.LastIndexOf('/');

                if (topicNameIndex > -1 && topic.Length > topicNameIndex + 1)
                    topic = topic.Substring(topicNameIndex + 1);
            }

            Page.Title += topic;

            base.OnPreRender(e);
        }

        protected string GetPersistSidebarHandleScript() {
            return
                "<script type=\"text/javascript\">" +
                    "var enablePersistSidebarHandle = " +
                    Properties.Settings.Default.EnablePersistSidebarHandle.ToString().ToLowerInvariant() +
                ";</script>";
        }
    }
}
