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
    public partial class GoodsReturnWmsListForm : Form
    {
        public GoodsReturnWmsListForm()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SerachData();
        }

        private void SerachData()
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                DateTime dateS = dateTimePickerStart.Value;
                DateTime dateE = dateTimePickerEnd.Value;
                List<CNWarehouse> list = new List<CNWarehouse>();
                list = db.CNWarehouse.Where(w => w.Enable == true &&
                DbFunctions.TruncateTime(w.CreateDate) >= DbFunctions.TruncateTime(dateS) &&
                DbFunctions.TruncateTime(w.CreateDate) <= DbFunctions.TruncateTime(dateE)).OrderBy(w => w.CreateDate).ToList();
                foreach (var item in list)
                {
                    dataGridView1.Rows.Add
                        (
                        Library.ConvertDateToThaiDate(item.CreateDate),
                        Library.GetFullNameUserById(item.CreateBy),
                        item.Code,
                        item.CNWarehouseDetails.Where(w => w.Enable == true).ToList().Count(),
                        item.TotalQtyUnit,
                        item.Vendor.Name,
                        item.WasteReason.Name,
                        item.DocRefer,
                        Library.ConvertDecimalToStringForm(item.TotalBalance)
                        );
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        /// <summary>
        /// ดูรายละเอียด
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            OpenDetailsDialog();
        }

        private void OpenDetailsDialog()
        {
            var cnNo = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value.ToString();
            GoodsReturnWmsDetailDialog obj = new GoodsReturnWmsDetailDialog(this, cnNo);
            obj.ShowDialog();
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            OpenDetailsDialog();
        }
        int keyCode = 2;
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                /// รหัสใบออเดอร์
                string code = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[keyCode].Value.ToString();
                frmMainReport mr = new frmMainReport(this, code);
                mr.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("จำนวนเอกสารผิดพลาด");
            }
        }
    }
}
