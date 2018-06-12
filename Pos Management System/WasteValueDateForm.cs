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
    public partial class WasteValueDateForm : Form
    {
        public WasteValueDateForm()
        {
            InitializeComponent();
        }

        private void WasteValueDateForm_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            using (SSLsEntities db = new SSLsEntities())
            {
                DateTime dateS = dateTimePickerStart.Value;
                var valueget = db.WasteWarehouseDetails.Where(w => w.Enable == true && w.IsInOrOut == true && w.ConfirmCNDate == null && DbFunctions.TruncateTime(w.CreateDate) <= DbFunctions.TruncateTime(dateS)).ToList();
                foreach (var item in valueget)
                {
                    dataGridView1.Rows.Add
                        (
                       item.ProductDetails.Code,
                        item.ProductDetails.Products.ThaiName,
                       item.ProductDetails.PackSize,
                        item.QtyUnit,
                        item.CostOnly,
                        item.SellPrice
                        );
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CodeFileDLL.ExcelReport(dataGridView1, "รายงานของเสียคงคลัง ณ วันที่ "+Library.ConvertDateToThaiDate(dateTimePickerStart.Value), "");
        }
    }
}
