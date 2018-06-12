namespace Pos_Management_System
{
    partial class ManageDebtorForm
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
            this.label16 = new System.Windows.Forms.Label();
            this.radioButtonCode = new System.Windows.Forms.RadioButton();
            this.label15 = new System.Windows.Forms.Label();
            this.textBoxSearchKey = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.radioButtonName = new System.Windows.Forms.RadioButton();
            this.buttonAddNew = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxCode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.textBoxTel = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxAddress = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxEmail = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxTax = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxLine = new System.Windows.Forms.TextBox();
            this.ColumnId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTotalUnvat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTotalHasVat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTotalBalance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label16
            // 
            this.label16.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label16.AutoSize = true;
            this.label16.ForeColor = System.Drawing.Color.Red;
            this.label16.Location = new System.Drawing.Point(1183, 15);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(39, 13);
            this.label16.TabIndex = 40;
            this.label16.Text = "* Enter";
            // 
            // radioButtonCode
            // 
            this.radioButtonCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonCode.AutoSize = true;
            this.radioButtonCode.Checked = true;
            this.radioButtonCode.Location = new System.Drawing.Point(877, 13);
            this.radioButtonCode.Name = "radioButtonCode";
            this.radioButtonCode.Size = new System.Drawing.Size(72, 17);
            this.radioButtonCode.TabIndex = 43;
            this.radioButtonCode.TabStop = true;
            this.radioButtonCode.Text = "รหัสลูกหนี้";
            this.radioButtonCode.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(791, 15);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(60, 13);
            this.label15.TabIndex = 42;
            this.label15.Text = "ค้นหาด้วย :";
            // 
            // textBoxSearchKey
            // 
            this.textBoxSearchKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSearchKey.Location = new System.Drawing.Point(1046, 12);
            this.textBoxSearchKey.Name = "textBoxSearchKey";
            this.textBoxSearchKey.Size = new System.Drawing.Size(132, 20);
            this.textBoxSearchKey.TabIndex = 41;
            this.textBoxSearchKey.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxSearchKey_KeyUp);
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
            this.ColumnId,
            this.ColumnNo,
            this.ColumnName,
            this.ColumnTax,
            this.ColumnTotalUnvat,
            this.ColumnTotalHasVat,
            this.ColumnTotalBalance,
            this.Column1});
            this.dataGridView1.Location = new System.Drawing.Point(12, 38);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1210, 482);
            this.dataGridView1.TabIndex = 34;
            this.dataGridView1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView1_RowPostPaint);
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // radioButtonName
            // 
            this.radioButtonName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonName.AutoSize = true;
            this.radioButtonName.Location = new System.Drawing.Point(974, 13);
            this.radioButtonName.Name = "radioButtonName";
            this.radioButtonName.Size = new System.Drawing.Size(66, 17);
            this.radioButtonName.TabIndex = 44;
            this.radioButtonName.Text = "ชื่อลูกหนี้";
            this.radioButtonName.UseVisualStyleBackColor = true;
            // 
            // buttonAddNew
            // 
            this.buttonAddNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAddNew.Location = new System.Drawing.Point(12, 616);
            this.buttonAddNew.Name = "buttonAddNew";
            this.buttonAddNew.Size = new System.Drawing.Size(75, 23);
            this.buttonAddNew.TabIndex = 37;
            this.buttonAddNew.Text = "เพิ่มใหม่";
            this.buttonAddNew.UseVisualStyleBackColor = true;
            this.buttonAddNew.Click += new System.EventHandler(this.buttonAddNew_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(1066, 616);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 39;
            this.button2.Text = "บันทึก";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(1147, 616);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 35;
            this.button1.Text = "ยกเลิก";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.textBoxLine);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.textBoxTax);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.textBoxEmail);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.textBoxAddress);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.textBoxTel);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.textBoxName);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.textBoxCode);
            this.panel1.Location = new System.Drawing.Point(12, 526);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1210, 84);
            this.panel1.TabIndex = 36;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(76, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "รหัสสมาชิก :";
            // 
            // textBoxCode
            // 
            this.textBoxCode.Location = new System.Drawing.Point(146, 14);
            this.textBoxCode.Name = "textBoxCode";
            this.textBoxCode.Size = new System.Drawing.Size(132, 20);
            this.textBoxCode.TabIndex = 22;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(86, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 26;
            this.label2.Text = "ชื่อ - สกุล :";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(146, 40);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(257, 20);
            this.textBoxName.TabIndex = 25;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(423, 17);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(31, 13);
            this.label13.TabIndex = 29;
            this.label13.Text = "โทร :";
            // 
            // textBoxTel
            // 
            this.textBoxTel.Location = new System.Drawing.Point(460, 14);
            this.textBoxTel.Name = "textBoxTel";
            this.textBoxTel.Size = new System.Drawing.Size(132, 20);
            this.textBoxTel.TabIndex = 28;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(421, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 13);
            this.label6.TabIndex = 31;
            this.label6.Text = "ที่อยู่ :";
            // 
            // textBoxAddress
            // 
            this.textBoxAddress.Location = new System.Drawing.Point(460, 40);
            this.textBoxAddress.Name = "textBoxAddress";
            this.textBoxAddress.Size = new System.Drawing.Size(378, 20);
            this.textBoxAddress.TabIndex = 30;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(631, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 33;
            this.label3.Text = "E-mail :";
            // 
            // textBoxEmail
            // 
            this.textBoxEmail.Location = new System.Drawing.Point(678, 14);
            this.textBoxEmail.Name = "textBoxEmail";
            this.textBoxEmail.Size = new System.Drawing.Size(160, 20);
            this.textBoxEmail.TabIndex = 32;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(863, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 13);
            this.label4.TabIndex = 35;
            this.label4.Text = "เลขประจำตัวประชาชน :";
            // 
            // textBoxTax
            // 
            this.textBoxTax.Location = new System.Drawing.Point(986, 14);
            this.textBoxTax.Name = "textBoxTax";
            this.textBoxTax.Size = new System.Drawing.Size(161, 20);
            this.textBoxTax.TabIndex = 34;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(935, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 13);
            this.label5.TabIndex = 37;
            this.label5.Text = "Line Id :";
            // 
            // textBoxLine
            // 
            this.textBoxLine.Location = new System.Drawing.Point(986, 40);
            this.textBoxLine.Name = "textBoxLine";
            this.textBoxLine.Size = new System.Drawing.Size(161, 20);
            this.textBoxLine.TabIndex = 36;
            // 
            // ColumnId
            // 
            this.ColumnId.HeaderText = "Id";
            this.ColumnId.Name = "ColumnId";
            this.ColumnId.ReadOnly = true;
            this.ColumnId.Visible = false;
            this.ColumnId.Width = 60;
            // 
            // ColumnNo
            // 
            this.ColumnNo.HeaderText = "รหัส";
            this.ColumnNo.Name = "ColumnNo";
            this.ColumnNo.ReadOnly = true;
            // 
            // ColumnName
            // 
            this.ColumnName.HeaderText = "ชื่อ-สกุล";
            this.ColumnName.Name = "ColumnName";
            this.ColumnName.ReadOnly = true;
            this.ColumnName.Width = 150;
            // 
            // ColumnTax
            // 
            this.ColumnTax.HeaderText = "เลขประจำตัวประชาชน";
            this.ColumnTax.Name = "ColumnTax";
            this.ColumnTax.ReadOnly = true;
            this.ColumnTax.Width = 200;
            // 
            // ColumnTotalUnvat
            // 
            this.ColumnTotalUnvat.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnTotalUnvat.HeaderText = "ที่อยู่";
            this.ColumnTotalUnvat.Name = "ColumnTotalUnvat";
            this.ColumnTotalUnvat.ReadOnly = true;
            // 
            // ColumnTotalHasVat
            // 
            this.ColumnTotalHasVat.HeaderText = "โทร";
            this.ColumnTotalHasVat.Name = "ColumnTotalHasVat";
            this.ColumnTotalHasVat.ReadOnly = true;
            this.ColumnTotalHasVat.Width = 160;
            // 
            // ColumnTotalBalance
            // 
            this.ColumnTotalBalance.HeaderText = "ไลน์";
            this.ColumnTotalBalance.Name = "ColumnTotalBalance";
            this.ColumnTotalBalance.ReadOnly = true;
            this.ColumnTotalBalance.Width = 160;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "E-mail";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // ManageDebtorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1234, 651);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.radioButtonCode);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.textBoxSearchKey);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.radioButtonName);
            this.Controls.Add(this.buttonAddNew);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.Name = "ManageDebtorForm";
            this.Text = "ManageDebtorForm";
            this.Load += new System.EventHandler(this.ManageDebtorForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.RadioButton radioButtonCode;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox textBoxSearchKey;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.RadioButton radioButtonName;
        private System.Windows.Forms.Button buttonAddNew;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBoxTel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxAddress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxEmail;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxTax;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxLine;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTax;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTotalUnvat;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTotalHasVat;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTotalBalance;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
    }
}