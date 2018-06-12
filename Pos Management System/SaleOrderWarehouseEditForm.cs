using Pos_Management_System.Singleton;
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
    public partial class SaleOrderWarehouseEditForm : Form
    {
        private string code;
        private SaleOrderWarehouseListForm saleOrderWarehouseListForm;

        public SaleOrderWarehouseEditForm()
        {
            InitializeComponent();
        }

        public SaleOrderWarehouseEditForm(SaleOrderWarehouseListForm saleOrderWarehouseListForm, string code)
        {
            InitializeComponent();
            this.saleOrderWarehouseListForm = saleOrderWarehouseListForm;
            this.code = code;
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }
        /// <summary>
        /// load form bindding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaleOrderWarehouseEditForm_Load(object sender, EventArgs e)
        {
            textBoxCode.Text = code;
            using (SSLsEntities db = new SSLsEntities())
            {
                var data = db.SaleOrderWarehouse.SingleOrDefault(w => w.Code == code);
                textBoxOrderDate.Text = Library.ConvertDateToThaiDate(data.CreateDate);
                if (data.FKSaleOrderWarehouseStatus == MyConstant.SaleOrderWarehouseStatus.CreateOrder && data.Enable)
                {

                }
                else
                {
                    button6.Enabled = false;
                }
                foreach (var item in data.SaleOrderWarehouseDtl.Where(w => w.Enable == true).ToList())
                {
                    dataGridView1.Rows.Add(item.FKProductDtl, item.ProductDetails.Code, "",
                        item.ProductDetails.Products.ThaiName,
                        item.ProductDetails.ProductUnit.Name,
                        item.Qty,
                        item.PricePerUnit,
                        Library.ConvertDecimalToStringForm(item.BathDiscount),
                        Library.ConvertDecimalToStringForm(item.TotalPrice),
                        item.ProductDetails.Products.ProductVatType.Name,
                        item.Description);
                }
            }
            TotalSummary();
        }
        int colId = 0;
        int colCode = 1;
        int colSearch = 2;
        int colName = 3;
        int colUnit = 4;
        int colQty = 5;
        int colPricePerUnit = 6;
        int colDiscount = 7;
        int colTotalPrice = 8;
        int colVatType = 9;
        int colRemark = 10;
        /// <summary>
        /// cell click เพื่อค้นหา
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == colSearch && e.RowIndex > -1)
            {
                SelectedProductPopup obj = new SelectedProductPopup(this);
                obj.ShowDialog();
            }
        }
        /// <summary>
        /// Bindding data main product
        /// </summary>
        /// <param name="send"></param>
        public void BinddingProductChoose(int id)
        {
            var send = SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == id);
            int row = dataGridView1.CurrentRow.Index;
            var checkQty = dataGridView1.Rows[row].Cells[colQty].Value ?? "0";
            decimal qty = decimal.Parse(checkQty.ToString());
            dataGridView1.Rows[row].Cells[colId].Value = send.Id;
            dataGridView1.Rows[row].Cells[colCode].Value = send.Code;
            dataGridView1.Rows[row].Cells[colName].Value = send.Products.ThaiName;
            dataGridView1.Rows[row].Cells[colUnit].Value = send.ProductUnit.Name;
            dataGridView1.Rows[row].Cells[colPricePerUnit].Value = Library.ConvertDecimalToStringForm(send.SellPrice);
            dataGridView1.Rows[row].Cells[colVatType].Value = send.Products.ProductVatType.Name;
            dataGridView1.Rows[row].Cells[colDiscount].Value = "0.00";
            dataGridView1.Rows[row].Cells[colRemark].Value = "-";

            if (qty == 0)
            {
                dataGridView1.Rows[row].Cells[colQty].Value = "1";
                dataGridView1.Rows[row].Cells[colTotalPrice].Value = Library.ConvertDecimalToStringForm(send.SellPrice);
            }
            else
            {
                dataGridView1.Rows[row].Cells[colQty].Value = qty;
                dataGridView1.Rows[row].Cells[colTotalPrice].Value = Library.ConvertDecimalToStringForm(qty * send.SellPrice);
            }

            dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colQty];
            dataGridView1.BeginEdit(true);
            TotalSummary();
        }
        void TotalSummary()
        {
            try
            {
                decimal totalQty = 0;
                decimal totalUnVat = 0;
                decimal totalHasVat = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells[colCode].Value == null)
                    {
                        continue;
                    }
                    else if (dataGridView1.Rows[i].Cells[colCode].Value.ToString() == "")
                    {
                        continue;
                    }
                    int id = int.Parse(dataGridView1.Rows[i].Cells[colId].Value.ToString());
                    ProductDetails product = SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == id);
                    decimal qty = decimal.Parse(dataGridView1.Rows[i].Cells[colQty].Value.ToString());
                    totalQty += qty;

                    decimal totalPrice = decimal.Parse(dataGridView1.Rows[i].Cells[colTotalPrice].Value.ToString());
                    if (product.Products.ProductVatType.Id == MyConstant.ProductVatType.UnVat)
                    {
                        // ถ้าเป็น ยกเว้นภาษี                       
                        totalUnVat += totalPrice;
                    }
                    else
                    {
                        totalHasVat += totalPrice;
                    }
                    //if (i >= dataGridView1.Rows.Count - 2) break;
                }
                textBoxQtyUnit.Text = Library.ConvertDecimalToStringForm(totalQty);
                textBoxTotalUnVat.Text = Library.ConvertDecimalToStringForm(totalUnVat);
                decimal vat = Library.CalVatFromValue(totalHasVat);
                decimal beforeVat = totalHasVat - vat;
                textBoxTotalBeforeVat.Text = Library.ConvertDecimalToStringForm(beforeVat);
                textBoxTotalVat.Text = Library.ConvertDecimalToStringForm(vat);
                textBoxTotalBalance.Text = Library.ConvertDecimalToStringForm(totalUnVat + totalHasVat);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string code = dataGridView1.Rows[e.RowIndex].Cells[colCode].Value.ToString();
                var product = SingletonProduct.Instance().ProductDetails.Where(w => w.Enable == true).FirstOrDefault(w => w.Code == code);
                var checkQty = dataGridView1.Rows[e.RowIndex].Cells[colQty].Value ?? "0";
                decimal qty = decimal.Parse(checkQty.ToString());
                switch (e.ColumnIndex)
                {
                    /// Code                
                    case 1:
                        if (product != null)
                        {
                            dataGridView1.Rows[e.RowIndex].Cells[colId].Value = product.Id;
                            dataGridView1.Rows[e.RowIndex].Cells[colName].Value = product.Products.ThaiName;
                            dataGridView1.Rows[e.RowIndex].Cells[colUnit].Value = product.ProductUnit.Name;
                            dataGridView1.Rows[e.RowIndex].Cells[colPricePerUnit].Value = Library.ConvertDecimalToStringForm(product.SellPrice);
                            dataGridView1.Rows[e.RowIndex].Cells[colVatType].Value = product.Products.ProductVatType.Name;
                            if (dataGridView1.Rows[e.RowIndex].Cells[colDiscount].Value == null)
                            {
                                dataGridView1.Rows[e.RowIndex].Cells[colDiscount].Value = "0.00";
                            }

                            if (qty == 0)
                            {
                                dataGridView1.Rows[e.RowIndex].Cells[colQty].Value = "1";
                                dataGridView1.Rows[e.RowIndex].Cells[colTotalPrice].Value = Library.ConvertDecimalToStringForm(product.SellPrice);
                            }
                            else
                            {
                                dataGridView1.Rows[e.RowIndex].Cells[colQty].Value = qty;
                                dataGridView1.Rows[e.RowIndex].Cells[colTotalPrice].Value = Library.ConvertDecimalToStringForm(qty * product.SellPrice);
                            }
                            TotalSummary();
                        }
                        else
                        {
                            //MessageBox.Show("ไม่พบข้อมูล");
                        }
                        break;
                    case 7: // คีย์ ส่วนลด
                        decimal pricePerUnit = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colPricePerUnit].Value.ToString());
                        decimal total = pricePerUnit * qty;
                        decimal discount = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colDiscount].Value.ToString());
                        dataGridView1.Rows[e.RowIndex].Cells[colTotalPrice].Value = Library.ConvertDecimalToStringForm(total - discount);
                        TotalSummary();
                        break;
                    case 5: // จำนวน
                        if (qty == 0)
                        {
                            dataGridView1.Rows[e.RowIndex].Cells[colQty].Value = "1";
                            dataGridView1.Rows[e.RowIndex].Cells[colTotalPrice].Value = Library.ConvertDecimalToStringForm(product.SellPrice);
                        }
                        else
                        {
                            dataGridView1.Rows[e.RowIndex].Cells[colQty].Value = qty;
                            dataGridView1.Rows[e.RowIndex].Cells[colTotalPrice].Value = Library.ConvertDecimalToStringForm(qty * product.SellPrice);
                        }
                        /// คีย์จำนวน ต้องไปเชค Stock 
                        int id = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[colId].Value.ToString());
                        decimal qtyUnit = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colQty].Value.ToString());
                        using (SSLsEntities db = new SSLsEntities())
                        {
                            var stockWms = db.WmsStockDetail.Where(w => w.Enable == true && w.FKProductDetail == id).OrderByDescending(w => w.CreateDate).FirstOrDefault();
                            Console.WriteLine("row " + e.RowIndex + " Want: " + qtyUnit + " stock: " + stockWms.ResultQtyUnit);
                            if (qtyUnit > stockWms.ResultQtyUnit)
                            {
                                MessageBox.Show("Stock คงเหลือ " + stockWms.ResultQtyUnit);
                                //dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[colQty];
                                //dataGridView1.CurrentCell.Selected = true;
                            }
                            else
                            {

                            }
                        }
                        TotalSummary();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {

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
        /// ประมวลผล เพื่อความแน่ใจ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            TotalSummary();
        }
        /// <summary>
        /// บันทึกการแก้ไข โดยไล่ Disable ตัวเดิมทิ้งให้หมด แล้ว add ใหม่
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่ ?", "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    using (SSLsEntities db = new SSLsEntities())
                    {
                        var order = db.SaleOrderWarehouse.SingleOrDefault(w => w.Code == code);
                        foreach (var item in order.SaleOrderWarehouseDtl.Where(w => w.Enable == true).ToList())
                        {
                            item.UpdateDate = DateTime.Now;
                            item.UpdateBy = SingletonAuthen.Instance().Id;
                            item.Enable = false;
                            db.Entry(item).State = EntityState.Modified;
                        }
                        List<SaleOrderWarehouseDtl> details = new List<SaleOrderWarehouseDtl>();
                        SaleOrderWarehouseDtl detail;
                        decimal discountList = 0;
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            detail = new SaleOrderWarehouseDtl();
                            detail.FKSaleOrderWarehouse = order.Id;
                            detail.Enable = true;
                            detail.Description = dataGridView1.Rows[i].Cells[colRemark].Value.ToString();
                            detail.CreateDate = DateTime.Now;
                            detail.CreateBy = SingletonAuthen.Instance().Id;
                            detail.UpdateDate = DateTime.Now;
                            detail.UpdateBy = SingletonAuthen.Instance().Id;
                            detail.FKProductDtl = int.Parse(dataGridView1.Rows[i].Cells[colId].Value.ToString());
                            detail.Qty = decimal.Parse(dataGridView1.Rows[i].Cells[colQty].Value.ToString()); // จำนวนหน่วย
                            detail.PricePerUnit = decimal.Parse(dataGridView1.Rows[i].Cells[colPricePerUnit].Value.ToString()); // จำนวนหน่วย
                            detail.BathDiscount = decimal.Parse(dataGridView1.Rows[i].Cells[colDiscount].Value.ToString()); // จำนวนเงินส่วนลด
                            detail.TotalPrice = decimal.Parse(dataGridView1.Rows[i].Cells[colTotalPrice].Value.ToString()); // จำนวนเงิน หลังหักส่วนลด
                            details.Add(detail);
                            discountList += decimal.Parse(dataGridView1.Rows[i].Cells[colDiscount].Value.ToString());
                            if (i >= dataGridView1.Rows.Count - 2) break;
                        }
                        db.SaleOrderWarehouseDtl.AddRange(details);
                        order.TotalDiscountInList = discountList; // วนใน ลิส
                        order.TotalBalance = decimal.Parse(textBoxTotalBalance.Text);
                        order.UpdateDate = DateTime.Now;
                        order.UpdateBy = SingletonAuthen.Instance().Id;
                        db.Entry(order).State = EntityState.Modified;
                        db.SaveChanges();
                        saleOrderWarehouseListForm.LoadGrid();
                        this.Dispose();
                    }
                    break;
                case DialogResult.No:
                    break;
            }
        }
        private decimal ConvertDecimalNull(float? value)
        {
            if (value == null)
            {
                return 0;
            }
            else
            {
                return (decimal)value;
            }
        }
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string code = dataGridView1.Rows[e.RowIndex].Cells[colCode].Value.ToString();
                var product = SingletonProduct.Instance().ProductDetails.Where(w => w.Enable == true).FirstOrDefault(w => w.Code == code);
                var checkQty = dataGridView1.Rows[e.RowIndex].Cells[colQty].Value ?? "0";
                decimal qty = decimal.Parse(checkQty.ToString());

                int col = dataGridView1.CurrentCell.ColumnIndex;
                int row = dataGridView1.CurrentRow.Index;
                if (col == colCode)
                {
                    if (product != null)
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[colId].Value = product.Id;
                        dataGridView1.Rows[e.RowIndex].Cells[colName].Value = product.Products.ThaiName;
                        dataGridView1.Rows[e.RowIndex].Cells[colUnit].Value = product.ProductUnit.Name;
                        dataGridView1.Rows[e.RowIndex].Cells[colPricePerUnit].Value = Library.ConvertDecimalToStringForm(product.SellPrice);
                        dataGridView1.Rows[e.RowIndex].Cells[colVatType].Value = product.Products.ProductVatType.Name;

                        if (dataGridView1.Rows[e.RowIndex].Cells[colDiscount].Value == null)
                        {
                            dataGridView1.Rows[e.RowIndex].Cells[colDiscount].Value = "0.00";
                        }

                        if (qty == 0)
                        {
                            dataGridView1.Rows[e.RowIndex].Cells[colQty].Value = "0";
                            dataGridView1.Rows[e.RowIndex].Cells[colTotalPrice].Value = "0";
                        }
                        else
                        {
                            dataGridView1.Rows[e.RowIndex].Cells[colQty].Value = qty;
                            dataGridView1.Rows[e.RowIndex].Cells[colTotalPrice].Value = Library.ConvertDecimalToStringForm(qty * product.SellPrice);
                        }
                        TotalSummary();
                    }
                    else
                    {
                        MessageBox.Show("ไม่พบข้อมูล");
                        dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[colCode];
                        dataGridView1.BeginEdit(true);
                    }

                    dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colQty];
                    dataGridView1.BeginEdit(true);
                }
                else if (col == colQty)
                {
                    decimal pricePerunit = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colPricePerUnit].Value.ToString());
                    decimal total = qty * pricePerunit;

                    if (qty == 0)
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[colQty].Value = "1";
                        dataGridView1.Rows[e.RowIndex].Cells[colTotalPrice].Value = Library.ConvertDecimalToStringForm(product.SellPrice);
                    }
                    else
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[colQty].Value = qty;
                        dataGridView1.Rows[e.RowIndex].Cells[colTotalPrice].Value = Library.ConvertDecimalToStringForm(qty * pricePerunit);
                    }
                    /// คีย์จำนวน ต้องไปเชค Stock 
                    int id = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[colId].Value.ToString());
                    decimal qtyUnit = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colQty].Value.ToString());
                    using (WH_TRATEntities db = new WH_TRATEntities())
                    {
                        var locStk = db.WH_LOCSTK.Where(w => w.PRODUCT_NO == code);
                        if (locStk.Count() > 0)
                        {
                            decimal sumQty = ConvertDecimalNull(locStk.Sum(w => w.QTY));
                            decimal sumbook = ConvertDecimalNull(locStk.Sum(w => w.BOOK_QTY));
                            decimal packsize = ConvertDecimalNull(locStk.FirstOrDefault().PACK_SIZE);
                            if (qty > ((sumQty - sumbook) / packsize))
                            {
                                MessageBox.Show("Stock ไม่พอ พบ : " + ((sumQty - sumbook) / packsize) + " " + product.ProductUnit.Name);
                                TotalSummary();

                                dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colQty];
                                dataGridView1.BeginEdit(true);
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("ไม่พบ Stock");
                            dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colCode];
                            dataGridView1.BeginEdit(true);
                            return;
                        }
                        //var stockWms = db.WmsStockDetail.Where(w => w.Enable == true && w.FKProductDetail == id).OrderByDescending(w => w.CreateDate).FirstOrDefault();
                        //Console.WriteLine("row " + e.RowIndex + " Want: " + qtyUnit + " stock: " + stockWms.ResultQtyUnit);
                        //if (qtyUnit > stockWms.ResultQtyUnit)
                        //{
                        //    MessageBox.Show("Stock คงเหลือ " + stockWms.ResultQtyUnit);
                        //    //dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[colQty];
                        //    //dataGridView1.CurrentCell.Selected = true;
                        //}
                        //else
                        //{

                        //}
                    }
                    TotalSummary();

                    dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colPricePerUnit];
                    dataGridView1.BeginEdit(true);
                }
                else if (col == colPricePerUnit)
                {
                    // calculate toal
                    decimal pricePerunit = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colPricePerUnit].Value.ToString());
                    decimal total = qty * pricePerunit;
                    dataGridView1.Rows[e.RowIndex].Cells[colTotalPrice].Value = Library.ConvertDecimalToStringForm(total);
                    dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colDiscount];
                    dataGridView1.BeginEdit(true);
                }
                else if (col == colDiscount)
                {
                    //decimal total = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colTotalPrice].Value.ToString());
                    decimal pricePerUnit = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colPricePerUnit].Value.ToString());
                    decimal total = pricePerUnit * qty;
                    decimal discount = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colDiscount].Value.ToString());
                    dataGridView1.Rows[e.RowIndex].Cells[colTotalPrice].Value = Library.ConvertDecimalToStringForm(total - discount);
                    TotalSummary();

                    if (row == dataGridView1.RowCount - 1)
                    {
                        dataGridView1.Rows.Add(1);
                        dataGridView1.CurrentCell = dataGridView1.Rows[row + 1].Cells[1];
                        dataGridView1.BeginEdit(true);
                    }
                    else
                    {
                        dataGridView1.CurrentCell = dataGridView1.Rows[row + 1].Cells[colDiscount];
                        dataGridView1.BeginEdit(true);
                    }
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
