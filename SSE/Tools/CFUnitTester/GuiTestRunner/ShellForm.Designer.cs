namespace Microsoft.Practices.Mobile.GuiTestRunner
{
	partial class ShellForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.MainMenu mainMenu1;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.miRunSelected = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.miSelectAll = new System.Windows.Forms.MenuItem();
			this.miUnselectAll = new System.Windows.Forms.MenuItem();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabTests = new System.Windows.Forms.TabPage();
			this.treTests = new System.Windows.Forms.TreeView();
			this.tabResults = new System.Windows.Forms.TabPage();
			this.lstResults = new System.Windows.Forms.ListView();
			this.colResult = new System.Windows.Forms.ColumnHeader();
			this.colTestName = new System.Windows.Forms.ColumnHeader();
			this.colMessage = new System.Windows.Forms.ColumnHeader();
			this.colClass = new System.Windows.Forms.ColumnHeader();
			this.colModule = new System.Windows.Forms.ColumnHeader();
			this.panel1 = new System.Windows.Forms.Panel();
			this.lblFailed = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.lblPassed = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lblTests = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.tabTrace = new System.Windows.Forms.TabPage();
			this.txtTrace = new System.Windows.Forms.TextBox();
			this.tabControl1.SuspendLayout();
			this.tabTests.SuspendLayout();
			this.tabResults.SuspendLayout();
			this.panel1.SuspendLayout();
			this.tabTrace.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.Add(this.miRunSelected);
			this.mainMenu1.MenuItems.Add(this.menuItem1);
			// 
			// miRunSelected
			// 
			this.miRunSelected.Text = Properties.Resources.RunSelText;
			this.miRunSelected.Click += new System.EventHandler(this.miRunSelected_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.MenuItems.Add(this.miSelectAll);
			this.menuItem1.MenuItems.Add(this.miUnselectAll);
			this.menuItem1.Text = Properties.Resources.MenuText;
			// 
			// miSelectAll
			// 
			this.miSelectAll.Text = Properties.Resources.SelectAllText;
			this.miSelectAll.Click += new System.EventHandler(this.miSelectAll_Click);
			// 
			// miUnselectAll
			// 
			this.miUnselectAll.Text = Properties.Resources.UnSelectAllText;
			this.miUnselectAll.Click += new System.EventHandler(this.miUnselectAll_Click);
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabTests);
			this.tabControl1.Controls.Add(this.tabResults);
			this.tabControl1.Controls.Add(this.tabTrace);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(240, 268);
			this.tabControl1.TabIndex = 1;
			// 
			// tabTests
			// 
			this.tabTests.Controls.Add(this.treTests);
			this.tabTests.Location = new System.Drawing.Point(0, 0);
			this.tabTests.Name = "tabTests";
			this.tabTests.Size = new System.Drawing.Size(240, 245);
			this.tabTests.Text = Properties.Resources.TabTestText;
			// 
			// treTests
			// 
			this.treTests.CheckBoxes = true;
			this.treTests.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treTests.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
			this.treTests.Location = new System.Drawing.Point(0, 0);
			this.treTests.Name = "treTests";
			this.treTests.Size = new System.Drawing.Size(240, 245);
			this.treTests.TabIndex = 0;
			this.treTests.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treTests_AfterCheck);
			// 
			// tabResults
			// 
			this.tabResults.Controls.Add(this.lstResults);
			this.tabResults.Controls.Add(this.panel1);
			this.tabResults.Location = new System.Drawing.Point(0, 0);
			this.tabResults.Name = "tabResults";
			this.tabResults.Size = new System.Drawing.Size(240, 245);
			this.tabResults.Text = Properties.Resources.ResultsText;
			// 
			// lstResults
			// 
			this.lstResults.Columns.Add(this.colResult);
			this.lstResults.Columns.Add(this.colTestName);
			this.lstResults.Columns.Add(this.colMessage);
			this.lstResults.Columns.Add(this.colClass);
			this.lstResults.Columns.Add(this.colModule);
			this.lstResults.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstResults.FullRowSelect = true;
			this.lstResults.Location = new System.Drawing.Point(0, 22);
			this.lstResults.Name = "lstResults";
			this.lstResults.Size = new System.Drawing.Size(240, 223);
			this.lstResults.TabIndex = 0;
			this.lstResults.View = System.Windows.Forms.View.Details;
			// 
			// colResult
			// 
			this.colResult.Text = "";
			this.colResult.Width = 26;
			// 
			// colTestName
			// 
			this.colTestName.Text = Properties.Resources.TestText;
			this.colTestName.Width = 100;
			// 
			// colMessage
			// 
			this.colMessage.Text = Properties.Resources.ErrorMessageText;
			this.colMessage.Width = 110;
			// 
			// colClass
			// 
			this.colClass.Text = Properties.Resources.ClassText;
			this.colClass.Width = 100;
			// 
			// colModule
			// 
			this.colModule.Text = Properties.Resources.ClassText;
			this.colModule.Width = 100;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.lblFailed);
			this.panel1.Controls.Add(this.label4);
			this.panel1.Controls.Add(this.lblPassed);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.lblTests);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(240, 22);
			// 
			// lblFailed
			// 
			this.lblFailed.Location = new System.Drawing.Point(203, 3);
			this.lblFailed.Name = "lblFailed";
			this.lblFailed.Size = new System.Drawing.Size(35, 15);
			this.lblFailed.Text = "lblFailed";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(160, 3);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(48, 13);
			this.label4.Text = Properties.Resources.FailedText;
			// 
			// lblPassed
			// 
			this.lblPassed.Location = new System.Drawing.Point(119, 3);
			this.lblPassed.Name = "lblPassed";
			this.lblPassed.Size = new System.Drawing.Size(35, 15);
			this.lblPassed.Text = "lblPassed";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(76, 3);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 13);
			this.label2.Text = Properties.Resources.PassedText;
			// 
			// lblTests
			// 
			this.lblTests.Location = new System.Drawing.Point(35, 3);
			this.lblTests.Name = "lblTests";
			this.lblTests.Size = new System.Drawing.Size(38, 15);
			this.lblTests.Text = "lblTests";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(-2, 3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(41, 17);
			this.label1.Text = Properties.Resources.TabTestText + ":";
			// 
			// tabTrace
			// 
			this.tabTrace.Controls.Add(this.txtTrace);
			this.tabTrace.Location = new System.Drawing.Point(0, 0);
			this.tabTrace.Name = "tabTrace";
			this.tabTrace.Size = new System.Drawing.Size(232, 242);
			this.tabTrace.Text = Properties.Resources.DebugMsgsText;
			// 
			// txtTrace
			// 
			this.txtTrace.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtTrace.Location = new System.Drawing.Point(0, 0);
			this.txtTrace.Multiline = true;
			this.txtTrace.Name = "txtTrace";
			this.txtTrace.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtTrace.Size = new System.Drawing.Size(232, 242);
			this.txtTrace.TabIndex = 2;
			// 
			// ShellForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(240, 268);
			this.Controls.Add(this.tabControl1);
			this.Menu = this.mainMenu1;
			this.MinimizeBox = false;
			this.Name = "ShellForm";
			this.Text = Properties.Resources.FormTitleText;
			this.tabControl1.ResumeLayout(false);
			this.tabTests.ResumeLayout(false);
			this.tabResults.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.tabTrace.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabTests;
		private System.Windows.Forms.TreeView treTests;
		private System.Windows.Forms.TabPage tabResults;
		private System.Windows.Forms.ListView lstResults;
		private System.Windows.Forms.ColumnHeader colResult;
		private System.Windows.Forms.ColumnHeader colTestName;
		private System.Windows.Forms.ColumnHeader colMessage;
		private System.Windows.Forms.MenuItem miRunSelected;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem miSelectAll;
		private System.Windows.Forms.MenuItem miUnselectAll;
		private System.Windows.Forms.ColumnHeader colClass;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label lblFailed;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label lblPassed;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblTests;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TabPage tabTrace;
		private System.Windows.Forms.TextBox txtTrace;
		private System.Windows.Forms.ColumnHeader colModule;

	}
}

