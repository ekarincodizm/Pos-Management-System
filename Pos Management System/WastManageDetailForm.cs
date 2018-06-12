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
    public partial class WastManageDetailForm : Form
    {
        private string code;
        private WastManageListForm wastManageListForm;

        public WastManageDetailForm()
        {
            InitializeComponent();
        }

        public WastManageDetailForm(WastManageListForm wastManageListForm, string code)
        {
            InitializeComponent();
            this.wastManageListForm = wastManageListForm;
            this.code = code;
        }

        private void WastManageDetailForm_Load(object sender, EventArgs e)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                var data = db.StoreFrontTransferWasteDtl.Where(w => w.Enable == true && w.FKStoreFrontTransferWaste == code).ToList();

                foreach (var item in data)
                {
                    dataGridView1.Rows.Add(item.Id, item.ProductDetails.Code,
                        item.ProductDetails.Products.ThaiName, item.ProductDetails.PackSize,
                        item.Qty, Library.ConvertDecimalToStringForm(item.CostPerUnit), item.Description);
                }
                textBox1.Text = code;
                textBox2.Text = Library.ConvertDateToThaiDate(data.FirstOrDefault().CreateDate);
            }
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
    }
}
