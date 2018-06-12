using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos_Management_System
{
    public partial class ResetPasswordForm : Form
    {
        public ResetPasswordForm()
        {
            InitializeComponent();
        }

        private void ResetPasswordForm_Load(object sender, EventArgs e)
        {
            textBox1.Text = Singleton.SingletonAuthen.Instance().Name;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        /// <summary>
        /// ตกลง
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            /// check ความถูกต้อง
            using (SSLsEntities db = new SSLsEntities())
            {
                string id = Singleton.SingletonAuthen.Instance().Id;
                var user = db.Users.SingleOrDefault(w => w.Id == id);
                if (textBoxNewPass.Text.Trim().Length < 6)
                {
                    // ถ้า password ใหม่ไม่ถึง 6 ตัว
                    MessageBox.Show("กรุณาตั้งรหัสความยาว 6 หลักขึ้นไปเพื่อความปลอดภัย");
                }
                else if (user.Password != textBoxOldPass.Text.Trim())
                {
                    // ถ้า password เดิมไม่ถูกต้อง
                    MessageBox.Show("กรุณาป้อนรหัสเดิมให้ถูกต้อง");
                }
                else if (textBoxNewPass.Text.Trim() != textBoxNewPass1.Text.Trim())
                {
                    // ถ้า password ใหม่ ไม่ถูกต้อง
                    MessageBox.Show("กรุณาป้อนรหัสใหม่ ให้ถูกต้อง");
                }
                else
                {
                    DialogResult dr = MessageBox.Show("เมื่อบันทึก กรุณาเข้าระบบใหม่อีกครั้ง ?",
           "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
                    switch (dr)
                    {
                        case DialogResult.Yes:
                            SaveSubmit();
                            break;
                        case DialogResult.No:
                            break;
                    }
                }
            }
        }

        private void SaveSubmit()
        {
            // save pass
            using (SSLsEntities db = new SSLsEntities())
            {
                string id = Singleton.SingletonAuthen.Instance().Id;
                var user = db.Users.SingleOrDefault(w => w.Id == id);
                user.Password = textBoxNewPass.Text.Trim();
                user.Description = "แก้ไขรหัสผ่าน " + Library.ConvertDateToThaiDate(DateTime.Now, true);
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
            }
            Environment.Exit(0);
        }
    }
}
