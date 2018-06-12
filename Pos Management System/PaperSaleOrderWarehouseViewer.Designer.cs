namespace Pos_Management_System
{
    partial class PaperSaleOrderWarehouseViewer
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
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource2 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SaleOrderWarehouseListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.SaleOrderWarehouseListBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            this.reportViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = this.SaleOrderWarehouseListBindingSource;
            reportDataSource2.Name = "DataSet2";
            reportDataSource2.Value = this.SaleOrderWarehouseListBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource2);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Pos_Management_System.PaperOderWarehouse.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(12, 12);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(774, 523);
            this.reportViewer1.TabIndex = 0;
            // 
            // SaleOrderWarehouseListBindingSource
            // 
            this.SaleOrderWarehouseListBindingSource.DataSource = typeof(Pos_Management_System.ModelForPaper.SaleOrderWarehouseList);
            // 
            // PaperSaleOrderWarehouseViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(798, 547);
            this.Controls.Add(this.reportViewer1);
            this.Name = "PaperSaleOrderWarehouseViewer";
            this.Text = "PaperSaleOrderWarehouseViewer";
            this.Load += new System.EventHandler(this.PaperSaleOrderWarehouseViewer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SaleOrderWarehouseListBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource SaleOrderWarehouseListBindingSource;
    }
}