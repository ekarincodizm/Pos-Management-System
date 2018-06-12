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
    public partial class EditMyProfileForm : Form
    {
        public EditMyProfileForm()
        {
            InitializeComponent();
        }

        private void EditMyProfileForm_Load(object sender, EventArgs e)
        {
            /// Load value
            using (SSLsEntities db = new SSLsEntities())
            {
                string username = Singleton.SingletonAuthen.Instance().Id;
                var data = db.Users.SingleOrDefault(w => w.Id == username);
                textBoxBranch.Text = data.Branch.Name + " " + data.Branch.BranchNo;
                textBoxUser.Text = data.Id;
                textBoxFullName.Text = data.Name;
                textBoxDesc.Text = data.Description;
                textBoxRole.Text = data.Role.Name;
            }
        }
        /// <summary>
        /// update value profile
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่ ?",
                      "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    SaveCommit();
                    break;
                case DialogResult.No:
                    break;
            }
        }

        private void SaveCommit()
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                string username = Singleton.SingletonAuthen.Instance().Id;
                var data = db.Users.SingleOrDefault(w => w.Id == username);

                data.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                data.UpdateDate = DateTime.Now;
                data.Name = textBoxFullName.Text;
                data.Description = textBoxDesc.Text;
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
                this.Dispose();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
