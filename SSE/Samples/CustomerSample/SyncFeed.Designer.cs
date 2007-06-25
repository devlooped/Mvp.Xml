namespace CustomerSample
{
	partial class SyncFeed
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

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
			System.Windows.Forms.Label linkLabel;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SyncFeed));
			System.Windows.Forms.Label descriptionLabel;
			System.Windows.Forms.Label label3;
			System.Windows.Forms.Label titleLabel;
			this.serverUrlTextBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.syncButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.formValidator1 = new CustomValidation.FormValidator();
			this.linkValidator = new CustomValidation.RequiredFieldValidator();
			this.label2 = new System.Windows.Forms.Label();
			this.descriptionTextBox = new System.Windows.Forms.TextBox();
			this.linkTextBox = new System.Windows.Forms.TextBox();
			this.titleTextBox = new System.Windows.Forms.TextBox();
			linkLabel = new System.Windows.Forms.Label();
			descriptionLabel = new System.Windows.Forms.Label();
			label3 = new System.Windows.Forms.Label();
			titleLabel = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.formValidator1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.linkValidator)).BeginInit();
			this.SuspendLayout();
			// 
			// linkLabel
			// 
			linkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			linkLabel.AutoSize = true;
			linkLabel.Location = new System.Drawing.Point(9, 284);
			linkLabel.Name = "linkLabel";
			linkLabel.Size = new System.Drawing.Size(62, 14);
			linkLabel.TabIndex = 3;
			linkLabel.Text = "Server Url:";
			// 
			// serverUrlTextBox
			// 
			this.serverUrlTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.serverUrlTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::CustomerSample.Properties.Settings.Default, "ServerUrl", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.serverUrlTextBox.Location = new System.Drawing.Point(87, 281);
			this.serverUrlTextBox.Name = "serverUrlTextBox";
			this.serverUrlTextBox.Size = new System.Drawing.Size(293, 22);
			this.serverUrlTextBox.TabIndex = 2;
			this.serverUrlTextBox.Text = global::CustomerSample.Properties.Settings.Default.ServerUrl;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.label1.BackColor = System.Drawing.SystemColors.ScrollBar;
			this.label1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(9, 246);
			this.label1.Name = "label1";
			this.label1.Padding = new System.Windows.Forms.Padding(2);
			this.label1.Size = new System.Drawing.Size(371, 18);
			this.label1.TabIndex = 7;
			this.label1.Text = "Server feed information";
			// 
			// syncButton
			// 
			this.syncButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.syncButton.AutoSize = true;
			this.syncButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.syncButton.Location = new System.Drawing.Point(218, 324);
			this.syncButton.Name = "syncButton";
			this.syncButton.Size = new System.Drawing.Size(81, 24);
			this.syncButton.TabIndex = 5;
			this.syncButton.Text = "&Synchronize";
			this.syncButton.UseVisualStyleBackColor = true;
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(305, 325);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 6;
			this.cancelButton.Text = "&Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// formValidator1
			// 
			this.formValidator1.HostingForm = this;
			// 
			// linkValidator
			// 
			this.linkValidator.ControlToValidate = this.serverUrlTextBox;
			this.linkValidator.ErrorMessage = "Server Url is required.";
			this.linkValidator.Icon = ((System.Drawing.Icon)(resources.GetObject("linkValidator.Icon")));
			this.linkValidator.InitialValue = "";
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.label2.BackColor = System.Drawing.SystemColors.ScrollBar;
			this.label2.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(9, 9);
			this.label2.Name = "label2";
			this.label2.Padding = new System.Windows.Forms.Padding(2);
			this.label2.Size = new System.Drawing.Size(371, 18);
			this.label2.TabIndex = 14;
			this.label2.Text = "Local feed information";
			// 
			// descriptionLabel
			// 
			descriptionLabel.AutoSize = true;
			descriptionLabel.Location = new System.Drawing.Point(9, 69);
			descriptionLabel.Name = "descriptionLabel";
			descriptionLabel.Size = new System.Drawing.Size(72, 14);
			descriptionLabel.TabIndex = 10;
			descriptionLabel.Text = "Description:";
			// 
			// descriptionTextBox
			// 
			this.descriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.descriptionTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::CustomerSample.Properties.Settings.Default, "FeedDescription", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.descriptionTextBox.Location = new System.Drawing.Point(87, 66);
			this.descriptionTextBox.Multiline = true;
			this.descriptionTextBox.Name = "descriptionTextBox";
			this.descriptionTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.descriptionTextBox.Size = new System.Drawing.Size(293, 127);
			this.descriptionTextBox.TabIndex = 9;
			this.descriptionTextBox.Text = global::CustomerSample.Properties.Settings.Default.FeedDescription;
			// 
			// label3
			// 
			label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			label3.AutoSize = true;
			label3.Location = new System.Drawing.Point(9, 203);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(32, 14);
			label3.TabIndex = 12;
			label3.Text = "Link:";
			// 
			// linkTextBox
			// 
			this.linkTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.linkTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::CustomerSample.Properties.Settings.Default, "FeedLink", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.linkTextBox.Location = new System.Drawing.Point(87, 200);
			this.linkTextBox.Name = "linkTextBox";
			this.linkTextBox.Size = new System.Drawing.Size(293, 22);
			this.linkTextBox.TabIndex = 11;
			this.linkTextBox.Text = global::CustomerSample.Properties.Settings.Default.FeedLink;
			// 
			// titleLabel
			// 
			titleLabel.AutoSize = true;
			titleLabel.Location = new System.Drawing.Point(9, 41);
			titleLabel.Name = "titleLabel";
			titleLabel.Size = new System.Drawing.Size(35, 14);
			titleLabel.TabIndex = 13;
			titleLabel.Text = "Title:";
			// 
			// titleTextBox
			// 
			this.titleTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.titleTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::CustomerSample.Properties.Settings.Default, "FeedTitle", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.titleTextBox.Location = new System.Drawing.Point(87, 38);
			this.titleTextBox.Name = "titleTextBox";
			this.titleTextBox.Size = new System.Drawing.Size(293, 22);
			this.titleTextBox.TabIndex = 8;
			this.titleTextBox.Text = global::CustomerSample.Properties.Settings.Default.FeedTitle;
			// 
			// SyncFeed
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(399, 360);
			this.Controls.Add(this.label2);
			this.Controls.Add(descriptionLabel);
			this.Controls.Add(this.descriptionTextBox);
			this.Controls.Add(label3);
			this.Controls.Add(this.linkTextBox);
			this.Controls.Add(titleLabel);
			this.Controls.Add(this.titleTextBox);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.syncButton);
			this.Controls.Add(this.label1);
			this.Controls.Add(linkLabel);
			this.Controls.Add(this.serverUrlTextBox);
			this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SyncFeed";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Synchronize";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SyncFeed_FormClosing);
			((System.ComponentModel.ISupportInitialize)(this.formValidator1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.linkValidator)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox serverUrlTextBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button syncButton;
		private System.Windows.Forms.Button cancelButton;
		private CustomValidation.FormValidator formValidator1;
		private CustomValidation.RequiredFieldValidator linkValidator;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox descriptionTextBox;
		private System.Windows.Forms.TextBox linkTextBox;
		private System.Windows.Forms.TextBox titleTextBox;
	}
}