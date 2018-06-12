namespace Pos_Management_System
{
    partial class FormBudgetYear
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Year = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreateBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Used = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GbAdd = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.TbDes = new System.Windows.Forms.TextBox();
            this.TbbudgetYear = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.TbDes2 = new System.Windows.Forms.TextBox();
            this.TbBudgetYear2 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.GbAdd.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
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
            this.Year,
            this.Description,
            this.StartDate,
            this.EndDate,
            this.CreateBy,
            this.CreateDate,
            this.Status,
            this.Used});
            this.dataGridView1.Location = new System.Drawing.Point(12, 33);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1099, 332);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
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
            // Year
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Year.DefaultCellStyle = dataGridViewCellStyle2;
            this.Year.HeaderText = "ปีงบประมาณ";
            this.Year.Name = "Year";
            this.Year.ReadOnly = true;
            // 
            // Description
            // 
            this.Description.HeaderText = "รายละเอียด";
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            this.Description.Width = 160;
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
            // Used
            // 
            this.Used.HeaderText = "";
            this.Used.Name = "Used";
            this.Used.ReadOnly = true;
            this.Used.Width = 90;
            // 
            // GbAdd
            // 
            this.GbAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.GbAdd.Controls.Add(this.button1);
            this.GbAdd.Controls.Add(this.label4);
            this.GbAdd.Controls.Add(this.label3);
            this.GbAdd.Controls.Add(this.label2);
            this.GbAdd.Controls.Add(this.dateTimePicker2);
            this.GbAdd.Controls.Add(this.dateTimePicker1);
            this.GbAdd.Controls.Add(this.label1);
            this.GbAdd.Controls.Add(this.TbDes);
            this.GbAdd.Controls.Add(this.TbbudgetYear);
            this.GbAdd.Location = new System.Drawing.Point(12, 375);
            this.GbAdd.Name = "GbAdd";
            this.GbAdd.Size = new System.Drawing.Size(531, 241);
            this.GbAdd.TabIndex = 2;
            this.GbAdd.TabStop = false;
            this.GbAdd.Text = "เพิ่มปีงบประมาณ";
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
            this.label4.Location = new System.Drawing.Point(10, 157);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(126, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "* วันที่สิ้นสุดปีงบประมาณ :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "* วันที่เริ่มปีงบประมาณ :";
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
            // dateTimePicker2
            // 
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker2.Location = new System.Drawing.Point(153, 154);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(342, 20);
            this.dateTimePicker2.TabIndex = 8;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(153, 117);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(342, 20);
            this.dateTimePicker1.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(58, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "* ปีงบประมาณ :";
            // 
            // TbDes
            // 
            this.TbDes.Location = new System.Drawing.Point(153, 58);
            this.TbDes.Multiline = true;
            this.TbDes.Name = "TbDes";
            this.TbDes.Size = new System.Drawing.Size(342, 42);
            this.TbDes.TabIndex = 6;
            // 
            // TbbudgetYear
            // 
            this.TbbudgetYear.Location = new System.Drawing.Point(153, 19);
            this.TbbudgetYear.Name = "TbbudgetYear";
            this.TbbudgetYear.Size = new System.Drawing.Size(342, 20);
            this.TbbudgetYear.TabIndex = 0;
            this.TbbudgetYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TbbudgetYear.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TbYKeyPress);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "ใช้งาน",
            "ไม่ใช้งาน",
            "ทั้งหมด"});
            this.comboBox1.Location = new System.Drawing.Point(12, 6);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(171, 21);
            this.comboBox1.TabIndex = 3;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.TbDes2);
            this.groupBox1.Controls.Add(this.TbBudgetYear2);
            this.groupBox1.Location = new System.Drawing.Point(563, 375);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(531, 241);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "เลือกข้อปีงบประมาณ";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(420, 121);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 14;
            this.button3.Text = "Default";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(153, 121);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 13;
            this.button2.Text = "ยกเลิก";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
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
            this.label8.Location = new System.Drawing.Point(65, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "ปีงบประมาณ :";
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
            // FormBudgetYear
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1123, 624);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.GbAdd);
            this.Controls.Add(this.dataGridView1);
            this.Name = "FormBudgetYear";
            this.Text = "จัดการปีงบประมาณ";
            this.Load += new System.EventHandler(this.FormBudgetYear_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.GbAdd.ResumeLayout(false);
            this.GbAdd.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.GroupBox GbAdd;
        private System.Windows.Forms.TextBox TbbudgetYear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.TextBox TbDes;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox TbDes2;
        private System.Windows.Forms.TextBox TbBudgetYear2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Year;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn EndDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreateBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreateDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn Used;
    }
}