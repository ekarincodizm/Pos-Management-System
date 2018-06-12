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
    public partial class ISS2FrontManageForm : Form
    {
        public ISS2FrontManageForm()
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

        private void ISS2FrontManageForm_Load(object sender, EventArgs e)
        {
            LoadGrid();
        }
        public void LoadGrid()
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                DateTime dateS = dateTimePickerStart.Value;
                DateTime dateE = dateTimePickerEnd.Value;
                List<ISS2Front> list = new List<ISS2Front>();
                if (_CreateBy == null)
                {
                    list = db.ISS2Front.Where(w => w.Enable && DbFunctions.TruncateTime(w.CreateDate) >= DbFunctions.TruncateTime(dateS) && DbFunctions.TruncateTime(w.CreateDate) <= DbFunctions.TruncateTime(dateE)).OrderBy(w => w.CreateDate).ToList();
                }
                else
                {
                    list = db.ISS2Front.Where(w => w.Enable && w.CreateBy == _CreateBy && DbFunctions.TruncateTime(w.CreateDate) >= DbFunctions.TruncateTime(dateS) && DbFunctions.TruncateTime(w.CreateDate) <= DbFunctions.TruncateTime(dateE)).OrderBy(w => w.CreateDate).ToList();
                }

                foreach (var item in list)
                {
                    //string status = "";
                    //if (item.Enable == false)
                    //{
                    //    status = "ยกเลิกออร์เดอร์";
                    //}
                    //else
                    //{
                    //    status = item.ISS2FrontStatus.Name;
                    //}
                    dataGridView1.Rows.Add(Library.ConvertDateToThaiDate(item.CreateDate),
                        Library.GetFullNameUserById(item.CreateBy),
                        item.Code,
                        item.ISS2FrontStatus.Name,
                        Library.ConvertDecimalToStringForm(item.TotalQtyUnit),
                        Library.ConvertDecimalToStringForm(item.TotalQty),
                        item.Description
                        );
                }

            }
        }

        /// <summary>
        /// ยืนยัน การเบิก เปิด list windows
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            string code = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value.ToString();
            ISS2FrontAllowConfirmForm obj = new ISS2FrontAllowConfirmForm(this, code);
            obj.ShowDialog();
        }
        /// <summary>
        /// ปฏิเสธออร์เดอร์
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                string code = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value.ToString();
                var data = db.ISS2Front.SingleOrDefault(w => w.Code == code);
                if (data.FKISS2FrontStatus == MyConstant.ISS2FrontStatus.CreateOrder)
                {
                    if (textBoxRemark.Text.Trim() == "")
                    {
                        MessageBox.Show("กรุณาใส่หมายเหตุ");
                        return;
                    }
                    DialogResult dr = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่ ?", "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
                    switch (dr)
                    {
                        case DialogResult.Yes:
                            data.FKISS2FrontStatus = MyConstant.ISS2FrontStatus.CancelOrder;
                            data.UpdateDate = DateTime.Now;
                            data.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                            data.Remark = textBoxRemark.Text.Trim();
                            data.Takedate = DateTime.Now;
                            data.TakeBy = Singleton.SingletonAuthen.Instance().Id;
                            db.Entry(data).State = EntityState.Modified;
                            db.SaveChanges();

                            LoadGrid();
                            break;
                        case DialogResult.No:
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("ไม่สามารถปฏิเสธออร์เดอร์นี้ได้");
                }
            }
        }

        private void RejectOrderSave()
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                string code = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value.ToString();
                var data = db.ISS2Front.SingleOrDefault(w => w.Code == code);
            }
        }
        /// <summary>
        /// ค้นหา
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }
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
    }
}
