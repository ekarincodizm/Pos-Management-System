namespace Pos_Management_System
{
    partial class MemberActiveAndResignReport
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
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.CbDateNow = new System.Windows.Forms.CheckBox();
            this.DtDateNow = new System.Windows.Forms.DateTimePicker();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.textBoxYear = new System.Windows.Forms.ComboBox();
            this.label18 = new System.Windows.Forms.Label();
            this.checkBoxYear = new System.Windows.Forms.CheckBox();
            this.textBoxEndNo = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxStartNo = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.checkBoxResign = new System.Windows.Forms.CheckBox();
            this.checkBoxActive = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxMale = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxTotalFromBefore = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxTotalAddThisYear = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxTotalResign = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxFemale = new System.Windows.Forms.TextBox();
            this.textBoxTotalBalance = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
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
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column9,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7,
            this.Column8});
            this.dataGridView1.Location = new System.Drawing.Point(12, 84);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1163, 529);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView1_RowPostPaint);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Id";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Visible = false;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "หมายเลข";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "ชื่อ-สกุล";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 300;
            // 
            // Column9
            // 
            this.Column9.HeaderText = "เพศ";
            this.Column9.Name = "Column9";
            this.Column9.ReadOnly = true;
            // 
            // Column4
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Column4.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column4.HeaderText = "ยกมา";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // Column5
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Column5.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column5.HeaderText = "เพิ่มหุ้น";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            // 
            // Column6
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Column6.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column6.HeaderText = "ลาออก";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            // 
            // Column7
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column7.DefaultCellStyle = dataGridViewCellStyle5;
            this.Column7.HeaderText = "วันที่ลาออก";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.Width = 150;
            // 
            // Column8
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Column8.DefaultCellStyle = dataGridViewCellStyle6;
            this.Column8.HeaderText = "รวม";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.CbDateNow);
            this.panel1.Controls.Add(this.DtDateNow);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.textBoxYear);
            this.panel1.Controls.Add(this.label18);
            this.panel1.Controls.Add(this.checkBoxYear);
            this.panel1.Controls.Add(this.textBoxEndNo);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.textBoxStartNo);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.checkBoxResign);
            this.panel1.Controls.Add(this.checkBoxActive);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1163, 66);
            this.panel1.TabIndex = 1;
            // 
            // CbDateNow
            // 
            this.CbDateNow.AutoSize = true;
            this.CbDateNow.Location = new System.Drawing.Point(637, 8);
            this.CbDateNow.Name = "CbDateNow";
            this.CbDateNow.Size = new System.Drawing.Size(59, 17);
            this.CbDateNow.TabIndex = 40;
            this.CbDateNow.Text = "ณ วันที่";
            this.CbDateNow.UseVisualStyleBackColor = true;
            this.CbDateNow.CheckedChanged += new System.EventHandler(this.CbDateNow_CheckedChanged);
            // 
            // DtDateNow
            // 
            this.DtDateNow.CustomFormat = " dd / MM / yyyy";
            this.DtDateNow.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtDateNow.Location = new System.Drawing.Point(699, 7);
            this.DtDateNow.Name = "DtDateNow";
            this.DtDateNow.Size = new System.Drawing.Size(126, 20);
            this.DtDateNow.TabIndex = 39;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(78, 6);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 38;
            // 
            // textBoxYear
            // 
            this.textBoxYear.FormattingEnabled = true;
            this.textBoxYear.Location = new System.Drawing.Point(488, 6);
            this.textBoxYear.Name = "textBoxYear";
            this.textBoxYear.Size = new System.Drawing.Size(121, 21);
            this.textBoxYear.TabIndex = 37;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(4, 9);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(71, 13);
            this.label18.TabIndex = 37;
            this.label18.Text = "ปีงบประมาณ :";
            // 
            // checkBoxYear
            // 
            this.checkBoxYear.AutoSize = true;
            this.checkBoxYear.Checked = true;
            this.checkBoxYear.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxYear.Location = new System.Drawing.Point(368, 8);
            this.checkBoxYear.Name = "checkBoxYear";
            this.checkBoxYear.Size = new System.Drawing.Size(117, 17);
            this.checkBoxYear.TabIndex = 8;
            this.checkBoxYear.Text = "ลาออก ปีงบประมาณ";
            this.checkBoxYear.UseVisualStyleBackColor = true;
            this.checkBoxYear.CheckedChanged += new System.EventHandler(this.checkBoxYear_CheckedChanged);
            // 
            // textBoxEndNo
            // 
            this.textBoxEndNo.Location = new System.Drawing.Point(1036, 7);
            this.textBoxEndNo.Name = "textBoxEndNo";
            this.textBoxEndNo.Size = new System.Drawing.Size(77, 20);
            this.textBoxEndNo.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(1020, 10);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(10, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "-";
            // 
            // textBoxStartNo
            // 
            this.textBoxStartNo.Location = new System.Drawing.Point(937, 7);
            this.textBoxStartNo.Name = "textBoxStartNo";
            this.textBoxStartNo.Size = new System.Drawing.Size(77, 20);
            this.textBoxStartNo.TabIndex = 5;
            this.textBoxStartNo.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxStartNo_KeyUp);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(862, 10);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "เลขที่สมาชิก :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(869, 42);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(289, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "* ยอดยกมาคือ การสมัครและเพิ่มหุ้นเมื่อปีงบประมาณที่ผ่านมา";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(788, 37);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "แสดง";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // checkBoxResign
            // 
            this.checkBoxResign.AutoSize = true;
            this.checkBoxResign.Checked = true;
            this.checkBoxResign.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxResign.Location = new System.Drawing.Point(302, 8);
            this.checkBoxResign.Name = "checkBoxResign";
            this.checkBoxResign.Size = new System.Drawing.Size(56, 17);
            this.checkBoxResign.TabIndex = 1;
            this.checkBoxResign.Text = "ลาออก";
            this.checkBoxResign.UseVisualStyleBackColor = true;
            // 
            // checkBoxActive
            // 
            this.checkBoxActive.AutoSize = true;
            this.checkBoxActive.Checked = true;
            this.checkBoxActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxActive.Location = new System.Drawing.Point(236, 8);
            this.checkBoxActive.Name = "checkBoxActive";
            this.checkBoxActive.Size = new System.Drawing.Size(51, 17);
            this.checkBoxActive.TabIndex = 0;
            this.checkBoxActive.Text = "คงอยู่";
            this.checkBoxActive.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(1019, 619);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Excel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 624);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "ชาย :";
            // 
            // textBoxMale
            // 
            this.textBoxMale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxMale.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.textBoxMale.Location = new System.Drawing.Point(50, 621);
            this.textBoxMale.Name = "textBoxMale";
            this.textBoxMale.ReadOnly = true;
            this.textBoxMale.Size = new System.Drawing.Size(62, 20);
            this.textBoxMale.TabIndex = 5;
            this.textBoxMale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(127, 624);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "หญิง :";
            // 
            // textBoxTotalFromBefore
            // 
            this.textBoxTotalFromBefore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxTotalFromBefore.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.textBoxTotalFromBefore.Location = new System.Drawing.Point(315, 621);
            this.textBoxTotalFromBefore.Name = "textBoxTotalFromBefore";
            this.textBoxTotalFromBefore.ReadOnly = true;
            this.textBoxTotalFromBefore.Size = new System.Drawing.Size(100, 20);
            this.textBoxTotalFromBefore.TabIndex = 9;
            this.textBoxTotalFromBefore.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(251, 624);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "รวมยกมา :";
            // 
            // textBoxTotalAddThisYear
            // 
            this.textBoxTotalAddThisYear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxTotalAddThisYear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.textBoxTotalAddThisYear.Location = new System.Drawing.Point(511, 621);
            this.textBoxTotalAddThisYear.Name = "textBoxTotalAddThisYear";
            this.textBoxTotalAddThisYear.ReadOnly = true;
            this.textBoxTotalAddThisYear.Size = new System.Drawing.Size(100, 20);
            this.textBoxTotalAddThisYear.TabIndex = 11;
            this.textBoxTotalAddThisYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(438, 624);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "รวมเพิ่มหุ้น :";
            // 
            // textBoxTotalResign
            // 
            this.textBoxTotalResign.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxTotalResign.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.textBoxTotalResign.Location = new System.Drawing.Point(698, 621);
            this.textBoxTotalResign.Name = "textBoxTotalResign";
            this.textBoxTotalResign.ReadOnly = true;
            this.textBoxTotalResign.Size = new System.Drawing.Size(100, 20);
            this.textBoxTotalResign.TabIndex = 13;
            this.textBoxTotalResign.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(630, 624);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "รวมลาออก :";
            // 
            // textBoxFemale
            // 
            this.textBoxFemale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxFemale.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.textBoxFemale.Location = new System.Drawing.Point(167, 621);
            this.textBoxFemale.Name = "textBoxFemale";
            this.textBoxFemale.ReadOnly = true;
            this.textBoxFemale.Size = new System.Drawing.Size(62, 20);
            this.textBoxFemale.TabIndex = 14;
            this.textBoxFemale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxTotalBalance
            // 
            this.textBoxTotalBalance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxTotalBalance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.textBoxTotalBalance.Location = new System.Drawing.Point(864, 622);
            this.textBoxTotalBalance.Name = "textBoxTotalBalance";
            this.textBoxTotalBalance.ReadOnly = true;
            this.textBoxTotalBalance.Size = new System.Drawing.Size(100, 20);
            this.textBoxTotalBalance.TabIndex = 16;
            this.textBoxTotalBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(811, 625);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "รวมหุ้น :";
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(1100, 620);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 17;
            this.button2.Text = "PDF";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // MemberActiveAndResignReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1187, 654);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBoxTotalBalance);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxFemale);
            this.Controls.Add(this.textBoxTotalResign);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxTotalAddThisYear);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxTotalFromBefore);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxMale);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "MemberActiveAndResignReport";
            this.Text = "MemberActiveAndResignReport";
            this.Load += new System.EventHandler(this.MemberActiveAndResignReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBoxResign;
        private System.Windows.Forms.CheckBox checkBoxActive;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxMale;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxTotalFromBefore;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxTotalAddThisYear;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxTotalResign;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxFemale;
        private System.Windows.Forms.TextBox textBoxTotalBalance;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.TextBox textBoxEndNo;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxStartNo;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox checkBoxYear;
        private System.Windows.Forms.ComboBox textBoxYear;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.DateTimePicker DtDateNow;
        private System.Windows.Forms.CheckBox CbDateNow;
    }
}