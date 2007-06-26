using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Mvp.Xml.Synchronization;

namespace CustomerSample
{
	public partial class SyncFeed : Form
	{
		public SyncFeed()
		{
			InitializeComponent();
		}

		public Feed FeedInformation
		{
			get
			{
				return new Feed(titleTextBox.Text, linkTextBox.Text, descriptionTextBox.Text);
			}
		}
		
		public string ServerUrl
		{
			get { return serverUrlTextBox.Text; }
		}

		private void SyncFeed_FormClosing(object sender, FormClosingEventArgs e)
		{
			Properties.Settings.Default.Save();
		}
	}
}