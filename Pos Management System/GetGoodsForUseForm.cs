using Pos_Management_System.Singleton;
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
    public partial class GetGoodsForUseForm : Form
    {
        public GetGoodsForUseForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// สำหรับการ เก็บ index เพื่อใช้ logic การ Enter Grid
        /// </summary>
        int row;
        private void GetGoodsForUseForm_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add(1);
            using (SSLsEntities db = new SSLsEntities())
            {
                int count = db.GetGoodsStoreFront.Where(w => w.CreateDate.Year == DateTime.Now.Year && w.CreateDate.Month == DateTime.Now.Month).Count() + 1;
                string docCode = MyConstant.PrefixForGenerateCode.GetGoodsForUse + Singleton.SingletonThisBudgetYear.Instance().ThisYear.CodeYear + DateTime.Now.ToString("MM") + Library.GenerateCodeFormCount(count, 4);
                textBoxTOCode.Text = docCode;
                textBoxTODate.Text = Library.ConvertDateToThaiDate(DateTime.Now);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                this.row = e.RowIndex;
                switch (e.ColumnIndex)
                {
                    case 2:
                        SelectedProductPopup obj = new SelectedProductPopup(this);
                        obj.ShowDialog();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            this.row = e.RowIndex;
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }
        int colId = 0;
        int colCode = 1;
        int colSearch = 2;
        int colName = 3;
        int colUnit = 4;
        int colpz = 5;
        int colQty = 6;
        int colCostPerUnit = 7;
        int colTotalCost = 8;
        int colVatType = 9;
        int colLocation = 10;
        private void dataGridView1_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                int col = dataGridView1.CurrentCell.ColumnIndex;
                var code = dataGridView1.Rows[row].Cells[colCode].Value.ToString();
                ProductDetails getProd = Singleton.SingletonProduct.Instance().ProductDetails.FirstOrDefault(w => w.Enable == true && w.Code == code);
                switch (e.KeyCode)
                {
                    case Keys.Enter:

                        if (col == colCode)
                        {
                            if (getProd != null)
                            {
                                dataGridView1.Rows[row].Cells[colId].Value = getProd.Id;
                                dataGridView1.Rows[row].Cells[colName].Value = getProd.Products.ThaiName;
                                dataGridView1.Rows[row].Cells[colUnit].Value = getProd.ProductUnit.Name;
                                dataGridView1.Rows[row].Cells[colpz].Value = getProd.PackSize;
                                dataGridView1.Rows[row].Cells[colQty].Value = "1.00";
                                dataGridView1.Rows[row].Cells[colCostPerUnit].Value = Library.ConvertDecimalToStringForm(getProd.CostOnly);
                                dataGridView1.Rows[row].Cells[colTotalCost].Value = Library.ConvertDecimalToStringForm(getProd.CostOnly);
                                dataGridView1.Rows[row].Cells[colVatType].Value = getProd.Products.ProductVatType.Name;
                                dataGridView1.Rows[row].Cells[colLocation].Value = "หน้าร้าน";
                                BinddingTotal();
                                dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colQty];
                                dataGridView1.BeginEdit(true);
                            }
                            else
                            {
                                MessageBox.Show("ไม่พบรหัสสินค้านี้","เเจ้งเตือน",MessageBoxButtons.OK,MessageBoxIcon.Error);
                                dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colCode];
                                dataGridView1.BeginEdit(true);
                            }
                            
                            

                        }
                        else if (col == colQty)
                        {

                            if (row == dataGridView1.Rows.Count - 1)
                            {
                                //row = row - 1;
                                dataGridView1.Rows.Add(1);
                                decimal qty = decimal.Parse(dataGridView1.Rows[row].Cells[colQty].Value.ToString());
                                decimal costOnly = decimal.Parse(dataGridView1.Rows[row].Cells[colCostPerUnit].Value.ToString());
                                dataGridView1.Rows[row].Cells[colTotalCost].Value = Library.ConvertDecimalToStringForm(qty * costOnly);
                                dataGridView1.CurrentCell = dataGridView1.Rows[row + 1].Cells[colCode];
                                dataGridView1.BeginEdit(true);
                                row = row + 1;
                            }
                            else
                            {
                                decimal qty = decimal.Parse(dataGridView1.Rows[row].Cells[colQty].Value.ToString());
                                decimal costOnly = decimal.Parse(dataGridView1.Rows[row].Cells[colCostPerUnit].Value.ToString());
                                dataGridView1.Rows[row].Cells[colTotalCost].Value = Library.ConvertDecimalToStringForm(qty * costOnly);
                            }

                        }

                        break;

                }

                BinddingTotal();
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// สรุปยอด การเบิก
        /// </summary>
        void BinddingTotal()
        {
            decimal totalQty = 0;
            //decimal totalUnVat = 0;
            //decimal totalHasVat = 0;
            decimal total = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                var code = dataGridView1.Rows[i].Cells[colCode].Value;
                if (code == null)
                {
                    continue;
                }
                code = code.ToString();
                //ProductDetails getProd = Singleton.SingletonProduct.Instance().ProductDetails.FirstOrDefault(w => w.Enable == true && w.Code == code.ToString());
                totalQty += decimal.Parse(dataGridView1.Rows[i].Cells[colQty].Value.ToString());
                decimal price = decimal.Parse(dataGridView1.Rows[i].Cells[colTotalCost].Value.ToString());
                total += price;
                //if (getProd.Products.FKProductVatType == MyConstant.ProductVatType.UnVat)
                //{
                //    // ถ้าเป็น สินค้า ยกเว้นภาษี 
                //    totalUnVat += price;
                //}
                //else
                //{
                //    totalHasVat += price;
                //}

                //if (i >= dataGridView1.Rows.Count - 2) break;
            }
            // ถอด vat
            //decimal vat = Library.CalVatFromValue(totalHasVat);
            textBoxQtyUnit.Text = Library.ConvertDecimalToStringForm(totalQty);
            //textBoxTotalUnVat.Text = Library.ConvertDecimalToStringForm(totalUnVat);
            //textBoxTotalBeforeVat.Text = Library.ConvertDecimalToStringForm(totalUnVat);
            //textBoxTotalBeforeVat.Text = Library.ConvertDecimalToStringForm(totalHasVat - vat);
            textBoxTotalBalance.Text = Library.ConvertDecimalToStringForm(total);

        }

        /// <summary>
        /// Bindding สินค้า เมื่อเลือกสินค้า
        /// </summary>
        /// <param name="id"></param>
        public void BinddingProductChoose(int id)
        {
            List<ProductDetails> products = Singleton.SingletonProduct.Instance().ProductDetails;
            var product = products.SingleOrDefault(w => w.Id == id);
            // start bind ui
            int currentRow = dataGridView1.CurrentRow.Index;
            dataGridView1.Rows[currentRow].Cells[colId].Value = product.Id;
            dataGridView1.Rows[currentRow].Cells[colCode].Value = product.Code;
            dataGridView1.Rows[currentRow].Cells[colName].Value = product.Products.ThaiName;
            dataGridView1.Rows[currentRow].Cells[colUnit].Value = product.ProductUnit.Name;
            dataGridView1.Rows[currentRow].Cells[colpz].Value = product.PackSize;
            dataGridView1.Rows[currentRow].Cells[colQty].Value = 1.00;

            dataGridView1.Rows[currentRow].Cells[colCostPerUnit].Value = Library.ConvertDecimalToStringForm(product.CostOnly);
            dataGridView1.Rows[currentRow].Cells[colTotalCost].Value = Library.ConvertDecimalToStringForm(product.CostOnly);
            dataGridView1.Rows[currentRow].Cells[colVatType].Value = product.Products.ProductVatType.Name;
            dataGridView1.Rows[currentRow].Cells[colLocation].Value = "หน้าร้าน";
            //dataGridView1.Rows[currentRow].Cells[colQty].Selected = true;
            dataGridView1.CurrentCell = dataGridView1.Rows[currentRow].Cells[colQty];
            dataGridView1.BeginEdit(true);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่ ย้ำกรุณาตรวจสอบรายการให้ถูกต้อง?", "คำเตือนจากระบบ", MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
            switch (dr)
            {
                case DialogResult.Yes:
                    Cursor.Current = Cursors.WaitCursor;
                    SaveCommit();
                    Cursor.Current = Cursors.Default;
                    break;
                case DialogResult.No:
                    break;
            }
        }

        private void SaveCommit()
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                int count = db.GetGoodsStoreFront.Where(w => w.CreateDate.Year == DateTime.Now.Year && w.CreateDate.Month == DateTime.Now.Month).Count() + 1;
                string docCode = MyConstant.PrefixForGenerateCode.GetGoodsForUse + Singleton.SingletonThisBudgetYear.Instance().ThisYear.CodeYear + DateTime.Now.ToString("MM") + Library.GenerateCodeFormCount(count, 4);

                GetGoodsStoreFront gg = new GetGoodsStoreFront();
                List<GetGoodsStoreFrontDetails> details = new List<GetGoodsStoreFrontDetails>();
                GetGoodsStoreFrontDetails detail;
                gg.Enable = true;
                gg.Code = docCode;
                gg.CreateDate = DateTime.Now;
                gg.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                gg.UpdateDate = DateTime.Now;
                gg.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                gg.TotalQtyUnit = decimal.Parse(textBoxQtyUnit.Text);
                gg.TotalBalance = decimal.Parse(textBoxTotalBalance.Text);
                gg.Description = textBoxDesc.Text;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    var code = dataGridView1.Rows[i].Cells[colCode].Value;
                    if (code == null)
                    {
                        continue;
                    }
                    code = code.ToString();

                    detail = new GetGoodsStoreFrontDetails();
                    detail.Enable = true;
                    detail.Description = dataGridView1.Rows[i].Cells[colLocation].Value.ToString();
                    detail.CreateDate = DateTime.Now;
                    detail.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                    detail.UpdateDate = DateTime.Now;
                    detail.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    detail.FKProductDetails = int.Parse(dataGridView1.Rows[i].Cells[colId].Value.ToString());
                    var prodDtl = Singleton.SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == detail.FKProductDetails);

                    detail.Qty = decimal.Parse(dataGridView1.Rows[i].Cells[colQty].Value.ToString());
                    detail.CostPerUnit = decimal.Parse(dataGridView1.Rows[i].Cells[colCostPerUnit].Value.ToString());
                    detail.SellPricePerUnit = prodDtl.SellPrice;
                    details.Add(detail);
                    gg.GetGoodsStoreFrontDetails.Add(detail);

                    // check product นี้ว่ามีในหน้าร้านหรือไม่ ถ้าไม่มีแสดงว่า ไม่เคยเบิกเติมหน้าร้าน เดี่ยวจะมีปันหา
                    var getTransactionPos = db.StoreFrontStockDetails.FirstOrDefault(w => w.Enable == true && w.FKProductDetails == detail.FKProductDetails);
                    if (getTransactionPos == null) // ถ้าไม่มีตัวตนในหน้าร้าน แปลว่า ของไม่มีการเบิกเติม แต่มีสินค้ายุ่จริง
                    {
                        MessageBox.Show("" + code + " " + prodDtl.Products.ThaiName + "(" + prodDtl.ProductUnit.Name + ") " + "ไม่มีในระบบ ไม่สามารถเบิกใช้ได้ กรุณาติดต่อ admin");
                        return;
                    }
                }
                db.GetGoodsStoreFront.Add(gg);
                /// add To Transaction 
                int j = 1;
                foreach (var item in details)
                {
                    var prodDtl = SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == item.FKProductDetails);
                    StoreFrontStockDetails addDtl = new StoreFrontStockDetails();
                    addDtl.DocNo = gg.Code;
                    addDtl.DocDtlNumber = j;
                    addDtl.Description = "เบิกหน้าร้าน ใช้เอง";
                    addDtl.CreateDate = DateTime.Now;
                    addDtl.CreateBy = SingletonAuthen.Instance().Id;
                    addDtl.UpdateDate = DateTime.Now;
                    addDtl.UpdateBy = SingletonAuthen.Instance().Id;
                    addDtl.Enable = true;
                    addDtl.ActionQty = prodDtl.PackSize * item.Qty; // จำนวนหน่วย * pz
                    var stockHD = db.StoreFrontStock.FirstOrDefault(w => w.FKProduct == prodDtl.FKProduct && w.Enable == true);
                    addDtl.FKStoreFrontStock = stockHD.Id;
                    addDtl.FKTransactionType = MyConstant.PosTransaction.GGF;
                    addDtl.Barcode = prodDtl.Code;
                    addDtl.Name = prodDtl.Products.ThaiName;
                    addDtl.FKProductDetails = prodDtl.Id;
                    addDtl.ResultQty = 0;

                    addDtl.PackSize = prodDtl.PackSize;
                    addDtl.DocRefer = "-";
                    addDtl.DocReferDtlNumber = 0;
                    addDtl.CostOnlyPerUnit = prodDtl.CostOnly;
                    addDtl.SellPricePerUnit = prodDtl.SellPrice;
                    db.StoreFrontStockDetails.Add(addDtl);
                    j++;
                }

                db.SaveChanges();
                // open paper               
                frmMainReport report = new frmMainReport(this, gg.Code);
                report.Show();
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();

                dataGridView1.Rows.Add(1);
                count = db.GetGoodsStoreFront.Where(w => w.CreateDate.Year == DateTime.Now.Year && w.CreateDate.Month == DateTime.Now.Month).Count() + 1;
                docCode = MyConstant.PrefixForGenerateCode.GetGoodsForUse + Singleton.SingletonThisBudgetYear.Instance().ThisYear.CodeYear + DateTime.Now.ToString("MM") + Library.GenerateCodeFormCount(count, 4);
                textBoxTOCode.Text = docCode;
                textBoxTODate.Text = Library.ConvertDateToThaiDate(DateTime.Now);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
