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
    public partial class CNPosListForm : Form
    {
        public CNPosListForm()
        {
            InitializeComponent();
        }

        private void CNPosListForm_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// ค้นหาแบบ สนใจวันที่
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            SearchWithDate();

        }
        /// <summary>
        /// ค้นหา
        /// </summary>
        private void SearchWithDate()
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                DateTime dateS = dateTimePickerStart.Value;
                DateTime dateE = dateTimePickerEnd.Value;
                List<CNHeader> list = new List<CNHeader>();
                list = db.CNHeader.Where(w => w.Enable == true &&
                DbFunctions.TruncateTime(w.CreateDate) >= DbFunctions.TruncateTime(dateS) &&
                DbFunctions.TruncateTime(w.CreateDate) <= DbFunctions.TruncateTime(dateE)).OrderBy(w => w.CreateDate).ToList();
                foreach (var item in list)
                {
                    dataGridView1.Rows.Add
                        (
                        Library.ConvertDateToThaiDate(item.CreateDate),
                        Library.GetFullNameUserById(item.CreateBy),
                        item.No,
                        item.CNType.Name,
                        item.PosHeader.InvoiceNo,
                        item.SequenceNo,
                        item.QtyList,
                        item.Qty,
                        Library.ConvertDecimalToStringForm(item.TotalBalance),
                        item.Description
                        );
                }
            }
        }
        /// <summary>
        /// ค้นหาแบบบ เลขที่เอกสาร
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            SearchWithDocumentNo();

        }

        private void SearchWithDocumentNo()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            string invoiceNo = textBoxInvoiceNo.Text.Trim();
            string cnNo = textBoxCNNo.Text.Trim();
            List<CNHeader> list = new List<CNHeader>();
            using (SSLsEntities db = new SSLsEntities())
            {
                if (cnNo != "")
                {
                    // สนใจ เลขที่ cn
                    list = db.CNHeader.Where(w => w.Enable == true && w.No == cnNo).ToList();
                }
                else if (invoiceNo != "")
                {
                    // สนใน invoiceNo
                    int fkpos = db.PosHeader.SingleOrDefault(w=>w.InvoiceNo == invoiceNo).Id;
                    list = db.CNHeader.Where(w => w.Enable == true && w.FKPosHeader == fkpos).ToList();
                }
                foreach (var item in list)
                {
                    dataGridView1.Rows.Add
                        (
                        Library.ConvertDateToThaiDate(item.CreateDate),
                        Library.GetFullNameUserById(item.CreateBy),
                        item.No,
                        item.CNType.Name,
                        item.PosHeader.InvoiceNo,
                        item.SequenceNo,
                        item.QtyList,
                        item.Qty,
                        Library.ConvertDecimalToStringForm(item.TotalBalance),
                        item.Description
                        );
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
        /// ดูรายละเอียด
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            OpenDetailList();
        }
        /// <summary>
        /// เปิดหน้าต่างรายละเอียด
        /// </summary>
        private void OpenDetailList()
        {
            var cnNo = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value.ToString();
            CNPosListDialog obj = new CNPosListDialog(this, cnNo);
            obj.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenDetailList();
        }
        /// <summary>
        /// พิมพ์ใบคืน
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        int NoCN = 2;
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                /// รหัสใบออเดอร์
                string code = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[NoCN].Value.ToString();
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
