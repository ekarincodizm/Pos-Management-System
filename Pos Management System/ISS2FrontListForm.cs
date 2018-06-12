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
    public partial class ISS2FrontListForm : Form
    {
        public ISS2FrontListForm()
        {
            InitializeComponent();
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }
        /// <summary>
        /// ค้นหา
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            if (_CreateBy == null)
            {
                LoadGrid();
            }
            else
            {
                LoadGrid(_CreateBy);
            }
        }

        private void ISS2FrontListForm_Load(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private void LoadGrid(string createBy)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                DateTime dateS = dateTimePickerStart.Value;
                DateTime dateE = dateTimePickerEnd.Value;
                List<ISS2Front> list = new List<ISS2Front>();
                list = db.ISS2Front.Where(w => w.CreateBy == createBy && DbFunctions.TruncateTime(w.CreateDate) >= DbFunctions.TruncateTime(dateS) && DbFunctions.TruncateTime(w.CreateDate) <= DbFunctions.TruncateTime(dateE)).OrderBy(w => w.CreateDate).ToList();
                foreach (var item in list)
                {
                    string status = "";
                    if (item.Enable == false)
                    {
                        status = "ยกเลิกออร์เดอร์";
                    }
                    else
                    {
                        status = item.ISS2FrontStatus.Name;
                    }
                    dataGridView1.Rows.Add(Library.ConvertDateToThaiDate(item.CreateDate),
                        Library.GetFullNameUserById(item.CreateBy),
                        item.Code,
                        status,
                        Library.ConvertDecimalToStringForm(item.TotalQtyUnit),
                        Library.ConvertDecimalToStringForm(item.TotalQty),
                        item.Description
                        );
                }

            }
        }
        private void LoadGrid()
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                DateTime dateS = dateTimePickerStart.Value;
                DateTime dateE = dateTimePickerEnd.Value;
                List<ISS2Front> list = new List<ISS2Front>();
                list = db.ISS2Front.Where(w => DbFunctions.TruncateTime(w.CreateDate) >= DbFunctions.TruncateTime(dateS) && DbFunctions.TruncateTime(w.CreateDate) <= DbFunctions.TruncateTime(dateE)).OrderBy(w => w.CreateDate).ToList();
                foreach (var item in list)
                {
                    string status = "";
                    if (item.Enable == false)
                    {
                        status = "ยกเลิกออร์เดอร์";
                    }
                    else
                    {
                        status = item.ISS2FrontStatus.Name;
                    }
                    dataGridView1.Rows.Add(Library.ConvertDateToThaiDate(item.CreateDate),
                        Library.GetFullNameUserById(item.CreateBy),
                        item.Code,
                        status,
                        Library.ConvertDecimalToStringForm(item.TotalQtyUnit),
                        Library.ConvertDecimalToStringForm(item.TotalQty),
                        item.Description
                        );
                }

            }
        }
        /// <summary>
        /// เปิด Employee
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            SelectedUserPopup obj = new SelectedUserPopup(this);
            obj.ShowDialog();
        }
        string _CreateBy = null;
        public void BinddingUser(Users send)
        {
            _CreateBy = send.Id;
            textBoxUserCode.Text = send.Id;
            textBoxUserName.Text = send.Name;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                string code = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value.ToString();
                var data = db.ISS2Front.SingleOrDefault(w => w.Code == code);
                // ต้อง เป็น สถานะ create และตัวเองเท่านั้น ถึงยกเลิกได้
                if (data.Enable && data.FKISS2FrontStatus == MyConstant.ISS2FrontStatus.CreateOrder && data.CreateBy == Singleton.SingletonAuthen.Instance().Id)
                {
                    DialogResult dr = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่ ?",
                      "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
                    switch (dr)
                    {
                        case DialogResult.Yes:
                            // update point
                            data.UpdateDate = DateTime.Now;
                            data.Enable = false;
                            db.Entry(data).State = EntityState.Modified;
                            db.SaveChanges();
                            if (_CreateBy == null)
                            {
                                LoadGrid();
                            }
                            else
                            {
                                LoadGrid(_CreateBy);
                            }
                            break;
                        case DialogResult.No:
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("ไม่สามารถยกเลิกได้");
                }
            }
        }
        /// <summary>
        /// Clear ผู้เบิก
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _CreateBy = null;
            textBoxUserCode.Text = "";
            textBoxUserName.Text = "";
        }
        /// <summary>
        /// Double Click เพื่อดู details สินค้า
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            string code = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value.ToString();
            ISS2FrontListDetailForm obj = new ISS2FrontListDetailForm(this, code);
            obj.ShowDialog();
        }
    }
}
