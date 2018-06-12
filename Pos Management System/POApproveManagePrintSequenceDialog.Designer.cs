namespace Pos_Management_System
{
    partial class POApproveManagePrintSequenceDialog
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxKeyValue = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.labelValue = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "พบ PO แก้ไขครั้งที่";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "พิมพ์ครั้งที่แก้ไข :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(28, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(144, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "*พิมพ์  0 คือ PO สร้างครั้งแรก";
            // 
            // textBoxKeyValue
            // 
            this.textBoxKeyValue.Location = new System.Drawing.Point(119, 72);
            this.textBoxKeyValue.Name = "textBoxKeyValue";
            this.textBoxKeyValue.Size = new System.Drawing.Size(100, 20);
            this.textBoxKeyValue.TabIndex = 3;
            this.textBoxKeyValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxKeyValue.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxKeyValue_KeyUp);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(208, 110);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "ตกลง";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // labelValue
            // 
            this.labelValue.AutoSize = true;
            this.labelValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.labelValue.Location = new System.Drawing.Point(165, 18);
            this.labelValue.Name = "labelValue";
            this.labelValue.Size = new System.Drawing.Size(20, 24);
            this.labelValue.TabIndex = 5;
            this.labelValue.Text = "..";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(226, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Enter";
            // 
            // POApproveManagePrintSequenceDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(295, 145);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.labelValue);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxKeyValue);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "POApproveManagePrintSequenceDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "POApproveManagePrintSequenceDialog";
            this.Load += new System.EventHandler(this.POApproveManagePrintSequenceDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxKeyValue;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label labelValue;
        private System.Windows.Forms.Label label4;
    }
}