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
	public partial class ExportFeed : Form
	{
		public ExportFeed()
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

		public string FeedFileName
		{
			get { return fileNameTextBox.Text; }
		}

		private void fileSelector_Click(object sender, EventArgs e)
		{
			SaveFileDialog dlg = new SaveFileDialog();
			dlg.RestoreDirectory = true;
			dlg.CheckPathExists = true;
			dlg.DefaultExt = ".xml";
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				fileNameTextBox.Text = dlg.FileName;
			}
		}

		private void ExportFeed_FormClosing(object sender, FormClosingEventArgs e)
		{
			Properties.Settings.Default.Save();
		}
	}
}