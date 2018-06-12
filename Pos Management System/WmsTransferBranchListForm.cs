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
    public partial class WmsTransferBranchListForm : Form
    {
        public WmsTransferBranchListForm()
        {
            InitializeComponent();
        }

        private void WmsTransferBranchListForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
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
                List<WmsTransferOut> list = new List<WmsTransferOut>();
                list = db.WmsTransferOut.Where(w => w.Enable == true &&
                DbFunctions.TruncateTime(w.CreateDate) >= DbFunctions.TruncateTime(dateS) &&
                DbFunctions.TruncateTime(w.CreateDate) <= DbFunctions.TruncateTime(dateE)).OrderBy(w => w.CreateDate).ToList();
                foreach (var item in list)
                {
                    dataGridView1.Rows.Add
                        (
                        Library.ConvertDateToThaiDate(item.CreateDate),
                        Library.GetFullNameUserById(item.CreateBy),
                        item.Code,
                        item.Branch.Name + " " + item.Branch.BranchNo,
                        Library.ConvertDecimalToStringForm(item.TotalQtyUnit),
                        Library.ConvertDecimalToStringForm(item.TotalBalance)
                        );
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenDetailsDialog();
        }

        private void OpenDetailsDialog()
        {
            var cnNo = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value.ToString();
            WmsTransferBranchListDialog obj = new WmsTransferBranchListDialog(this, cnNo);
            obj.ShowDialog();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            OpenDetailsDialog();
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }
    }
}
