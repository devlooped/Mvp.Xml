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
using System.Text;
using System.ComponentModel;

namespace DocSite {
    public partial class DocSiteSidebar : System.Web.UI.UserControl {
        #region Public Properties
        [Category("Appearance"), DefaultValue(true)]
        public bool ContentsSelected {
            get {
                return (bool?)ViewState["_$ContentsSelected"] ?? true;
            }
            set {
                ViewState["_$ContentsSelected"] = value;
            }
        }

        [Browsable(false)]
        public DocSiteContents TableOfContents {
            get {
                return contents;
            }
        }

        [Browsable(false)]
        public DocSiteIndex Index {
            get {
                return index;
            }
        }
        #endregion

        #region Private / Protected
        private bool changingSelection;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs a new instance of the <see cref="DocSiteSidebar" /> class.
        /// </summary>
        public DocSiteSidebar() {
        }
        #endregion

        #region Methods
        public void Initialize(string topic, string helpFile) {
            changingSelection = true;

            contents.SelectedTopic = topic;
            index.SelectedHelpFile = (helpFile == null) ? string.Empty : helpFile.ToLowerInvariant();

            changingSelection = false;
        }
        #endregion

        #region Event Handlers
        protected override void OnLoad(EventArgs e) {
            if (Page.IsPostBack)
                ContentsSelected = !"sidebar_index_button".Equals(
                    selectedButtonHiddenField.Value, StringComparison.OrdinalIgnoreCase);

            base.OnLoad(e);
        }

        protected override void OnPreRender(EventArgs e) {
            string script = string.Format("selectDocsiteSidebarButton(document.getElementById(\"{0}\"), document.getElementById(\"{1}\"));",
                (ContentsSelected) ? "sidebar_contents_button" : "sidebar_index_button",
                (ContentsSelected) ? "docsite_toc" : "docsite_index");

            Page.ClientScript.RegisterStartupScript(typeof(DocSiteSidebar), "initSidebar", script, true);

            base.OnPreRender(e);
        }

        protected void contents_SelectedTopicChanged(object sender, EventArgs e) {
            if (!changingSelection) {
                changingSelection = true;

                ContentsSelected = true;
                DocSiteNavigator.NavigateToTopic(contents.SelectedTopic, false);
                index.SelectedHelpFile = DocSiteNavigator.ResolveTopicHelpFile(contents.SelectedTopic, false);

                changingSelection = false;
            }
        }

        protected void index_SelectedHelpFileChanged(object sender, EventArgs e) {
            if (!changingSelection) {
                changingSelection = true;

                ContentsSelected = false;
                DocSiteNavigator.NavigateToHelpFile(index.SelectedHelpFile);
                contents.SelectedTopic = DocSiteNavigator.ResolveHelpFileTopic(index.SelectedHelpFile, false);

                changingSelection = false;
            }
        }
        #endregion
    }
}