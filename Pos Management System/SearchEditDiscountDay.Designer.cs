namespace Pos_Management_System
{
    partial class SearchEditDiscountDay
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ColumnCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCurrentPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSpecialPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDiffPercent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDiffBath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnRemark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnCode,
            this.ColumnName,
            this.ColumnUnit,
            this.ColumnCurrentPrice,
            this.ColumnSpecialPrice,
            this.ColumnDiffPercent,
            this.ColumnDiffBath,
            this.ColumnRemark,
            this.ColumnId});
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1210, 598);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView1_RowPostPaint);
            // 
            // ColumnCode
            // 
            this.ColumnCode.HeaderText = "รหัสสินค้า";
            this.ColumnCode.Name = "ColumnCode";
            this.ColumnCode.ReadOnly = true;
            // 
            // ColumnName
            // 
            this.ColumnName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.ColumnName.DefaultCellStyle = dataGridViewCellStyle1;
            this.ColumnName.HeaderText = "ชื่อสินค้า";
            this.ColumnName.Name = "ColumnName";
            this.ColumnName.ReadOnly = true;
            this.ColumnName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // ColumnUnit
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            this.ColumnUnit.DefaultCellStyle = dataGridViewCellStyle2;
            this.ColumnUnit.HeaderText = "หน่วย";
            this.ColumnUnit.Name = "ColumnUnit";
            this.ColumnUnit.ReadOnly = true;
            this.ColumnUnit.Width = 120;
            // 
            // ColumnCurrentPrice
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            this.ColumnCurrentPrice.DefaultCellStyle = dataGridViewCellStyle3;
            this.ColumnCurrentPrice.HeaderText = "ราคาปกติ";
            this.ColumnCurrentPrice.Name = "ColumnCurrentPrice";
            this.ColumnCurrentPrice.ReadOnly = true;
            this.ColumnCurrentPrice.Width = 90;
            // 
            // ColumnSpecialPrice
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.ColumnSpecialPrice.DefaultCellStyle = dataGridViewCellStyle4;
            this.ColumnSpecialPrice.HeaderText = "ราคาพิเศษ";
            this.ColumnSpecialPrice.Name = "ColumnSpecialPrice";
            this.ColumnSpecialPrice.ReadOnly = true;
            this.ColumnSpecialPrice.Width = 90;
            // 
            // ColumnDiffPercent
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            this.ColumnDiffPercent.DefaultCellStyle = dataGridViewCellStyle5;
            this.ColumnDiffPercent.HeaderText = "ส่วนต่าง (%)";
            this.ColumnDiffPercent.Name = "ColumnDiffPercent";
            this.ColumnDiffPercent.ReadOnly = true;
            // 
            // ColumnDiffBath
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            this.ColumnDiffBath.DefaultCellStyle = dataGridViewCellStyle6;
            this.ColumnDiffBath.HeaderText = "ส่วนต่าง (บาท)";
            this.ColumnDiffBath.Name = "ColumnDiffBath";
            this.ColumnDiffBath.ReadOnly = true;
            // 
            // ColumnRemark
            // 
            this.ColumnRemark.HeaderText = "หมายเหตุ";
            this.ColumnRemark.Name = "ColumnRemark";
            this.ColumnRemark.ReadOnly = true;
            this.ColumnRemark.Width = 250;
            // 
            // ColumnId
            // 
            this.ColumnId.HeaderText = "SellingId";
            this.ColumnId.Name = "ColumnId";
            this.ColumnId.Visible = false;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(1147, 616);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "ปิดหน้าต่าง";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button3.Location = new System.Drawing.Point(12, 616);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(113, 23);
            this.button3.TabIndex = 4;
            this.button3.Text = "ลบออกจากรายการ";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(131, 621);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 35;
            this.label1.Text = "label1";
            this.label1.Visible = false;
            // 
            // SearchEditDiscountDay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1234, 651);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "SearchEditDiscountDay";
            this.Text = "SearchEditDiscountDay";
            this.Load += new System.EventHandler(this.SearchEditDiscountDay_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCurrentPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSpecialPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDiffPercent;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDiffBath;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnRemark;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnId;
        private System.Windows.Forms.Label label1;
    }
}