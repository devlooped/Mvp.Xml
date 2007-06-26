using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CustomerLibrary;
using System.Xml;
using Mvp.Xml.Synchronization;
using System.Data.SqlServerCe;

namespace CustomerSample
{
	public partial class MainForm : Form
	{
		BindingList<Customer> dataSource;

		public MainForm()
		{
			InitializeComponent();
			LoadData();
		}

		private void LoadData()
		{
			CustomerDataAccess dac = new CustomerDataAccess(
				new SqlCeProviderFactory(), Properties.Settings.Default.DB);
			dataSource = new CustomerSource(dac.GetAll());
			customerSource.DataSource = dataSource;
		}

		private void customerGrid_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
		{
			if (customerGrid.CurrentRow != null &&
				customerGrid.CurrentRow.DataBoundItem != null &&
				customerGrid.IsCurrentRowDirty)
			{
				CustomerDataAccess dac = new CustomerDataAccess(
					new SqlCeProviderFactory(), Properties.Settings.Default.DB);
				Customer c = (Customer)customerGrid.CurrentRow.DataBoundItem;
				try
				{
					if (c.Id > 0)
						dac.Update(c);
					else
						dac.Add(c);

					customerGrid.CurrentRow.ErrorText = null;
				}
				catch (Exception ex)
				{
					customerGrid.CurrentRow.ErrorText = ex.Message;
					e.Cancel = true;
				}
			}
		}

		private void exportFeedMenuItem_Click(object sender, EventArgs e)
		{
			ExportFeed exportDlg = new ExportFeed();
			if (exportDlg.ShowDialog(this) == DialogResult.OK)
			{
				XmlWriterSettings set = new XmlWriterSettings();
				set.Indent = true;

				using (XmlWriter writer = XmlWriter.Create(exportDlg.FeedFileName, set))
				{
					IXmlRepository xmlRepo = new CustomerRepository(
						new SqlCeProviderFactory(), Properties.Settings.Default.DB);
					ISyncRepository syncRepo = new DbSyncRepository(
						new SqlCeProviderFactory(), "Customer",
						Properties.Settings.Default.SyncDB);
					SyncEngine engine = new SyncEngine(xmlRepo, syncRepo);

					engine.Publish(exportDlg.FeedInformation, new RssFeedWriter(writer));
					MessageBox.Show(this, "Export completed successfully.", "Export Feed", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
		}

		private void importFeedMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog openDlg = new OpenFileDialog();
			openDlg.CheckFileExists = true;
			openDlg.RestoreDirectory = true;
			if (openDlg.ShowDialog(this) == DialogResult.OK)
			{
				XmlReaderSettings set = new XmlReaderSettings();
				set.CheckCharacters = true;
				set.IgnoreComments = true;
				set.IgnoreProcessingInstructions = true;
				set.IgnoreWhitespace = true;

				using (XmlReader reader = XmlReader.Create(openDlg.FileName, set))
				{
					IXmlRepository xmlRepo = new CustomerRepository(
						new SqlCeProviderFactory(), Properties.Settings.Default.DB);
					ISyncRepository syncRepo = new DbSyncRepository(
						new SqlCeProviderFactory(), "Customer", 
						Properties.Settings.Default.SyncDB);
					SyncEngine engine = new SyncEngine(xmlRepo, syncRepo);

					engine.Subscribe(new RssFeedReader(reader));
					MessageBox.Show(this, "Import completed successfully.", "Import Feed", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}

				LoadData();
			}
		}

		private void synchronizeMenuItem_Click(object sender, EventArgs e)
		{
			SyncFeed syncDlg = new SyncFeed();
			if (syncDlg.ShowDialog(this) == DialogResult.OK)
			{
				IXmlRepository xmlRepo = new CustomerRepository(
					new SqlCeProviderFactory(), Properties.Settings.Default.DB);
				ISyncRepository syncRepo = new DbSyncRepository(
					new SqlCeProviderFactory(), "Customer", 
					Properties.Settings.Default.SyncDB);
				SyncEngine engine = new SyncEngine(xmlRepo, syncRepo);

				HttpSync sync = new HttpSync(engine);
				IList<Item> conflicts = sync.Synchronize(syncDlg.FeedInformation, syncDlg.ServerUrl);

				MessageBox.Show(String.Format("Synchronization completed successfully. Conflicts: {0}.",
					conflicts.Count), "Synchronization", 
					MessageBoxButtons.OK, MessageBoxIcon.Information);

				LoadData();
			}
		}

		private void exitMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			Properties.Settings.Default.Save();
		}

		// According to MSDN Forums, this is the 
		// only way to customize the way items are 
		// deleted from a data-bound grid :S
		class CustomerSource : BindingList<Customer>
		{
			public CustomerSource(IEnumerable<Customer> customers)
				: base(new List<Customer>(customers))
			{
			}

			protected override void RemoveItem(int index)
			{
				Customer c = this[index];
				CustomerDataAccess dac = new CustomerDataAccess(
					new SqlCeProviderFactory(), Properties.Settings.Default.DB);
				dac.Delete(c.Id);

				base.RemoveItem(index);
			}
		}
	}
}