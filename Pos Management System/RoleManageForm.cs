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
    public partial class RoleManageForm : Form
    {
        public RoleManageForm()
        {
            InitializeComponent();
        }

        private void RoleManageForm_Load(object sender, EventArgs e)
        {
            ReloaGrid1();
        }

        private void ReloaGrid1()
        {

            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            using (SSLsEntities db = new SSLsEntities())
            {
                foreach (var item in db.Role.Where(w => w.Enable == true).ToList())
                {
                    dataGridView1.Rows.Add
                        (
                        item.Id,
                        item.Code,
                        item.Name,
                        item.Description,
                        Library.ConvertDateToThaiDate(item.UpdateDate),
                        Library.GetFullNameUserById(item.UpdateBy)
                        );
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }
        /// <summary>
        /// Click Row Choose Role In Grid1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        int _idRole = 0;
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                int id = int.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString());
                _idRole = id;
                var role = db.Role.SingleOrDefault(w => w.Id == id);
                // bindding
                textBoxCode.Text = role.Code;
                textBoxName.Text = role.Name;
                textBoxDesc.Text = role.Description;

                dataGridView2.Rows.Clear();
                dataGridView2.Refresh();
                var menus = db.Menu.Where(w => w.Enable == true).ToList();

                foreach (var item in role.MenuAccess.Where(w => w.Enable == true).ToList())
                {
                    menus.Remove(item.Menu);
                    dataGridView2.Rows.Add(item.FKMenu, item.Menu.Code, item.Menu.Description, Library.ConvertDateToThaiDate(item.Menu.CreateDate), Library.GetFullNameUserById(item.UpdateBy));
                }
                dataGridView3.Rows.Clear();
                dataGridView3.Refresh();
                foreach (var item in menus)
                {
                    dataGridView3.Rows.Add(item.Id, item.Code, item.Description, Library.ConvertDateToThaiDate(item.CreateDate), Library.GetFullNameUserById(item.UpdateBy));
                }
            }
        }

        private void dataGridView2_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void dataGridView3_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }
        /// <summary>
        /// เลือกตั้งค่า
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            // ดึงจาก Grid 3 แบบ มัลติ Row
            // มาไว้ Grid2
            foreach (DataGridViewRow r in dataGridView3.SelectedRows)
            {
                dataGridView3.Rows.Remove(r);
                dataGridView2.Rows.Add(r);
            }
        }
        /// <summary>
        /// ยกเลิก ตั้งค่า *ปิดหน้าต่าง
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            // ดึงจาก Grid2 แบบ มัลติ Row
            // มาไว้ Grid3
            foreach (DataGridViewRow r in dataGridView2.SelectedRows)
            {
                dataGridView2.Rows.Remove(r);
                dataGridView3.Rows.Add(r);
            }
        }
        /// <summary>
        /// ยืนยันบันทึก disable ชุดเดิมทิ้งทั้งหมด
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            List<int> ids = new List<int>();
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                //Console.WriteLine();
                int id = int.Parse(dataGridView2.Rows[i].Cells[0].Value.ToString());
                ids.Add(id);
            }
            DialogResult dr = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่ ?",
                      "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    using (SSLsEntities db = new SSLsEntities())
                    {
                        var menuAc = db.MenuAccess.Where(w => w.Enable == true && w.FKRole == _idRole);
                        foreach (var item in menuAc)
                        {
                            item.Enable = false;
                            item.UpdateDate = DateTime.Now;
                            item.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                            db.Entry(item).State = EntityState.Modified;
                        }
                        // add new ids
                        foreach (var item in ids)
                        {
                            MenuAccess ac = new MenuAccess();
                            ac.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                            ac.CreateDate = DateTime.Now;
                            ac.FKRole = _idRole;
                            ac.FKMenu = item;
                            ac.UpdateDate = DateTime.Now;
                            ac.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                            ac.Enable = true;
                            db.MenuAccess.Add(ac);
                        }
                        db.SaveChanges();
                        ReloaGrid1();
                    }
                    break;
                case DialogResult.No:
                    break;
            }
        }
        /// <summary>
        /// add new role
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            textBoxCode.Text = "";
            textBoxName.Text = "";
            textBoxDesc.Text = "";
            _idRole = 0;

            dataGridView2.Rows.Clear();
            dataGridView2.Refresh();

            dataGridView3.Rows.Clear();
            dataGridView3.Refresh();
        }
        /// <summary>
        /// บันทึก Role
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                if (_idRole == 0)
                {
                    // add new role
                    Role role = new Role();
                    role.Code = textBoxCode.Text;
                    role.Name = textBoxName.Text;
                    role.Description = textBoxDesc.Text;
                    role.CreateDate = DateTime.Now;
                    role.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                    role.UpdateDate = DateTime.Now;
                    role.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    role.Enable = true;
                    db.Role.Add(role);
                }
                else
                {
                    // Update Role Value
                    Role role = new Role();
                    role = db.Role.SingleOrDefault(w => w.Id == _idRole);
                    role.Code = textBoxCode.Text;
                    role.Name = textBoxName.Text;
                    role.Description = textBoxDesc.Text;
                    role.UpdateDate = DateTime.Now;
                    role.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    db.Entry(role).State = EntityState.Modified;
                }
                DialogResult dr = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่ ?",
                      "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
                switch (dr)
                {
                    case DialogResult.Yes:
                        db.SaveChanges();
                        ReloaGrid1();
                        break;
                    case DialogResult.No:
                        break;
                }
     
            }           
        }
        /// <summary>
        /// ปิดหน้าต่าง
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
