namespace Pos_Management_System
{
    partial class UserRoleManageForm
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
            this.label16 = new System.Windows.Forms.Label();
            this.radioButtonName = new System.Windows.Forms.RadioButton();
            this.radioButtonCode = new System.Windows.Forms.RadioButton();
            this.label15 = new System.Windows.Forms.Label();
            this.textBoxSearchKey = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxFullName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxDesc = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxUser = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.textBoxRoleId = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.textBoxRoleName = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.radioButtonActive = new System.Windows.Forms.RadioButton();
            this.radioButtonNon = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.ForeColor = System.Drawing.Color.Red;
            this.label16.Location = new System.Drawing.Point(356, 15);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(39, 13);
            this.label16.TabIndex = 39;
            this.label16.Text = "* Enter";
            // 
            // radioButtonName
            // 
            this.radioButtonName.AutoSize = true;
            this.radioButtonName.Location = new System.Drawing.Point(163, 13);
            this.radioButtonName.Name = "radioButtonName";
            this.radioButtonName.Size = new System.Drawing.Size(38, 17);
            this.radioButtonName.TabIndex = 43;
            this.radioButtonName.Text = "ชื่อ";
            this.radioButtonName.UseVisualStyleBackColor = true;
            // 
            // radioButtonCode
            // 
            this.radioButtonCode.AutoSize = true;
            this.radioButtonCode.Checked = true;
            this.radioButtonCode.Location = new System.Drawing.Point(102, 13);
            this.radioButtonCode.Name = "radioButtonCode";
            this.radioButtonCode.Size = new System.Drawing.Size(44, 17);
            this.radioButtonCode.TabIndex = 42;
            this.radioButtonCode.TabStop = true;
            this.radioButtonCode.Text = "รหัส";
            this.radioButtonCode.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(24, 15);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(60, 13);
            this.label15.TabIndex = 41;
            this.label15.Text = "ค้นหาด้วย :";
            // 
            // textBoxSearchKey
            // 
            this.textBoxSearchKey.Location = new System.Drawing.Point(218, 12);
            this.textBoxSearchKey.Name = "textBoxSearchKey";
            this.textBoxSearchKey.Size = new System.Drawing.Size(132, 20);
            this.textBoxSearchKey.TabIndex = 40;
            this.textBoxSearchKey.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxSearchKey_KeyUp);
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
            this.Column4,
            this.Column5});
            this.dataGridView1.Location = new System.Drawing.Point(12, 38);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(820, 448);
            this.dataGridView1.TabIndex = 44;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView1_RowPostPaint);
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "User";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column2.HeaderText = "ชื่อ-สกุล";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "หมายเหตุ";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 200;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "บทบาท";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "สถานะ";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.button5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.textBoxFullName);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.textBoxDesc);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.textBoxUser);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.textBoxRoleId);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.textBoxRoleName);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.radioButtonActive);
            this.panel1.Controls.Add(this.radioButtonNon);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 492);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(820, 124);
            this.panel1.TabIndex = 45;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(96, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "ชื่อ-สกุล :";
            // 
            // textBoxFullName
            // 
            this.textBoxFullName.Location = new System.Drawing.Point(150, 38);
            this.textBoxFullName.Name = "textBoxFullName";
            this.textBoxFullName.Size = new System.Drawing.Size(216, 20);
            this.textBoxFullName.TabIndex = 14;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(86, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "หมายเหตุ :";
            // 
            // textBoxDesc
            // 
            this.textBoxDesc.Location = new System.Drawing.Point(150, 90);
            this.textBoxDesc.Name = "textBoxDesc";
            this.textBoxDesc.Size = new System.Drawing.Size(361, 20);
            this.textBoxDesc.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(109, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "User :";
            // 
            // textBoxUser
            // 
            this.textBoxUser.Enabled = false;
            this.textBoxUser.Location = new System.Drawing.Point(150, 12);
            this.textBoxUser.Name = "textBoxUser";
            this.textBoxUser.Size = new System.Drawing.Size(216, 20);
            this.textBoxUser.TabIndex = 10;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(3, 96);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 9;
            this.button4.Text = "เพิ่มใหม่";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // textBoxRoleId
            // 
            this.textBoxRoleId.Location = new System.Drawing.Point(748, 3);
            this.textBoxRoleId.Name = "textBoxRoleId";
            this.textBoxRoleId.Size = new System.Drawing.Size(67, 20);
            this.textBoxRoleId.TabIndex = 8;
            this.textBoxRoleId.Visible = false;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(367, 62);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(28, 23);
            this.button3.TabIndex = 7;
            this.button3.Text = "...";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBoxRoleName
            // 
            this.textBoxRoleName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.textBoxRoleName.Location = new System.Drawing.Point(150, 64);
            this.textBoxRoleName.Name = "textBoxRoleName";
            this.textBoxRoleName.ReadOnly = true;
            this.textBoxRoleName.Size = new System.Drawing.Size(216, 20);
            this.textBoxRoleName.TabIndex = 6;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(659, 96);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "ตกลง";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(740, 96);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "ยกเลิก";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // radioButtonActive
            // 
            this.radioButtonActive.AutoSize = true;
            this.radioButtonActive.Checked = true;
            this.radioButtonActive.Location = new System.Drawing.Point(639, 39);
            this.radioButtonActive.Name = "radioButtonActive";
            this.radioButtonActive.Size = new System.Drawing.Size(77, 17);
            this.radioButtonActive.TabIndex = 3;
            this.radioButtonActive.TabStop = true;
            this.radioButtonActive.Text = "ใช้งานปกติ";
            this.radioButtonActive.UseVisualStyleBackColor = true;
            // 
            // radioButtonNon
            // 
            this.radioButtonNon.AutoSize = true;
            this.radioButtonNon.Location = new System.Drawing.Point(504, 39);
            this.radioButtonNon.Name = "radioButtonNon";
            this.radioButtonNon.Size = new System.Drawing.Size(79, 17);
            this.radioButtonNon.TabIndex = 2;
            this.radioButtonNon.Text = "ระงับใช้งาน";
            this.radioButtonNon.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(98, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "บทบาท :";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(578, 96);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 16;
            this.button5.Text = "Excel";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // UserRoleManageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(844, 628);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.radioButtonName);
            this.Controls.Add(this.radioButtonCode);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.textBoxSearchKey);
            this.Name = "UserRoleManageForm";
            this.Text = "UserRoleManageForm";
            this.Load += new System.EventHandler(this.UserRoleManageForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.RadioButton radioButtonName;
        private System.Windows.Forms.RadioButton radioButtonCode;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox textBoxSearchKey;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton radioButtonActive;
        private System.Windows.Forms.RadioButton radioButtonNon;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBoxRoleName;
        private System.Windows.Forms.TextBox textBoxRoleId;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxUser;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxDesc;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxFullName;
        private System.Windows.Forms.Button button5;
    }
}