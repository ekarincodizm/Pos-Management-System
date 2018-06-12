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
    public partial class GetGoodsUsedListForm : Form
    {
        public GetGoodsUsedListForm()
        {
            InitializeComponent();
        }
        int row;
        private void GetGoodsUsedListForm_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            using (SSLsEntities db = new SSLsEntities())
            {
                var date1 = dateTimePicker1.Value;
                var date2 = dateTimePicker2.Value;

                var getData = db.GetGoodsStoreFront.Where(w => w.Enable == true &&
                DbFunctions.TruncateTime(w.CreateDate) >= DbFunctions.TruncateTime(date1) &&
                DbFunctions.TruncateTime(w.CreateDate) <= DbFunctions.TruncateTime(date2))
                .OrderBy(w => w.CreateDate).ToList();
                decimal sumQty = 0;
                decimal sumValue = 0;
                foreach (var item in getData)
                {
                    sumQty += item.TotalQtyUnit;
                    sumValue += item.TotalBalance;
                    dataGridView1.Rows.Add
                        (
                        item.Code,
                        Library.ConvertDateToThaiDate(item.CreateDate, true),
                        Library.GetFullNameUserById(item.CreateBy),
                        item.Description,
                        Library.ConvertDecimalToStringForm(item.TotalQtyUnit),
                        Library.ConvertDecimalToStringForm(item.TotalBalance)
                        );
                }
                label4.Text = Library.ConvertDecimalToStringForm(sumQty);
                label5.Text = Library.ConvertDecimalToStringForm(sumValue);
            }
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CodeFileDLL.ExcelReport(dataGridView1, "รายงานเบิกใช้ ณ วันที่ " + Library.ConvertDateToThaiDate(dateTimePicker1.Value) + " - " + Library.ConvertDateToThaiDate(dateTimePicker2.Value), "");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (row > -1)
            {
                DialogResult dr = MessageBox.Show("คุณต้องพิมพ์ซ้ำ ใช่หรือไม่ ?", "เเจ้งเพื่อทราบ", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                switch (dr)
                {
                    case DialogResult.Yes:
                        Cursor.Current = Cursors.WaitCursor;
                        frmMainReport report = new frmMainReport(this, dataGridView1.Rows[row].Cells["Column1"].Value.ToString());
                        report.Show();
                        Cursor.Current = Cursors.Default;
                        break;
                    case DialogResult.No:
                        break;
                }
            }
        }

        private void DgCC(object sender, DataGridViewCellEventArgs e)
        {
            row = e.RowIndex;
        }

        private void DgC(object sender, DataGridViewCellEventArgs e)
        {
            row = e.RowIndex;
        }
    }
}
