namespace Pos_Management_System
{
    partial class FormMonthShare
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.TbDes2 = new System.Windows.Forms.TextBox();
            this.TbBudgetYear2 = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TbDes = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.GbAdd = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.CbAgeM = new System.Windows.Forms.ComboBox();
            this.CbD2 = new System.Windows.Forms.ComboBox();
            this.CbM2 = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.CbD = new System.Windows.Forms.ComboBox();
            this.CbM = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AgeMonth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreateBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            this.GbAdd.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.TbDes2);
            this.groupBox1.Controls.Add(this.TbBudgetYear2);
            this.groupBox1.Location = new System.Drawing.Point(563, 376);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(531, 241);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "เลือกข้อปีงบประมาณ";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(420, 116);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 13;
            this.button2.Text = "ยกเลิก";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(69, 61);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "รายละเอียด :";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(79, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "อายุเดือน :";
            // 
            // TbDes2
            // 
            this.TbDes2.Enabled = false;
            this.TbDes2.Location = new System.Drawing.Point(153, 58);
            this.TbDes2.Multiline = true;
            this.TbDes2.Name = "TbDes2";
            this.TbDes2.Size = new System.Drawing.Size(342, 42);
            this.TbDes2.TabIndex = 6;
            // 
            // TbBudgetYear2
            // 
            this.TbBudgetYear2.Enabled = false;
            this.TbBudgetYear2.Location = new System.Drawing.Point(153, 19);
            this.TbBudgetYear2.Name = "TbBudgetYear2";
            this.TbBudgetYear2.Size = new System.Drawing.Size(342, 20);
            this.TbBudgetYear2.TabIndex = 0;
            this.TbBudgetYear2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "ใช้งาน",
            "ไม่ใช้งาน",
            "ทั้งหมด"});
            this.comboBox1.Location = new System.Drawing.Point(12, 7);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(171, 21);
            this.comboBox1.TabIndex = 7;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(72, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "* อายุเดือน :";
            // 
            // TbDes
            // 
            this.TbDes.Location = new System.Drawing.Point(153, 58);
            this.TbDes.Multiline = true;
            this.TbDes.Name = "TbDes";
            this.TbDes.Size = new System.Drawing.Size(342, 42);
            this.TbDes.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(420, 212);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "เพิ่ม";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(68, 157);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "* วันที่สิ้นสุด :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(77, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "* วันที่เริ่ม :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(69, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "รายละเอียด :";
            // 
            // GbAdd
            // 
            this.GbAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.GbAdd.Controls.Add(this.label11);
            this.GbAdd.Controls.Add(this.CbAgeM);
            this.GbAdd.Controls.Add(this.CbD2);
            this.GbAdd.Controls.Add(this.CbM2);
            this.GbAdd.Controls.Add(this.label9);
            this.GbAdd.Controls.Add(this.label10);
            this.GbAdd.Controls.Add(this.CbD);
            this.GbAdd.Controls.Add(this.CbM);
            this.GbAdd.Controls.Add(this.label6);
            this.GbAdd.Controls.Add(this.label5);
            this.GbAdd.Controls.Add(this.button1);
            this.GbAdd.Controls.Add(this.label4);
            this.GbAdd.Controls.Add(this.label3);
            this.GbAdd.Controls.Add(this.label2);
            this.GbAdd.Controls.Add(this.label1);
            this.GbAdd.Controls.Add(this.TbDes);
            this.GbAdd.Location = new System.Drawing.Point(12, 376);
            this.GbAdd.Name = "GbAdd";
            this.GbAdd.Size = new System.Drawing.Size(531, 241);
            this.GbAdd.TabIndex = 6;
            this.GbAdd.TabStop = false;
            this.GbAdd.Text = "เพิ่มปีงบประมาณ";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.Red;
            this.label11.Location = new System.Drawing.Point(259, 22);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(33, 13);
            this.label11.TabIndex = 29;
            this.label11.Text = "เดือน";
            // 
            // CbAgeM
            // 
            this.CbAgeM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CbAgeM.FormattingEnabled = true;
            this.CbAgeM.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12"});
            this.CbAgeM.Location = new System.Drawing.Point(153, 19);
            this.CbAgeM.Name = "CbAgeM";
            this.CbAgeM.Size = new System.Drawing.Size(100, 21);
            this.CbAgeM.TabIndex = 28;
            // 
            // CbD2
            // 
            this.CbD2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CbD2.FormattingEnabled = true;
            this.CbD2.Items.AddRange(new object[] {
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31"});
            this.CbD2.Location = new System.Drawing.Point(340, 154);
            this.CbD2.Name = "CbD2";
            this.CbD2.Size = new System.Drawing.Size(100, 21);
            this.CbD2.TabIndex = 27;
            // 
            // CbM2
            // 
            this.CbM2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CbM2.FormattingEnabled = true;
            this.CbM2.Items.AddRange(new object[] {
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12"});
            this.CbM2.Location = new System.Drawing.Point(153, 155);
            this.CbM2.Name = "CbM2";
            this.CbM2.Size = new System.Drawing.Size(100, 21);
            this.CbM2.TabIndex = 26;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(446, 159);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(21, 13);
            this.label9.TabIndex = 25;
            this.label9.Text = "วัน";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.Red;
            this.label10.Location = new System.Drawing.Point(259, 159);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(33, 13);
            this.label10.TabIndex = 24;
            this.label10.Text = "เดือน";
            // 
            // CbD
            // 
            this.CbD.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CbD.FormattingEnabled = true;
            this.CbD.Items.AddRange(new object[] {
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31"});
            this.CbD.Location = new System.Drawing.Point(340, 116);
            this.CbD.Name = "CbD";
            this.CbD.Size = new System.Drawing.Size(100, 21);
            this.CbD.TabIndex = 23;
            // 
            // CbM
            // 
            this.CbM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CbM.FormattingEnabled = true;
            this.CbM.Items.AddRange(new object[] {
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12"});
            this.CbM.Location = new System.Drawing.Point(153, 117);
            this.CbM.Name = "CbM";
            this.CbM.Size = new System.Drawing.Size(100, 21);
            this.CbM.TabIndex = 22;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(446, 121);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(21, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "วัน";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(259, 121);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "เดือน";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Azure;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.AgeMonth,
            this.Description,
            this.StartDate,
            this.EndDate,
            this.CreateBy,
            this.CreateDate,
            this.Status});
            this.dataGridView1.Location = new System.Drawing.Point(12, 34);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1099, 332);
            this.dataGridView1.TabIndex = 5;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgClick);
            this.dataGridView1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.DataGrid1RowPostPaint);
            // 
            // Id
            // 
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Visible = false;
            this.Id.Width = 60;
            // 
            // AgeMonth
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.AgeMonth.DefaultCellStyle = dataGridViewCellStyle2;
            this.AgeMonth.HeaderText = "อายุเดือน";
            this.AgeMonth.Name = "AgeMonth";
            this.AgeMonth.ReadOnly = true;
            // 
            // Description
            // 
            this.Description.HeaderText = "รายละเอียด";
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            this.Description.Width = 240;
            // 
            // StartDate
            // 
            this.StartDate.HeaderText = "วันที่เริ่ม";
            this.StartDate.Name = "StartDate";
            this.StartDate.ReadOnly = true;
            this.StartDate.Width = 150;
            // 
            // EndDate
            // 
            this.EndDate.HeaderText = "วันที่สิ้นสุด";
            this.EndDate.Name = "EndDate";
            this.EndDate.ReadOnly = true;
            this.EndDate.Width = 150;
            // 
            // CreateBy
            // 
            this.CreateBy.HeaderText = "ผุ้สร้าง";
            this.CreateBy.Name = "CreateBy";
            this.CreateBy.ReadOnly = true;
            // 
            // CreateDate
            // 
            this.CreateDate.HeaderText = "วันที่สร้าง";
            this.CreateDate.Name = "CreateDate";
            this.CreateDate.ReadOnly = true;
            this.CreateDate.Width = 160;
            // 
            // Status
            // 
            this.Status.HeaderText = "สถานะ";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.Width = 140;
            // 
            // FormMonthShare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1123, 624);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.GbAdd);
            this.Controls.Add(this.dataGridView1);
            this.Name = "FormMonthShare";
            this.Text = "จัดการหุ้นไม่ครบปี";
            this.Load += new System.EventHandler(this.FormMonthShare_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.GbAdd.ResumeLayout(false);
            this.GbAdd.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox TbDes2;
        private System.Windows.Forms.TextBox TbBudgetYear2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TbDes;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox GbAdd;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn AgeMonth;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn EndDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreateBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreateDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox CbAgeM;
        private System.Windows.Forms.ComboBox CbD2;
        private System.Windows.Forms.ComboBox CbM2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox CbD;
        private System.Windows.Forms.ComboBox CbM;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
    }
}