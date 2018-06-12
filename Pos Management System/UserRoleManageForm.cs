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
    public partial class UserRoleManageForm : Form
    {
        public UserRoleManageForm()
        {
            InitializeComponent();
        }

        private void UserRoleManageForm_Load(object sender, EventArgs e)
        {
            Reload();
        }
        /// <summary>
        /// โหลด Default
        /// </summary>
        private void Reload()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            using (SSLsEntities db = new SSLsEntities())
            {
                var data = db.Users.ToList();
                foreach (var item in data)
                {
                    string enable = "";
                    if (item.Enable)
                    {
                        enable = "ใช้งานปกติ";
                    }
                    else
                    {
                        enable = "ระงับใช้งานแล้ว";
                    }
                    dataGridView1.Rows.Add(item.Id, item.Name, item.Description, item.Role.Name, enable);
                }
            }
        }
        /// <summary>
        /// โหลด ตามคีย์ค้นหา
        /// </summary>
        /// <param name="key"></param>
        private void Reload(string key)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            using (SSLsEntities db = new SSLsEntities())
            {
                var data = db.Users.ToList();
                if (radioButtonCode.Checked)
                {
                    data = data.Where(w => w.Id.Contains(key)).ToList();
                }
                else
                {
                    data = data.Where(w => w.Id.Contains(key)).ToList();
                }
                foreach (var item in data)
                {
                    string enable = "";
                    if (item.Enable)
                    {
                        enable = "ใช้งานปกติ";
                    }
                    else
                    {
                        enable = "ระงับใช้งานแล้ว";
                    }
                    dataGridView1.Rows.Add(item.Id, item.Name, item.Description, item.Role.Name, enable);
                }
            }
        }
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }
        /// <summary>
        /// เลือก User ใน Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            SelectRowGrid();
        }

        private void SelectRowGrid()
        {
            _checkAdd = false;
            string user = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString();

            using (SSLsEntities db = new SSLsEntities())
            {
                var data = db.Users.SingleOrDefault(w => w.Id == user);
                textBoxUser.Text = data.Id;
                textBoxFullName.Text = data.Name;
                textBoxRoleName.Text = data.Role.Name;
                textBoxRoleId.Text = data.FKRole + "";
                textBoxDesc.Text = data.Description;
                if (data.Enable == true)
                {
                    radioButtonActive.Checked = true;
                }
                else
                {
                    radioButtonNon.Checked = true;
                }
            }
        }

        /// <summary>
        /// open popup role
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            SelectedRoleForm obj = new SelectedRoleForm(this);
            obj.ShowDialog();
        }
        /// <summary>
        /// หลังจากเลือก Role
        /// </summary>
        /// <param name="role"></param>
        public void BinddingRole(int role)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                var data = db.Role.SingleOrDefault(w => w.Id == role);
                textBoxRoleName.Text = data.Name;
                textBoxRoleId.Text = data.Id.ToString();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        /// <summary>
        /// เพิ่มใหม่
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            ResetTextbox();

        }
        bool _checkAdd = false;
        private void ResetTextbox()
        {
            textBoxUser.Text = "";
            textBoxUser.Enabled = true;
            textBoxFullName.Text = "";
            textBoxRoleName.Text = "";
            textBoxRoleId.Text = "";
            textBoxDesc.Text = "";
            radioButtonActive.Checked = true;
            //dataGridView1.ClearSelection();
            _checkAdd = true;
        }

        /// <summary>
        /// บันทึก
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                try
                {
                    if (_checkAdd)
                    {
                        // is add new 
                        Users user = new Users();
                        user.Id = textBoxUser.Text;
                        user.Password = "123456";
                        user.Name = textBoxFullName.Text;
                        user.Description = textBoxDesc.Text;
                        user.FKRole = int.Parse(textBoxRoleId.Text);
                        user.FKBranch = MyConstant.MyBranch._02;
                        user.CreateDate = DateTime.Now;
                        user.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                        user.UpdateDate = DateTime.Now;
                        user.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                        user.Enable = true;
                        db.Users.Add(user);
                        _checkAdd = false;
                    }
                    else
                    {
                        var data = db.Users.SingleOrDefault(w => w.Id == textBoxUser.Text);
                        data.Name = textBoxFullName.Text;
                        data.FKRole = int.Parse(textBoxRoleId.Text);
                        data.Description = textBoxDesc.Text;
                        data.UpdateDate = DateTime.Now;
                        data.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                        if (radioButtonNon.Checked == true)
                        {
                            // ถ้าระงับใช้งาน
                            data.Enable = false;
                        }
                        else
                        {
                            data.Enable = true;
                        }
                        db.Entry(data).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    Reload();
                }
                catch (Exception)
                {
                    MessageBox.Show("กรุณาตรวจสอบ User ซ้ำ");
                }
            }

        }
        /// <summary>
        /// CLick Cell ให้ Disable user textbox 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectRowGrid();
            textBoxUser.Enabled = false;
            _checkAdd = false;
        }
        /// <summary>
        /// Enter Search Key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxSearchKey_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    Reload(textBoxSearchKey.Text.Trim());
                    break;
                default:
                    break;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            CodeFileDLL.ExcelReport(dataGridView1, "รายงานผู้ใช้งาน", "");
        }
    }
}
