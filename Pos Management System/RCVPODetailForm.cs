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
    public partial class RCVPODetailForm : Form
    {
        public RCVPODetailForm()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ReloadGrid();
        }
        public void ReloadGrid()
        {
            try
            {
                using (SSLsEntities db = new SSLsEntities())
                {
                    dataGridView1.Rows.Clear();
                    dataGridView1.Refresh();
                    DateTime dateS = dateTimePickerStart.Value;
                    DateTime dateE = dateTimePickerEnd.Value;
                    List<PORcv> list = new List<PORcv>();
                    list = db.PORcv.Where(w => w.Enable == true &&
                    DbFunctions.TruncateTime(w.CreateDate) >= DbFunctions.TruncateTime(dateS) &&
                    DbFunctions.TruncateTime(w.CreateDate) <= DbFunctions.TruncateTime(dateE)).OrderBy(w => w.CreateDate).ToList();

                    foreach (var item in list)
                    {
                        dataGridView1.Rows.Add(
                           Library.ConvertDateToThaiDate(item.CreateDate),
                           Library.GetFullNameUserById(item.CreateBy),
                            item.Code,
                            item.POHeader.PONo,
                            item.POHeader.Vendor.Name,
                            item.Description,
                            item.PORcvDetails.ToList().Count(),
                           Library.ConvertDecimalToStringForm(item.DiscountBath),
                            Library.ConvertDecimalToStringForm(item.TotalBUnVat),
                           Library.ConvertDecimalToStringForm(item.TotalBHasVat),
                           Library.ConvertDecimalToStringForm(item.TotalVat),
                           Library.ConvertDecimalToStringForm(item.TotalGift),
                            item.Transport.Name
                            );
                    }
                    //dataGridView1.AutoResizeColumns();
                    //dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "แจ้งเตือนจากระบบ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }
        int keyCode = 2;

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                /// รหัสใบออเดอร์
                //string code = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[keyCode].Value.ToString();
                //RCVPODetails mr = new RCVPODetails(this, code);
                //mr.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("จำนวนเอกสารผิดพลาด");
            }
        }

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

        private void ปิด_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                /// เลขที่ใบรับเข้า
                string code = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[keyCode].Value.ToString();
                RCVPOEditForm rcv = new RCVPOEditForm(this, code);
                rcv.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("จำนวนเอกสารผิดพลาด");
            }
        }
    }
}
