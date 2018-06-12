namespace Pos_Management_System
{
    partial class PaperCNWasteViewer
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
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource4 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource5 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource6 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.BranchValueBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.WasteDocValueBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.WasteDocDetailsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.BranchValueBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WasteDocValueBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WasteDocDetailsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            this.reportViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            reportDataSource4.Name = "DataSet1";
            reportDataSource4.Value = this.BranchValueBindingSource;
            reportDataSource5.Name = "DataSet2";
            reportDataSource5.Value = this.WasteDocValueBindingSource;
            reportDataSource6.Name = "DataSet3";
            reportDataSource6.Value = this.WasteDocDetailsBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource4);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource5);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource6);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Pos_Management_System.PaperCNWaste.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(10, 12);
            this.reportViewer1.Margin = new System.Windows.Forms.Padding(0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(788, 467);
            this.reportViewer1.TabIndex = 1;
            // 
            // BranchValueBindingSource
            // 
            this.BranchValueBindingSource.DataSource = typeof(Pos_Management_System.ModelForPaper.BranchValue);
            // 
            // WasteDocValueBindingSource
            // 
            this.WasteDocValueBindingSource.DataSource = typeof(Pos_Management_System.ModelForPaper.WasteDocValue);
            // 
            // WasteDocDetailsBindingSource
            // 
            this.WasteDocDetailsBindingSource.DataSource = typeof(Pos_Management_System.ModelForPaper.WasteDocDetails);
            // 
            // PaperCNWasteViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(809, 491);
            this.Controls.Add(this.reportViewer1);
            this.Name = "PaperCNWasteViewer";
            this.Text = "PaperCNWasteViewer";
            this.Load += new System.EventHandler(this.PaperCNWasteViewer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.BranchValueBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WasteDocValueBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WasteDocDetailsBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource BranchValueBindingSource;
        private System.Windows.Forms.BindingSource WasteDocValueBindingSource;
        private System.Windows.Forms.BindingSource WasteDocDetailsBindingSource;
    }
}