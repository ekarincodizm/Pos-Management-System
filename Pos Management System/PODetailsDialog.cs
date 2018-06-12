using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos_Management_System
{
    public partial class PODetailsDialog : Form
    {
        private POHeader data;

        public PODetailsDialog()
        {
            InitializeComponent();
        }

        public PODetailsDialog(POHeader data)
        {
            InitializeComponent();
            this.data = data;
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

        private void PODetailsDialog_Load(object sender, EventArgs e)
        {
            foreach (var item in data.PODetail.Where(w => w.Enable == true).ToList())
            {
                decimal cost = 0;
                if (data.Vendor.POCostType.Id == MyConstant.POCostType.CostAndVat)
                {
                    // ยึดทุนรวมภาษี
                    cost = item.CostAndVat;
                }
                else
                {
                    cost = item.CostOnly;
                }
                dataGridView1.Rows.Add(item.Id, item.ProductDetails.Code, item.ProductDetails.Products.ThaiName, item.Qty, item.RcvQty, item.GiftQty, item.RcvGiftQty, item.ProductDetails.Products.CostType.Name, Library.ConvertDecimalToStringForm(cost), Library.ConvertDecimalToStringForm(item.CostAndVat - item.CostOnly));
            }
        }
    }
}
