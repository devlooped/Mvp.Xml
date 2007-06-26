namespace CustomerSample
{
	partial class ExportFeed
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
			System.Windows.Forms.Label descriptionLabel;
			System.Windows.Forms.Label linkLabel;
			System.Windows.Forms.Label titleLabel;
			System.Windows.Forms.Label label2;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportFeed));
			this.linkTextBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.exportButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.formValidator1 = new CustomValidation.FormValidator();
			this.titleValidator = new CustomValidation.RequiredFieldValidator();
			this.titleTextBox = new System.Windows.Forms.TextBox();
			this.descriptionValidator = new CustomValidation.RequiredFieldValidator();
			this.descriptionTextBox = new System.Windows.Forms.TextBox();
			this.linkValidator = new CustomValidation.RequiredFieldValidator();
			this.fileNameTextBox = new System.Windows.Forms.TextBox();
			this.fileSelector = new System.Windows.Forms.Button();
			descriptionLabel = new System.Windows.Forms.Label();
			linkLabel = new System.Windows.Forms.Label();
			titleLabel = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.formValidator1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.titleValidator)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.descriptionValidator)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.linkValidator)).BeginInit();
			this.SuspendLayout();
			// 
			// descriptionLabel
			// 
			descriptionLabel.AutoSize = true;
			descriptionLabel.Location = new System.Drawing.Point(9, 69);
			descriptionLabel.Name = "descriptionLabel";
			descriptionLabel.Size = new System.Drawing.Size(72, 14);
			descriptionLabel.TabIndex = 1;
			descriptionLabel.Text = "Description:";
			// 
			// linkLabel
			// 
			linkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			linkLabel.AutoSize = true;
			linkLabel.Location = new System.Drawing.Point(9, 200);
			linkLabel.Name = "linkLabel";
			linkLabel.Size = new System.Drawing.Size(32, 14);
			linkLabel.TabIndex = 3;
			linkLabel.Text = "Link:";
			// 
			// titleLabel
			// 
			titleLabel.AutoSize = true;
			titleLabel.Location = new System.Drawing.Point(9, 41);
			titleLabel.Name = "titleLabel";
			titleLabel.Size = new System.Drawing.Size(35, 14);
			titleLabel.TabIndex = 5;
			titleLabel.Text = "Title:";
			// 
			// label2
			// 
			label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(9, 231);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(62, 14);
			label2.TabIndex = 10;
			label2.Text = "Filename:";
			// 
			// linkTextBox
			// 
			this.linkTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.linkTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::CustomerSample.Properties.Settings.Default, "FeedLink", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.linkTextBox.Location = new System.Drawing.Point(87, 197);
			this.linkTextBox.Name = "linkTextBox";
			this.linkTextBox.Size = new System.Drawing.Size(293, 22);
			this.linkTextBox.TabIndex = 2;
			this.linkTextBox.Text = global::CustomerSample.Properties.Settings.Default.FeedLink;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.label1.BackColor = System.Drawing.SystemColors.ScrollBar;
			this.label1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(9, 9);
			this.label1.Name = "label1";
			this.label1.Padding = new System.Windows.Forms.Padding(2);
			this.label1.Size = new System.Drawing.Size(371, 18);
			this.label1.TabIndex = 7;
			this.label1.Text = "Feed information";
			// 
			// exportButton
			// 
			this.exportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.exportButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.exportButton.Location = new System.Drawing.Point(224, 270);
			this.exportButton.Name = "exportButton";
			this.exportButton.Size = new System.Drawing.Size(75, 23);
			this.exportButton.TabIndex = 5;
			this.exportButton.Text = "&Export";
			this.exportButton.UseVisualStyleBackColor = true;
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(305, 270);
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
			// titleValidator
			// 
			this.titleValidator.ControlToValidate = this.titleTextBox;
			this.titleValidator.ErrorMessage = "Title is required.";
			this.titleValidator.Icon = ((System.Drawing.Icon)(resources.GetObject("titleValidator.Icon")));
			// 
			// titleTextBox
			// 
			this.titleTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.titleTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::CustomerSample.Properties.Settings.Default, "FeedTitle", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.titleTextBox.Location = new System.Drawing.Point(87, 38);
			this.titleTextBox.Name = "titleTextBox";
			this.titleTextBox.Size = new System.Drawing.Size(293, 22);
			this.titleTextBox.TabIndex = 0;
			this.titleTextBox.Text = global::CustomerSample.Properties.Settings.Default.FeedTitle;
			// 
			// descriptionValidator
			// 
			this.descriptionValidator.ControlToValidate = this.descriptionTextBox;
			this.descriptionValidator.ErrorMessage = "Description is required.";
			this.descriptionValidator.Icon = ((System.Drawing.Icon)(resources.GetObject("descriptionValidator.Icon")));
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
			this.descriptionTextBox.Size = new System.Drawing.Size(293, 124);
			this.descriptionTextBox.TabIndex = 1;
			this.descriptionTextBox.Text = global::CustomerSample.Properties.Settings.Default.FeedDescription;
			// 
			// linkValidator
			// 
			this.linkValidator.ControlToValidate = this.linkTextBox;
			this.linkValidator.ErrorMessage = "Link is required.";
			this.linkValidator.Icon = ((System.Drawing.Icon)(resources.GetObject("linkValidator.Icon")));
			this.linkValidator.InitialValue = "";
			// 
			// fileNameTextBox
			// 
			this.fileNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.fileNameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::CustomerSample.Properties.Settings.Default, "FeedFileName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.fileNameTextBox.Location = new System.Drawing.Point(87, 228);
			this.fileNameTextBox.Name = "fileNameTextBox";
			this.fileNameTextBox.Size = new System.Drawing.Size(263, 22);
			this.fileNameTextBox.TabIndex = 3;
			this.fileNameTextBox.Text = global::CustomerSample.Properties.Settings.Default.FeedFileName;
			// 
			// fileSelector
			// 
			this.fileSelector.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.fileSelector.Location = new System.Drawing.Point(353, 228);
			this.fileSelector.Margin = new System.Windows.Forms.Padding(0);
			this.fileSelector.Name = "fileSelector";
			this.fileSelector.Size = new System.Drawing.Size(26, 22);
			this.fileSelector.TabIndex = 4;
			this.fileSelector.Text = "...";
			this.fileSelector.UseVisualStyleBackColor = true;
			this.fileSelector.Click += new System.EventHandler(this.fileSelector_Click);
			// 
			// ExportFeed
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(399, 305);
			this.Controls.Add(this.fileSelector);
			this.Controls.Add(label2);
			this.Controls.Add(this.fileNameTextBox);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.exportButton);
			this.Controls.Add(this.label1);
			this.Controls.Add(descriptionLabel);
			this.Controls.Add(this.descriptionTextBox);
			this.Controls.Add(linkLabel);
			this.Controls.Add(this.linkTextBox);
			this.Controls.Add(titleLabel);
			this.Controls.Add(this.titleTextBox);
			this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ExportFeed";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Export Feed";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExportFeed_FormClosing);
			((System.ComponentModel.ISupportInitialize)(this.formValidator1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.titleValidator)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.descriptionValidator)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.linkValidator)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox descriptionTextBox;
		private System.Windows.Forms.TextBox linkTextBox;
		private System.Windows.Forms.TextBox titleTextBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button exportButton;
		private System.Windows.Forms.Button cancelButton;
		private CustomValidation.FormValidator formValidator1;
		private CustomValidation.RequiredFieldValidator titleValidator;
		private CustomValidation.RequiredFieldValidator descriptionValidator;
		private CustomValidation.RequiredFieldValidator linkValidator;
		private System.Windows.Forms.Button fileSelector;
		private System.Windows.Forms.TextBox fileNameTextBox;
	}
}