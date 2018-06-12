namespace Pos_Management_System
{
    partial class CheckFrontStockForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxScanner = new System.Windows.Forms.TextBox();
            this.textBoxCount = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxQty = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.labelUnit = new System.Windows.Forms.Label();
            this.textBoxDiff = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxPZ = new System.Windows.Forms.TextBox();
            this.textBoxStat = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.buttonConfirm = new System.Windows.Forms.Button();
            this.buttonReCount = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            this.textBoxEmpCode = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.textBoxEmpName = new System.Windows.Forms.TextBox();
            this.textBoxCostOnly = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxPrice = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.Location = new System.Drawing.Point(58, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 33);
            this.label1.TabIndex = 0;
            this.label1.Text = "บาร์โค้ด :";
            // 
            // textBoxScanner
            // 
            this.textBoxScanner.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.textBoxScanner.Location = new System.Drawing.Point(182, 89);
            this.textBoxScanner.Name = "textBoxScanner";
            this.textBoxScanner.Size = new System.Drawing.Size(383, 40);
            this.textBoxScanner.TabIndex = 1;
            this.textBoxScanner.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyUp);
            // 
            // textBoxCount
            // 
            this.textBoxCount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.textBoxCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.textBoxCount.Location = new System.Drawing.Point(182, 135);
            this.textBoxCount.Name = "textBoxCount";
            this.textBoxCount.Size = new System.Drawing.Size(163, 40);
            this.textBoxCount.TabIndex = 3;
            this.textBoxCount.Text = "0";
            this.textBoxCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxCount.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxCount_KeyUp);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label2.Location = new System.Drawing.Point(22, 138);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(154, 33);
            this.label2.TabIndex = 2;
            this.label2.Text = "ตรวจนับได้ :";
            // 
            // textBoxQty
            // 
            this.textBoxQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.textBoxQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.textBoxQty.Location = new System.Drawing.Point(553, 342);
            this.textBoxQty.Name = "textBoxQty";
            this.textBoxQty.ReadOnly = true;
            this.textBoxQty.Size = new System.Drawing.Size(75, 22);
            this.textBoxQty.TabIndex = 5;
            this.textBoxQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxQty.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label3.Location = new System.Drawing.Point(514, 342);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "ระบบ :";
            this.label3.Visible = false;
            // 
            // labelUnit
            // 
            this.labelUnit.AutoSize = true;
            this.labelUnit.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.labelUnit.Location = new System.Drawing.Point(351, 138);
            this.labelUnit.Name = "labelUnit";
            this.labelUnit.Size = new System.Drawing.Size(80, 33);
            this.labelUnit.TabIndex = 6;
            this.labelUnit.Text = "หน่วย";
            // 
            // textBoxDiff
            // 
            this.textBoxDiff.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.textBoxDiff.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.textBoxDiff.Location = new System.Drawing.Point(553, 370);
            this.textBoxDiff.Name = "textBoxDiff";
            this.textBoxDiff.ReadOnly = true;
            this.textBoxDiff.Size = new System.Drawing.Size(75, 22);
            this.textBoxDiff.TabIndex = 9;
            this.textBoxDiff.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxDiff.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label7.Location = new System.Drawing.Point(514, 373);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 16);
            this.label7.TabIndex = 8;
            this.label7.Text = "ผลต่าง :";
            this.label7.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label8.Location = new System.Drawing.Point(11, 224);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 29);
            this.label8.TabIndex = 11;
            this.label8.Text = "สินค้า :";
            // 
            // textBoxName
            // 
            this.textBoxName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.textBoxName.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.textBoxName.Location = new System.Drawing.Point(94, 221);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.ReadOnly = true;
            this.textBoxName.Size = new System.Drawing.Size(630, 35);
            this.textBoxName.TabIndex = 12;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label9.Location = new System.Drawing.Point(11, 265);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(121, 29);
            this.label9.TabIndex = 13;
            this.label9.Text = "Packsize :";
            // 
            // textBoxPZ
            // 
            this.textBoxPZ.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.textBoxPZ.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.textBoxPZ.Location = new System.Drawing.Point(138, 262);
            this.textBoxPZ.Name = "textBoxPZ";
            this.textBoxPZ.ReadOnly = true;
            this.textBoxPZ.Size = new System.Drawing.Size(163, 35);
            this.textBoxPZ.TabIndex = 14;
            this.textBoxPZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxStat
            // 
            this.textBoxStat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.textBoxStat.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.textBoxStat.Location = new System.Drawing.Point(432, 262);
            this.textBoxStat.Name = "textBoxStat";
            this.textBoxStat.ReadOnly = true;
            this.textBoxStat.Size = new System.Drawing.Size(163, 35);
            this.textBoxStat.TabIndex = 16;
            this.textBoxStat.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label10.Location = new System.Drawing.Point(339, 265);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(87, 29);
            this.label10.TabIndex = 15;
            this.label10.Text = "สถานะ :";
            // 
            // buttonConfirm
            // 
            this.buttonConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.buttonConfirm.Location = new System.Drawing.Point(138, 320);
            this.buttonConfirm.Name = "buttonConfirm";
            this.buttonConfirm.Size = new System.Drawing.Size(151, 44);
            this.buttonConfirm.TabIndex = 17;
            this.buttonConfirm.Text = "ยืนยันนับ";
            this.buttonConfirm.UseVisualStyleBackColor = true;
            this.buttonConfirm.Click += new System.EventHandler(this.buttonConfirm_Click);
            // 
            // buttonReCount
            // 
            this.buttonReCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.buttonReCount.Location = new System.Drawing.Point(601, 262);
            this.buttonReCount.Name = "buttonReCount";
            this.buttonReCount.Size = new System.Drawing.Size(123, 35);
            this.buttonReCount.TabIndex = 18;
            this.buttonReCount.Text = "นับใหม่";
            this.buttonReCount.UseVisualStyleBackColor = true;
            this.buttonReCount.Click += new System.EventHandler(this.buttonReCount_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(650, 367);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 19;
            this.button3.Text = "ปิดหน้าต่าง";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // buttonClear
            // 
            this.buttonClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.buttonClear.Location = new System.Drawing.Point(329, 320);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(151, 44);
            this.buttonClear.TabIndex = 20;
            this.buttonClear.Text = "ล้างหน้าจอ";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // textBoxEmpCode
            // 
            this.textBoxEmpCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.textBoxEmpCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.textBoxEmpCode.Location = new System.Drawing.Point(182, 43);
            this.textBoxEmpCode.Name = "textBoxEmpCode";
            this.textBoxEmpCode.Size = new System.Drawing.Size(107, 40);
            this.textBoxEmpCode.TabIndex = 21;
            this.textBoxEmpCode.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxEmpCode_KeyUp);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label4.Location = new System.Drawing.Point(36, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(140, 33);
            this.label4.TabIndex = 22;
            this.label4.Text = "ผู้ตรวจนับ :";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.button1.Location = new System.Drawing.Point(295, 40);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(53, 44);
            this.button1.TabIndex = 23;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBoxEmpName
            // 
            this.textBoxEmpName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.textBoxEmpName.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.textBoxEmpName.Location = new System.Drawing.Point(354, 43);
            this.textBoxEmpName.Name = "textBoxEmpName";
            this.textBoxEmpName.ReadOnly = true;
            this.textBoxEmpName.Size = new System.Drawing.Size(313, 40);
            this.textBoxEmpName.TabIndex = 24;
            // 
            // textBoxCostOnly
            // 
            this.textBoxCostOnly.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.textBoxCostOnly.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.textBoxCostOnly.Location = new System.Drawing.Point(182, 180);
            this.textBoxCostOnly.Name = "textBoxCostOnly";
            this.textBoxCostOnly.ReadOnly = true;
            this.textBoxCostOnly.Size = new System.Drawing.Size(163, 35);
            this.textBoxCostOnly.TabIndex = 25;
            this.textBoxCostOnly.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label5.Location = new System.Drawing.Point(36, 179);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(139, 33);
            this.label5.TabIndex = 26;
            this.label5.Text = "ทุน/หน่วย :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label6.Location = new System.Drawing.Point(358, 179);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(201, 33);
            this.label6.TabIndex = 28;
            this.label6.Text = "ราคาขาย/หน่วย :";
            // 
            // textBoxPrice
            // 
            this.textBoxPrice.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.textBoxPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.textBoxPrice.Location = new System.Drawing.Point(562, 180);
            this.textBoxPrice.Name = "textBoxPrice";
            this.textBoxPrice.ReadOnly = true;
            this.textBoxPrice.Size = new System.Drawing.Size(163, 35);
            this.textBoxPrice.TabIndex = 27;
            this.textBoxPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // CheckFrontStockForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(737, 402);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxPrice);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxCostOnly);
            this.Controls.Add(this.textBoxEmpName);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxEmpCode);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.buttonReCount);
            this.Controls.Add(this.buttonConfirm);
            this.Controls.Add(this.textBoxStat);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBoxPZ);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBoxDiff);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.labelUnit);
            this.Controls.Add(this.textBoxQty);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxCount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxScanner);
            this.Controls.Add(this.label1);
            this.Name = "CheckFrontStockForm";
            this.Text = "CheckFrontStockForm";
            this.Load += new System.EventHandler(this.CheckFrontStockForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxScanner;
        private System.Windows.Forms.TextBox textBoxCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxQty;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelUnit;
        private System.Windows.Forms.TextBox textBoxDiff;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxPZ;
        private System.Windows.Forms.TextBox textBoxStat;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button buttonConfirm;
        private System.Windows.Forms.Button buttonReCount;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.TextBox textBoxEmpCode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBoxEmpName;
        private System.Windows.Forms.TextBox textBoxCostOnly;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxPrice;
    }
}