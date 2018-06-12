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
    public partial class PopupProductBaseForm : Form
    {
        private int idProDtl;
        private PORcvInvoiceForm pORcvInvoiceForm;

        public PopupProductBaseForm()
        {
            InitializeComponent();
        }

        public PopupProductBaseForm(PORcvInvoiceForm pORcvInvoiceForm, int idProDtl)
        {
            InitializeComponent();
            this.pORcvInvoiceForm = pORcvInvoiceForm;
            this.idProDtl = idProDtl;
        }

        private void PopupProductBaseForm_Load(object sender, EventArgs e)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                var prod = db.ProductDetails.SingleOrDefault(w => w.Id == idProDtl);
                var prodDtlList = db.ProductDetails.Where(w => w.FKProduct == prod.FKProduct && w.Enable == true);
                foreach (var item in prodDtlList)
                {
                    dataGridView1.Rows.Add(item.Id,
                        item.Code,
                        item.PackSize,
                        item.ProductUnit.Name,
                        Library.ConvertDecimalToStringForm(item.CostOnly));
                }
            }
        }
        /// <summary>
        /// เลือกสินค้า
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                int idDtl = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                pORcvInvoiceForm.BinddingProductDtChoose(idDtl);
                this.Dispose();
                //int getroductDtl = db.ProductDetails.SingleOrDefault(w => w.Id == idDtl);
            }
            
        }
    }
}
