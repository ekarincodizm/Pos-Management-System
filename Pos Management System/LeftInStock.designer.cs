namespace Pos_Management_System
{
    partial class LeftInStock
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
            this.button1 = new System.Windows.Forms.Button();
            this.txt_p_E = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_p_S = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.checkBoxProduct = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBox_E = new System.Windows.Forms.ComboBox();
            this.comboBox_S = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lb_msg = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dateTimePicker_S = new System.Windows.Forms.DateTimePicker();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button5 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 194);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Report";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txt_p_E
            // 
            this.txt_p_E.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txt_p_E.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txt_p_E.BackColor = System.Drawing.Color.PaleGreen;
            this.txt_p_E.Location = new System.Drawing.Point(445, 19);
            this.txt_p_E.Name = "txt_p_E";
            this.txt_p_E.Size = new System.Drawing.Size(219, 20);
            this.txt_p_E.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(100, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "จาก";
            // 
            // txt_p_S
            // 
            this.txt_p_S.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.txt_p_S.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txt_p_S.BackColor = System.Drawing.Color.PaleGreen;
            this.txt_p_S.Location = new System.Drawing.Point(139, 19);
            this.txt_p_S.Name = "txt_p_S";
            this.txt_p_S.Size = new System.Drawing.Size(219, 20);
            this.txt_p_S.TabIndex = 27;
            this.txt_p_S.TextChanged += new System.EventHandler(this.txt_p_S_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(420, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(19, 13);
            this.label3.TabIndex = 29;
            this.label3.Text = "ถึง";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(420, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(19, 13);
            this.label4.TabIndex = 30;
            this.label4.Text = "ถึง";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(100, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "จาก";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.checkBoxProduct);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txt_p_E);
            this.groupBox1.Controls.Add(this.txt_p_S);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(12, 70);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(738, 56);
            this.groupBox1.TabIndex = 32;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "สินค้า";
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button4.Location = new System.Drawing.Point(670, 18);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(50, 23);
            this.button4.TabIndex = 38;
            this.button4.Text = "ค้นหา";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button3.Location = new System.Drawing.Point(364, 16);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(50, 23);
            this.button3.TabIndex = 37;
            this.button3.Text = "ค้นหา";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // checkBoxProduct
            // 
            this.checkBoxProduct.AutoSize = true;
            this.checkBoxProduct.Location = new System.Drawing.Point(28, 24);
            this.checkBoxProduct.Name = "checkBoxProduct";
            this.checkBoxProduct.Size = new System.Drawing.Size(59, 17);
            this.checkBoxProduct.TabIndex = 30;
            this.checkBoxProduct.Text = "ทั้งหมด";
            this.checkBoxProduct.UseVisualStyleBackColor = true;
            this.checkBoxProduct.CheckedChanged += new System.EventHandler(this.checkBoxProduct_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBox_E);
            this.groupBox2.Controls.Add(this.comboBox_S);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(12, 132);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(738, 56);
            this.groupBox2.TabIndex = 33;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "หมวดสินค้า";
            // 
            // comboBox_E
            // 
            this.comboBox_E.FormattingEnabled = true;
            this.comboBox_E.Location = new System.Drawing.Point(445, 22);
            this.comboBox_E.Name = "comboBox_E";
            this.comboBox_E.Size = new System.Drawing.Size(275, 21);
            this.comboBox_E.TabIndex = 33;
            // 
            // comboBox_S
            // 
            this.comboBox_S.FormattingEnabled = true;
            this.comboBox_S.Location = new System.Drawing.Point(139, 22);
            this.comboBox_S.Name = "comboBox_S";
            this.comboBox_S.Size = new System.Drawing.Size(275, 21);
            this.comboBox_S.TabIndex = 32;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(93, 194);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 34;
            this.button2.Text = "Clear";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(376, 194);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(205, 23);
            this.progressBar1.TabIndex = 35;
            // 
            // lb_msg
            // 
            this.lb_msg.AutoSize = true;
            this.lb_msg.Location = new System.Drawing.Point(249, 201);
            this.lb_msg.Name = "lb_msg";
            this.lb_msg.Size = new System.Drawing.Size(121, 13);
            this.lb_msg.TabIndex = 36;
            this.lb_msg.Text = "โปรแกรมกำลังทำงาน . . ";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dateTimePicker_S);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(738, 52);
            this.groupBox3.TabIndex = 37;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "ณ วันนี้";
            // 
            // dateTimePicker_S
            // 
            this.dateTimePicker_S.CustomFormat = "dd/MM/yyyy";
            this.dateTimePicker_S.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker_S.Location = new System.Drawing.Point(139, 19);
            this.dateTimePicker_S.Name = "dateTimePicker_S";
            this.dateTimePicker_S.Size = new System.Drawing.Size(219, 20);
            this.dateTimePicker_S.TabIndex = 34;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 223);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(1203, 386);
            this.dataGridView1.TabIndex = 38;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(587, 194);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 39;
            this.button5.Text = "Excel";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // LeftInStock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1227, 621);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.lb_msg);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Name = "LeftInStock";
            this.Text = "LeftInStock";
            this.Load += new System.EventHandler(this.LeftInStock_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txt_p_E;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_p_S;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox checkBoxProduct;
        private System.Windows.Forms.ComboBox comboBox_E;
        private System.Windows.Forms.ComboBox comboBox_S;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lb_msg;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DateTimePicker dateTimePicker_S;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button5;
    }
}