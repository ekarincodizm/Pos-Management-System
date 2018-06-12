using Pos_Management_System.Singleton;
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
    public partial class GoodsReturnVendorConfirmForm : Form
    {
        public GoodsReturnVendorConfirmForm()
        {
            InitializeComponent();
        }

        private void GoodsReturnVendorConfirmForm_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// ค้นหา
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            SearchData();
        }

        private void SearchData()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            using (SSLsEntities db = new SSLsEntities())
            {
                DateTime dateS = dateTimePickerStart.Value;
                DateTime dateE = dateTimePickerEnd.Value;

                var data = db.CNWarehouse.Where(w => w.Enable == true && 
                DbFunctions.TruncateTime(w.CreateDate) >= DbFunctions.TruncateTime(dateS) && 
                DbFunctions.TruncateTime(w.CreateDate) <= DbFunctions.TruncateTime(dateE)).ToList();
                if (_VendorId != 0)
                {
                    data = data.Where(w => w.FKVendor == _VendorId).ToList();
                }
                decimal qty = 0;
                decimal cn = 0;
                decimal nocn = 0;
                foreach (var item in data)
                {
                    string cnString = "0";
                    string nocnString = "0";
                    qty += item.TotalQtyUnit;
                    if (item.ConfirmCNDate == null)
                    {
                        nocn += item.TotalBalance;
                        nocnString = Library.ConvertDecimalToStringForm(item.TotalBalance);
                    }
                    else
                    {
                        cn += item.TotalBalance;
                        cnString = Library.ConvertDecimalToStringForm(item.TotalBalance);
                    }
                    dataGridView1.Rows.Add
                        (
                        item.Code,
                        Library.ConvertDateToThaiDate(item.CreateDate),
                        Library.GetFullNameUserById(item.CreateBy),
                        Library.ConvertDateToThaiDate(item.ConfirmCNDate),
                        Library.GetFullNameUserById(item.ConfirmCNBy),
                        item.WasteReason.Name,
                        item.Vendor.Name,
                        Library.ConvertDecimalToStringForm(item.TotalQtyUnit),
                        Library.ConvertDecimalToStringForm(item.TotalBalance),
                        cnString,
                        nocnString
                        );    
                }
                labelQty.Text = Library.ConvertDecimalToStringForm(qty);
                labelCN.Text = Library.ConvertDecimalToStringForm(cn);
                labelNoCN.Text = Library.ConvertDecimalToStringForm(nocn);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SelectedVendorPopup dwv = new SelectedVendorPopup(this);
            dwv.ShowDialog();
        }
        int _VendorId = 0;
        public void BinddingVendor(int id)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            _VendorId = id;
            List<Vendor> vendors = SingletonVender.Instance().Vendors;
            var data = vendors.SingleOrDefault(w => w.Id == id);
            textBoxVendorCode.Text = data.Code;
            textBoxVendorName.Text = data.Name;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _VendorId = 0;
            textBoxVendorCode.Text = "";
            textBoxVendorName.Text = "";
        }
        /// <summary>
        /// คอนเฟิม 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่ ?",
                      "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    SaveConfirm();
                    break;
                case DialogResult.No:
                    break;
            }
        }

        private void SaveConfirm()
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                string code = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString();
                var data = db.CNWarehouse.SingleOrDefault(w => w.Enable == true && w.Code == code);
                data.ConfirmCNDate = DateTime.Now;
                data.ConfirmCNBy = SingletonAuthen.Instance().Id;
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
                SearchData();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string code = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString();
            // Open Paper WasteCN
            frmMainReport mr = new frmMainReport(this, code);
            mr.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            CodeFileDLL.ExcelReport(dataGridView1, "รายงานใบทำคืนของเสีย ณ วันที่ " + Library.ConvertDateToThaiDate(dateTimePickerStart.Value), "");
        }
    }
}
