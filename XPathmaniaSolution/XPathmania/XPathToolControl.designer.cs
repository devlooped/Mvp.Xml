namespace XmlMvp.XPathmania
{
    partial class XPathToolControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;




        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.xpathTextBox = new System.Windows.Forms.TextBox();
            this.XPathQueryButton = new System.Windows.Forms.Button();
            this.queryTabControl = new System.Windows.Forms.TabControl();
            this.ResultsTabPage = new System.Windows.Forms.TabPage();
            this.resultsGridView = new System.Windows.Forms.DataGridView();
            this.Match = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Line = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NamespaceTabPage = new System.Windows.Forms.TabPage();
            this.NamespaceGridView = new System.Windows.Forms.DataGridView();
            this.PrefixColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NamespaceColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ErrorTabPage = new System.Windows.Forms.TabPage();
            this.errorListGridView = new System.Windows.Forms.DataGridView();
            this.imageDataGridViewImageColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.sequenceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.errorInfoLineBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.errorInfoLineBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.queryTabControl.SuspendLayout();
            this.ResultsTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resultsGridView)).BeginInit();
            this.NamespaceTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NamespaceGridView)).BeginInit();
            this.ErrorTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorListGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorInfoLineBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorInfoLineBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // xpathTextBox
            // 
            this.xpathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.xpathTextBox.Location = new System.Drawing.Point(3, 3);
            this.xpathTextBox.Name = "xpathTextBox";
            this.xpathTextBox.Size = new System.Drawing.Size(700, 20);
            this.xpathTextBox.TabIndex = 0;
            // 
            // XPathQueryButton
            // 
            this.XPathQueryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.XPathQueryButton.AutoSize = true;
            this.XPathQueryButton.Location = new System.Drawing.Point(709, 1);
            this.XPathQueryButton.Name = "XPathQueryButton";
            this.XPathQueryButton.Size = new System.Drawing.Size(75, 23);
            this.XPathQueryButton.TabIndex = 1;
            this.XPathQueryButton.Text = "Query";
            this.XPathQueryButton.UseVisualStyleBackColor = true;
            this.XPathQueryButton.Click += new System.EventHandler(this.XPathQueryButton_Click);
            // 
            // queryTabControl
            // 
            this.queryTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.queryTabControl.Controls.Add(this.ResultsTabPage);
            this.queryTabControl.Controls.Add(this.NamespaceTabPage);
            this.queryTabControl.Controls.Add(this.ErrorTabPage);
            this.queryTabControl.Location = new System.Drawing.Point(3, 29);
            this.queryTabControl.Name = "queryTabControl";
            this.queryTabControl.SelectedIndex = 0;
            this.queryTabControl.Size = new System.Drawing.Size(781, 146);
            this.queryTabControl.TabIndex = 2;
            // 
            // ResultsTabPage
            // 
            this.ResultsTabPage.Controls.Add(this.resultsGridView);
            this.ResultsTabPage.Location = new System.Drawing.Point(4, 22);
            this.ResultsTabPage.Name = "ResultsTabPage";
            this.ResultsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.ResultsTabPage.Size = new System.Drawing.Size(773, 120);
            this.ResultsTabPage.TabIndex = 0;
            this.ResultsTabPage.Text = "Results";
            this.ResultsTabPage.UseVisualStyleBackColor = true;
            // 
            // resultsGridView
            // 
            this.resultsGridView.AllowUserToAddRows = false;
            this.resultsGridView.AllowUserToDeleteRows = false;
            this.resultsGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.resultsGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.resultsGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.resultsGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.resultsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.resultsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Match,
            this.Line,
            this.Column});
            this.resultsGridView.GridColor = System.Drawing.SystemColors.Info;
            this.resultsGridView.Location = new System.Drawing.Point(3, 0);
            this.resultsGridView.MultiSelect = false;
            this.resultsGridView.Name = "resultsGridView";
            this.resultsGridView.ReadOnly = true;
            this.resultsGridView.RowHeadersVisible = false;
            this.resultsGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.resultsGridView.Size = new System.Drawing.Size(770, 117);
            this.resultsGridView.TabIndex = 0;
            this.resultsGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.resultsGridView_KeyDown);
            this.resultsGridView.DoubleClick += new System.EventHandler(this.resultsGridView_DoubleClick);
            // 
            // Match
            // 
            this.Match.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Match.DataPropertyName = "Match";
            this.Match.FillWeight = 111.9289F;
            this.Match.HeaderText = "Match";
            this.Match.Name = "Match";
            this.Match.ReadOnly = true;
            // 
            // Line
            // 
            this.Line.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Line.DataPropertyName = "Line";
            this.Line.FillWeight = 111.9289F;
            this.Line.HeaderText = "Line";
            this.Line.MinimumWidth = 10;
            this.Line.Name = "Line";
            this.Line.ReadOnly = true;
            this.Line.Width = 75;
            // 
            // Column
            // 
            this.Column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column.DataPropertyName = "Column";
            this.Column.FillWeight = 76.14214F;
            this.Column.HeaderText = "Column";
            this.Column.MinimumWidth = 10;
            this.Column.Name = "Column";
            this.Column.ReadOnly = true;
            this.Column.Width = 75;
            // 
            // NamespaceTabPage
            // 
            this.NamespaceTabPage.Controls.Add(this.NamespaceGridView);
            this.NamespaceTabPage.Location = new System.Drawing.Point(4, 22);
            this.NamespaceTabPage.Name = "NamespaceTabPage";
            this.NamespaceTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.NamespaceTabPage.Size = new System.Drawing.Size(773, 120);
            this.NamespaceTabPage.TabIndex = 2;
            this.NamespaceTabPage.Text = "Namespace Table";
            this.NamespaceTabPage.UseVisualStyleBackColor = true;
            // 
            // NamespaceGridView
            // 
            this.NamespaceGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.NamespaceGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.NamespaceGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedHeaders;
            this.NamespaceGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.NamespaceGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PrefixColumn,
            this.NamespaceColumn});
            this.NamespaceGridView.Location = new System.Drawing.Point(3, 3);
            this.NamespaceGridView.Name = "NamespaceGridView";
            this.NamespaceGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.NamespaceGridView.Size = new System.Drawing.Size(767, 114);
            this.NamespaceGridView.TabIndex = 0;
            // 
            // PrefixColumn
            // 
            this.PrefixColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.NullValue = null;
            this.PrefixColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.PrefixColumn.HeaderText = "Prefix";
            this.PrefixColumn.Name = "PrefixColumn";
            this.PrefixColumn.Width = 58;
            // 
            // NamespaceColumn
            // 
            this.NamespaceColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.NamespaceColumn.HeaderText = "Namespace";
            this.NamespaceColumn.Name = "NamespaceColumn";
            // 
            // ErrorTabPage
            // 
            this.ErrorTabPage.Controls.Add(this.errorListGridView);
            this.ErrorTabPage.Location = new System.Drawing.Point(4, 22);
            this.ErrorTabPage.Name = "ErrorTabPage";
            this.ErrorTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.ErrorTabPage.Size = new System.Drawing.Size(773, 120);
            this.ErrorTabPage.TabIndex = 1;
            this.ErrorTabPage.Text = "Error List";
            this.ErrorTabPage.UseVisualStyleBackColor = true;
            // 
            // errorListGridView
            // 
            this.errorListGridView.AllowUserToAddRows = false;
            this.errorListGridView.AllowUserToDeleteRows = false;
            this.errorListGridView.AllowUserToResizeColumns = false;
            this.errorListGridView.AllowUserToResizeRows = false;
            this.errorListGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.errorListGridView.AutoGenerateColumns = false;
            this.errorListGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.errorListGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.errorListGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.errorListGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.errorListGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.imageDataGridViewImageColumn,
            this.sequenceDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn});
            this.errorListGridView.DataSource = this.errorInfoLineBindingSource1;
            this.errorListGridView.GridColor = System.Drawing.SystemColors.Info;
            this.errorListGridView.Location = new System.Drawing.Point(1, 2);
            this.errorListGridView.MultiSelect = false;
            this.errorListGridView.Name = "errorListGridView";
            this.errorListGridView.ReadOnly = true;
            this.errorListGridView.RowHeadersVisible = false;
            this.errorListGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.errorListGridView.Size = new System.Drawing.Size(770, 117);
            this.errorListGridView.TabIndex = 1;
            // 
            // imageDataGridViewImageColumn
            // 
            this.imageDataGridViewImageColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.imageDataGridViewImageColumn.DataPropertyName = "Image";
            this.imageDataGridViewImageColumn.HeaderText = "";
            this.imageDataGridViewImageColumn.Name = "imageDataGridViewImageColumn";
            this.imageDataGridViewImageColumn.ReadOnly = true;
            this.imageDataGridViewImageColumn.Width = 5;
            // 
            // sequenceDataGridViewTextBoxColumn
            // 
            this.sequenceDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.sequenceDataGridViewTextBoxColumn.DataPropertyName = "Sequence";
            this.sequenceDataGridViewTextBoxColumn.HeaderText = "Seq";
            this.sequenceDataGridViewTextBoxColumn.Name = "sequenceDataGridViewTextBoxColumn";
            this.sequenceDataGridViewTextBoxColumn.ReadOnly = true;
            this.sequenceDataGridViewTextBoxColumn.Width = 51;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // errorInfoLineBindingSource1
            // 
            this.errorInfoLineBindingSource1.DataSource = typeof(XmlMvp.XPathmania.Internal.ErrorInfoLine);
            // 
            // errorInfoLineBindingSource
            // 
            this.errorInfoLineBindingSource.DataSource = typeof(XmlMvp.XPathmania.Internal.ErrorInfoLine);
            // 
            // XPathToolControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Controls.Add(this.queryTabControl);
            this.Controls.Add(this.XPathQueryButton);
            this.Controls.Add(this.xpathTextBox);
            this.Name = "XPathToolControl";
            this.Size = new System.Drawing.Size(800, 200);
            this.queryTabControl.ResumeLayout(false);
            this.ResultsTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.resultsGridView)).EndInit();
            this.NamespaceTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NamespaceGridView)).EndInit();
            this.ErrorTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorListGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorInfoLineBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorInfoLineBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.TextBox xpathTextBox;
        private System.Windows.Forms.Button XPathQueryButton;
        private System.Windows.Forms.TabControl queryTabControl;
        private System.Windows.Forms.TabPage ResultsTabPage;
        private System.Windows.Forms.TabPage ErrorTabPage;
        private System.Windows.Forms.TabPage NamespaceTabPage;
        private System.Windows.Forms.DataGridView NamespaceGridView;
        private System.Windows.Forms.DataGridView resultsGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Match;
        private System.Windows.Forms.DataGridViewTextBoxColumn Line;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn PrefixColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn NamespaceColumn;
        private System.Windows.Forms.DataGridView errorListGridView;
        private System.Windows.Forms.BindingSource errorInfoLineBindingSource;
        private System.Windows.Forms.BindingSource errorInfoLineBindingSource1;
        private System.Windows.Forms.DataGridViewImageColumn imageDataGridViewImageColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sequenceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
    }
}
