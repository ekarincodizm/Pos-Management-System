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
    public partial class DialogWindowsProducts : Form
    {
        CreatePOForm cp;
        ManageProductForm mpf;
        string activeClass = "";
        public DialogWindowsProducts()
        {
            InitializeComponent();
        }
        /// <summary>
        /// เรียกใช้โดย CreatePOForm *PO
        /// </summary>
        /// <param name="cp"></param>
        public DialogWindowsProducts(CreatePOForm cp)
        {
            activeClass = "CreatePOForm";
            this.cp = cp;
            InitializeComponent();
        }
        /// <summary>
        /// เรียกใช้โดย ManageProductForm *สินค้า
        /// </summary>
        /// <param name="mpf"></param>
        public DialogWindowsProducts(ManageProductForm mpf)
        {
            activeClass = "ManageProductForm";
            this.mpf = mpf;
            InitializeComponent();
        }

        private void DialogWindowsProducts_Load(object sender, EventArgs e)
        {
            List<ProductDetails> products = Singleton.SingletonProduct.Instance().ProductDetails;
            dataGridView1.Rows.Clear();
            dataGridView1.ColumnCount = 7;
            dataGridView1.Columns[0].Name = "Id";
            dataGridView1.Columns[0].Visible = false;

            dataGridView1.Columns[1].Name = "รหัส";
            dataGridView1.Columns[2].Name = "ชื่อ";
            dataGridView1.Columns[3].Name = "หน่วย";
            dataGridView1.Columns[4].Name = "ประเภทภาษี";
            dataGridView1.Columns[5].Name = "ยี่ห้อ";
            dataGridView1.Columns[6].Name = "ผู้ผลิต";
            //dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            var columns = (from t in products
                           where t.Enable == true
                           select new
                           {
                               Id = t.Id,
                               Code = t.Code,
                               Name = t.Name,
                               Unit = t.ProductUnit.Name,
                               ProductVatType = t.Products.ProductVatType.Name,
                               Brand = t.Products.ProductBrands.Name,
                               Supplier = t.Products.Supplier.Name
                           }).ToList();

            foreach (var item in columns)
            {
                dataGridView1.Rows.Add(item.Id, item.Code, item.Name, item.Unit, item.ProductVatType, item.Brand, item.Supplier);
            }
        }
        /// <summary>
        /// เลือกสินค้า
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            int iRow = dataGridView1.CurrentRow.Index;
            if (iRow < 0)
            {
                return;
            }

            int id = int.Parse(dataGridView1.Rows[iRow].Cells[0].Value.ToString());
            switch (activeClass)
            {
                case "CreatePOForm":
                    cp.BinddingProductChoose(id);
                    break;
                case "ManageProductForm":
                    mpf.BinddingProduct(id);
                    break;
            }


            this.Dispose();

        }

        private void buttoCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = int.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString());
            switch (activeClass)
            {
                case "CreatePOForm":
                    cp.BinddingProductChoose(id);
                    break;
                case "ManageProductForm":
                    mpf.BinddingProduct(id);
                    break;
            }


            this.Dispose();
        }
    }
}
