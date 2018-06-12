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
    public partial class SaleOrderWarehouseAllowForm : Form
    {
        private SaleOrderWarehouseTakeForm saleOrderWarehouseTakeForm;
        private string code;

        public SaleOrderWarehouseAllowForm()
        {
            InitializeComponent();
        }

        public SaleOrderWarehouseAllowForm(SaleOrderWarehouseTakeForm saleOrderWarehouseTakeForm, string code)
        {
            InitializeComponent();
            this.saleOrderWarehouseTakeForm = saleOrderWarehouseTakeForm;
            this.code = code;
        }
        int colId = 0;
        int colCode = 1;
        int colName = 2;
        int colUnit = 3;
        int colQtyReq = 4;
        int colQtyAllow = 5;
        int colQtyStock = 6;
        int colPricePerUnit = 7;
        int colDiscountBath = 8;
        int colTotalBalance = 9;
        int colVatType = 10;
        int colDescription = 11;
        private void SaleOrderWarehouseAllowForm_Load(object sender, EventArgs e)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                var data = db.SaleOrderWarehouse.SingleOrDefault(w => w.Code == code);
                if (data.FKSaleOrderWarehouseStatus == MyConstant.SaleOrderWarehouseStatus.ConfirmOrder)
                {

                }
                else
                {
                    button6.Enabled = false;
                }
                textBoxCode.Text = data.Code;
                textBoxOrderDate.Text = Library.ConvertDateToThaiDate(data.CreateDate);
                foreach (var item in data.SaleOrderWarehouseDtl.Where(w => w.Enable == true).ToList())
                {
                    var stock = db.WmsStockDetail.Where(w => w.Enable == true && w.FKProductDetail == item.FKProductDtl).OrderByDescending(w => w.CreateDate).FirstOrDefault();
                    if (data.FKSaleOrderWarehouseStatus == MyConstant.SaleOrderWarehouseStatus.WarehouseISS || data.FKSaleOrderWarehouseStatus == MyConstant.SaleOrderWarehouseStatus.WarehouseISSSuccess)
                    {
                        dataGridView1.Rows.Add(
                            item.Id,
                            item.ProductDetails.Code,
                           item.ProductDetails.Products.ThaiName,
                           item.ProductDetails.ProductUnit.Name,
                           item.Qty,
                           item.QtyAllow,
                           stock.ResultQtyUnit,
                           Library.ConvertDecimalToStringForm(item.PricePerUnit),
                           Library.ConvertDecimalToStringForm(item.BathDiscount),
                           Library.ConvertDecimalToStringForm(item.TotalPrice),
                           item.ProductDetails.Products.ProductVatType.Name,
                           item.Description
                   );
                    }
                    else
                    {
                        dataGridView1.Rows.Add(item.Id, item.ProductDetails.Code,
                   item.ProductDetails.Products.ThaiName,
                   item.ProductDetails.ProductUnit.Name,
                   item.Qty,
                   item.Qty,
                   stock.ResultQtyUnit,
                   Library.ConvertDecimalToStringForm(item.PricePerUnit),
                   Library.ConvertDecimalToStringForm(item.BathDiscount),
                   Library.ConvertDecimalToStringForm(item.TotalPrice),
                   item.ProductDetails.Products.ProductVatType.Name,
                   item.Description
                   );
                    }

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
        /// ปิดหน้าต่าง
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        /// <summary>
        /// ยืนยันหยิบ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่ ?", "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    ConfirmISSOrder();
                    break;
                case DialogResult.No:
                    break;
            }
        }
        /// <summary>
        /// ทำการตัด stock ที่ QtyAllow
        /// </summary>
        private void ConfirmISSOrder()
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                var data = db.SaleOrderWarehouse.SingleOrDefault(w => w.Code == code);
                data.FKSaleOrderWarehouseStatus = MyConstant.SaleOrderWarehouseStatus.WarehouseISS;
                data.UpdateDate = DateTime.Now;
                data.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                db.Entry(data).State = EntityState.Modified;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    int id = int.Parse(dataGridView1.Rows[i].Cells[colId].Value.ToString());
                    var dtl = data.SaleOrderWarehouseDtl.SingleOrDefault(w => w.Id == id);
                    dtl.QtyAllow = decimal.Parse(dataGridView1.Rows[i].Cells[colQtyAllow].Value.ToString());
                    dtl.UpdateDate = DateTime.Now;
                    dtl.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    db.Entry(dtl).State = EntityState.Modified;
                }
                db.SaveChanges();
                data = db.SaleOrderWarehouse.SingleOrDefault(w => w.Code == code);
                Library.MakeValueForUpdateStockWms(data.SaleOrderWarehouseDtl.Where(w => w.Enable == true).ToList());
                this.saleOrderWarehouseTakeForm.LoadGrid();
                this.Dispose();

            }
        }
    }
}
