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
    public partial class ISS2FrontListDetailForm : Form
    {
        private string code;
        private ISS2FrontListForm iSS2FrontListForm;

        public ISS2FrontListDetailForm()
        {
            InitializeComponent();
        }

        public ISS2FrontListDetailForm(ISS2FrontListForm iSS2FrontListForm, string code)
        {
            InitializeComponent();
            this.iSS2FrontListForm = iSS2FrontListForm;
            this.code = code;
        }

        private void ISS2FrontListDetailForm_Load(object sender, EventArgs e)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                var data = db.ISS2Front.SingleOrDefault(w => w.Code == code);
                textBox1.Text = code;
                if (data.FKEmployee != null)
                {
                    textBoxEmpCode.Text = data.Employee.Code;
                    textBoxEmpName.Text = data.Employee.Name;
                }
                textBoxRemark.Text = data.Remark;
                foreach (var item in data.ISS2FrontDetails.Where(w => w.Enable == true).ToList())
                {
                    decimal qtyAllow = 0;
                    if (data.FKISS2FrontStatus == MyConstant.ISS2FrontStatus.ConfirmOrder || data.FKISS2FrontStatus == MyConstant.ISS2FrontStatus.ISSSuccessValue || data.FKISS2FrontStatus == MyConstant.ISS2FrontStatus.ISSSucessNotValue)
                    {
                        qtyAllow = item.QtyAllow;
                    }
                    else
                    {
                        qtyAllow = item.Qty;
                    }
                    var stock = db.WmsStockDetail.Where(w => w.Enable == true && w.FKProductDetail == item.FKProductDetails).OrderByDescending(w => w.CreateDate).FirstOrDefault();
                    dataGridView1.Rows.Add(item.Id, item.ProductDetails.Code,
                        item.ProductDetails.Products.ThaiName,
                        Library.ConvertDecimalToStringForm(item.Qty),
                        Library.ConvertDecimalToStringForm(qtyAllow),
                        Library.ConvertDecimalToStringForm(stock.ResultQtyUnit),
                        Library.ConvertDecimalToStringForm(item.PricePerUnit),
                        item.Shelf.Name, item.Description);
                }                
            }
        }

        private void button2_Click(object sender, EventArgs e)
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
