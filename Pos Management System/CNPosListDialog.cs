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
    public partial class CNPosListDialog : Form
    {
        private string cnNo;
        private CNPosListForm cNPosListForm;
        private CNPosListCancelForm cNPosListCancelForm;

        public CNPosListDialog()
        {
            InitializeComponent();
        }
        private string _FromForm = "";
        public CNPosListDialog(CNPosListForm cNPosListForm, string cnNo)
        {
            InitializeComponent();
            this.cNPosListForm = cNPosListForm;
            this.cnNo = cnNo;
            _FromForm = "CNPosListForm";
        }

        public CNPosListDialog(CNPosListCancelForm cNPosListCancelForm, string cnNo)
        {
            InitializeComponent();
            this.cNPosListCancelForm = cNPosListCancelForm;
            this.cnNo = cnNo;
            _FromForm = "CNPosListCancelForm";
        }

        private void CNPosListDialog_Load(object sender, EventArgs e)
        {
            switch (_FromForm)
            {
                case "CNPosListForm":
                    LoadGridCNPosListForm();
                    break;
                case "CNPosListCancelForm":
                    LoadGridCNPosListCancelForm();
                    break;
                default:
                    break;
            }
        }

        private void LoadGridCNPosListForm()
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                var data = db.CNHeader.SingleOrDefault(w => w.Enable == true && w.No == cnNo);
                textBoxCNNo.Text = cnNo;
                textBoxCnDate.Text = Library.ConvertDateToThaiDate(data.CreateDate);
                textBoxInvoiceNo.Text = data.PosHeader.InvoiceNo;
                foreach (var item in data.CNDetails.Where(w => w.Enable == true).ToList())
                {
                    dataGridView1.Rows.Add
                        (
                        item.PosDetails.ProductDetails.Code,
                        item.PosDetails.ProductDetails.Products.ThaiName,
                        item.PosDetails.ProductDetails.ProductUnit.Name,
                        item.PosDetails.ProductDetails.PackSize,
                        Library.ConvertDecimalToStringForm(item.PricePerUnit),
                        Library.ConvertDecimalToStringForm(item.CNQty),
                        Library.ConvertDecimalToStringForm(item.CNDisCoupon),
                        Library.ConvertDecimalToStringForm(item.CNDisShop),
                        Library.ConvertDecimalToStringForm(item.CNTotalPrice + item.CNDisShop + item.CNDisCoupon)
                        );
                }
                textBoxCNTotalPrice.Text = Library.ConvertDecimalToStringForm(data.TotalBalance);
            }
        }
        private void LoadGridCNPosListCancelForm()
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                var data = db.CNHeader.SingleOrDefault(w => w.Enable == false && w.No == cnNo);
                textBoxCNNo.Text = cnNo;
                textBoxCnDate.Text = Library.ConvertDateToThaiDate(data.CreateDate);
                textBoxInvoiceNo.Text = data.PosHeader.InvoiceNo;
                foreach (var item in data.CNDetails.Where(w => w.Enable == false).ToList())
                {
                    dataGridView1.Rows.Add
                        (
                        item.PosDetails.ProductDetails.Code,
                        item.PosDetails.ProductDetails.Products.ThaiName,
                        item.PosDetails.ProductDetails.ProductUnit.Name,
                        item.PosDetails.ProductDetails.PackSize,
                        Library.ConvertDecimalToStringForm(item.PricePerUnit),
                        Library.ConvertDecimalToStringForm(item.CNQty),
                        Library.ConvertDecimalToStringForm(item.CNDisCoupon),
                        Library.ConvertDecimalToStringForm(item.CNDisShop),
                        Library.ConvertDecimalToStringForm(item.CNTotalPrice + item.CNDisShop + item.CNDisCoupon)
                        );
                }
                textBoxCNTotalPrice.Text = Library.ConvertDecimalToStringForm(data.TotalBalance);
            }
        }
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
