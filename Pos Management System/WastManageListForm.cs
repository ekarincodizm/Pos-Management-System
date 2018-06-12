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
    public partial class WastManageListForm : Form
    {
        public WastManageListForm()
        {
            InitializeComponent();
        }

        private void WastManageListForm_Load(object sender, EventArgs e)
        {
            ReloadGrid();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReloadGrid();
        }
        public void ReloadGrid()
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                DateTime dateS = dateTimePickerStart.Value;
                DateTime dateE = dateTimePickerEnd.Value;
                List<StoreFrontTransferWaste> list = new List<StoreFrontTransferWaste>();
                list = db.StoreFrontTransferWaste.Where(w => w.Enable == true &&
                DbFunctions.TruncateTime(w.CreateDate) >= DbFunctions.TruncateTime(dateS) &&
                DbFunctions.TruncateTime(w.CreateDate) <= DbFunctions.TruncateTime(dateE)).OrderBy(w => w.CreateDate).ToList();

                decimal totalQty = 0;
                foreach (var item in list)
                {
                    totalQty += item.TotalQtyUnit;
                    dataGridView1.Rows.Add(
                        Library.ConvertDateToThaiDate(item.CreateDate),
                        Library.GetFullNameUserById(item.CreateBy),
                        item.Code,
                        item.StoreFrontTransferWasteDtl.Where(w => w.Enable == true).ToList().Count(),
                        Library.ConvertDecimalToStringForm(item.TotalQtyUnit),
                        item.WasteReason.Name,
                        item.Description
                        );
                }
                textBoxQtyList.Text = Library.ConvertDecimalToStringForm(list.Count()) + "";
                textBoxTotalUnit.Text = Library.ConvertDecimalToStringForm(totalQty) + "";
            }
        }
        public void ReloadGrid(string code)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();

                List<StoreFrontTransferWaste> list = new List<StoreFrontTransferWaste>();
                list = db.StoreFrontTransferWaste.Where(w => code == code).OrderBy(w => w.CreateDate).ToList();

                decimal totalQty = 0;
                foreach (var item in list)
                {
                    totalQty += item.TotalQtyUnit;
                    dataGridView1.Rows.Add(
                        Library.ConvertDateToThaiDate(item.CreateDate),
                        Library.GetFullNameUserById(item.CreateBy),
                        item.Code,
                        item.StoreFrontTransferWasteDtl.Where(w => w.Enable == true).ToList().Count(),
                        Library.ConvertDecimalToStringForm(item.TotalQtyUnit),
                        item.Warehouse.Name + " " + item.Warehouse.Description,
                        item.WasteReason.Name,
                        item.Description
                        );
                }
                textBoxQtyList.Text = Library.ConvertDecimalToStringForm(list.Count()) + "";
                textBoxTotalUnit.Text = Library.ConvertDecimalToStringForm(totalQty) + "";
            }
        }
        /// <summary>
        /// double click เพื่อดูรายละเอียด
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            /// เปิดรายการ
            string code = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value.ToString();
            WastManageDetailForm obj = new WastManageDetailForm(this, code);
            obj.ShowDialog();
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }
        /// <summary>
        /// ค้นหา 1 code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonPlus_Click(object sender, EventArgs e)
        {
            string code = textBoxKeySearch.Text.Trim();
            ReloadGrid(code);
        }

        private void textBoxKeySearch_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    string code = textBoxKeySearch.Text.Trim();
                    ReloadGrid(code);
                    break;
                default:
                    break;
            }
        }
        int NoWM = 2;
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                /// รหัสใบออเดอร์
                string code = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[NoWM].Value.ToString();
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
