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
    public partial class GoodsReturnWmsForm : Form
    {
        public GoodsReturnWmsForm()
        {
            InitializeComponent();
        }

        private void GoodsReturnWmsForm_Load(object sender, EventArgs e)
        {
            textBoxDate.Text = Library.ConvertDateToThaiDate(DateTime.Now);
            string code = "";
            using (SSLsEntities db = new SSLsEntities())
            {
                int currentYear = DateTime.Now.Year;
                int currentMonth = DateTime.Now.Month;
                var cnw = db.CNWarehouse.Where(w => w.CreateDate.Year == currentYear && w.CreateDate.Month == currentMonth).Count() + 1;
                BudgetYear budget = Singleton.SingletonThisBudgetYear.Instance().ThisYear;
                //Branch branch = Singleton.SingletonP
                code = MyConstant.PrefixForGenerateCode.GoodsReturnCN + "" + budget.CodeYear + DateTime.Now.ToString("MM") + Library.GenerateCodeFormCount(cnw, 4);
                textBoxCode.Text = code;
            }
            dataGridView1.Rows.Add(1);
        }
        /// <summary>
        /// เลือกผู้จัดจำหน่าย
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            SelectedVendorPopup dwv = new SelectedVendorPopup(this);
            dwv.ShowDialog();
        }
        int _VendorId;
        public void BinddingVendor(int id)
        {
            _VendorId = id;
            List<Vendor> vendors = SingletonVender.Instance().Vendors;
            var data = vendors.SingleOrDefault(w => w.Id == id);
            textBoxVendorCode.Text = data.Code;
            textBoxVendorName.Text = data.Name;
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }
        /// <summary>
        /// เหตุผล
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            SelectedWasteReasonPopup obj = new SelectedWasteReasonPopup(this);
            obj.ShowDialog();
        }
        int _WasteReason;
        public void BinddingWasteReasonSelected(WasteReason send)
        {
            _WasteReason = send.Id;
            textBoxWRCode.Text = send.Code;
            textBoxWRName.Text = send.Name;
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
        int colId = 0;
        int colCode = 1;
        int colSearch = 2;
        int colName = 3;
        int colUnit = 4;
        int colQty = 5;
        int colPZ = 6;
        int colCostPerUnit = 7;
        int colAllPrice = 8;
        int colVatType = 9;
        int colDescription = 10;
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
            dataGridView1.Rows[currentRow].Cells[colQty].Value = 1.00;
            dataGridView1.Rows[currentRow].Cells[colPZ].Value = product.PackSize;
            dataGridView1.Rows[currentRow].Cells[colCostPerUnit].Value = Library.ConvertDecimalToStringForm(product.CostOnly);
            dataGridView1.Rows[currentRow].Cells[colAllPrice].Value = Library.ConvertDecimalToStringForm(product.CostOnly);
            dataGridView1.Rows[currentRow].Cells[colVatType].Value = product.Products.ProductVatType.Name;
            dataGridView1.Rows[currentRow].Cells[colDescription].Value = "-";
            //dataGridView1.Rows[currentRow].Cells[colQty].Selected = true;
            dataGridView1.CurrentCell = dataGridView1.Rows[currentRow].Cells[colQty];
            dataGridView1.CurrentCell.Selected = true;

        }

        private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                switch (e.ColumnIndex)
                {
                    case 5:
                        decimal qty = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colQty].Value.ToString());
                        decimal cost = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colCostPerUnit].Value.ToString());
                        dataGridView1.Rows[e.RowIndex].Cells[colAllPrice].Value = Library.ConvertDecimalToStringForm(qty * cost);
                        break;
                    default:
                        break;
                }
                // cal summary
                CalSummary();
            }
            catch (Exception)
            {

            }
        }

        private void CalSummary()
        {
            try
            {
                decimal qty = 0;
                decimal hasvat = 0;
                decimal unvat = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    var code = dataGridView1.Rows[i].Cells[colCode].Value;
                    if (code == null)
                    {
                        continue;
                    }
                    qty += decimal.Parse(dataGridView1.Rows[i].Cells[colQty].Value.ToString());

                    int fkProdDtl = int.Parse(dataGridView1.Rows[i].Cells[colId].Value.ToString());
                    var prodDtl = Singleton.SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == fkProdDtl);
                    if (prodDtl.Products.ProductVatType.Id == MyConstant.ProductVatType.HasVat)
                    {
                        // ถ้าเป็นสินค้ามีภาษี
                        hasvat += decimal.Parse(dataGridView1.Rows[i].Cells[colAllPrice].Value.ToString());
                    }
                    else
                    {
                        // ถ้าเป็นสินค้า ยกเว้นภาษี
                        unvat += decimal.Parse(dataGridView1.Rows[i].Cells[colAllPrice].Value.ToString());
                    }

                }
                //decimal vat = hasvat * MyConstant.MyVat.Vat / 100;
                decimal vat = 0;
                decimal total = vat + hasvat + unvat;
                textBoxQtyUnit.Text = Library.ConvertDecimalToStringForm(qty);
                textBoxTotalUnVat.Text = Library.ConvertDecimalToStringForm(unvat);
                textBoxTotalBeforeVat.Text = Library.ConvertDecimalToStringForm(hasvat);
                textBoxTotalVat.Text = Library.ConvertDecimalToStringForm(vat);
                textBoxTotalBalance.Text = Library.ConvertDecimalToStringForm(total);
            }
            catch (Exception)
            {

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // cal summary
            CalSummary();
            DialogResult dr = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่ ?",
                  "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    SaveCommit();
                    break;
                case DialogResult.No:
                    break;
            }
        }

        private void SaveCommit()
        {
            try
            {
                CNWarehouse cnw = new CNWarehouse();

                cnw.Enable = true;
                cnw.Description = textBoxDesc.Text;
                cnw.CreateDate = DateTime.Now;
                cnw.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                cnw.UpdateDate = DateTime.Now;
                cnw.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                cnw.FKWasteReason = _WasteReason;
                decimal qtyPiece = 0;
                List<CNWarehouseDetails> details = new List<CNWarehouseDetails>();
                CNWarehouseDetails detail;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    var code = dataGridView1.Rows[i].Cells[colCode].Value;
                    if (code == null)
                    {
                        continue;
                    }
                    detail = new CNWarehouseDetails();
                    qtyPiece += decimal.Parse(dataGridView1.Rows[i].Cells[colQty].Value.ToString()) * decimal.Parse(dataGridView1.Rows[i].Cells[colPZ].Value.ToString());
                    detail.Enable = true;
                    detail.Description = dataGridView1.Rows[i].Cells[colDescription].Value.ToString();
                    detail.CreateDate = DateTime.Now;
                    detail.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                    detail.UpdateDate = DateTime.Now;
                    detail.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    detail.FKProductDetails = int.Parse(dataGridView1.Rows[i].Cells[colId].Value.ToString());
                    detail.Qty = decimal.Parse(dataGridView1.Rows[i].Cells[colQty].Value.ToString());
                    detail.PricePerUnit = decimal.Parse(dataGridView1.Rows[i].Cells[colCostPerUnit].Value.ToString());
                    detail.BeforeVat = decimal.Parse(dataGridView1.Rows[i].Cells[colCostPerUnit].Value.ToString()) * detail.Qty;
                    detail.Vat = 0;
                    detail.TotalPrice = detail.BeforeVat + detail.Vat;
                    details.Add(detail);
                    //if (i >= dataGridView1.Rows.Count - 2) break;
                }
                cnw.FKVendor = _VendorId;
                cnw.DocDate = DateTime.Now;
                cnw.DocRefer = textBoxDocRefer.Text;
                cnw.TotalQty = qtyPiece;
                cnw.TotalQtyUnit = decimal.Parse(textBoxQtyUnit.Text);
                cnw.TotalUnVat = decimal.Parse(textBoxTotalUnVat.Text);
                cnw.TotalBeforeVat = decimal.Parse(textBoxTotalBeforeVat.Text);
                cnw.TotalVat = decimal.Parse(textBoxTotalVat.Text);
                cnw.TotalBalance = decimal.Parse(textBoxTotalBalance.Text);
                cnw.CNWarehouseDetails = details;

                using (SSLsEntities db = new SSLsEntities())
                {
                    int currentYear = DateTime.Now.Year;
                    int currentMonth = DateTime.Now.Month;
                    var getCNCode = db.CNWarehouse.Where(w => w.CreateDate.Year == currentYear && w.CreateDate.Month == currentMonth).Count() + 1;
                    BudgetYear budget = Singleton.SingletonThisBudgetYear.Instance().ThisYear;
                    //Branch branch = Singleton.SingletonP
                    var code = MyConstant.PrefixForGenerateCode.GoodsReturnCN + "" + budget.CodeYear + DateTime.Now.ToString("MM") + Library.GenerateCodeFormCount(getCNCode, 4);
                    cnw.Code = code;
                    db.CNWarehouse.Add(cnw);
                    db.SaveChanges();
                    // ManageStock Wms
                    //Library.MakeValueForUpdateStockWms(details);
                    MessageBox.Show("บันทึกเรียบร้อย " + cnw.Code);

                    frmMainReport mr = new frmMainReport(this, code);
                    mr.Show();

                    ResetForm();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("พบข้อมผิดพลาด");
            }
        }

        private void ResetForm()
        {
            string code = "";
            using (SSLsEntities db = new SSLsEntities())
            {
                int currentYear = DateTime.Now.Year;
                int currentMonth = DateTime.Now.Month;
                var cnw = db.CNWarehouse.Where(w => w.CreateDate.Year == currentYear && w.CreateDate.Month == currentMonth).Count() + 1;
                BudgetYear budget = Singleton.SingletonThisBudgetYear.Instance().ThisYear;
                //Branch branch = Singleton.SingletonP
                code = MyConstant.PrefixForGenerateCode.GoodsReturnCN + "" + budget.CodeYear + DateTime.Now.ToString("MM") + Library.GenerateCodeFormCount(cnw, 4);
                textBoxCode.Text = code;

            }
        }
        int row;
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
                                dataGridView1.Rows[row].Cells[colCode].Value = getProd.Code;
                                dataGridView1.Rows[row].Cells[colName].Value = getProd.Products.ThaiName;
                                dataGridView1.Rows[row].Cells[colUnit].Value = getProd.ProductUnit.Name;
                                dataGridView1.Rows[row].Cells[colQty].Value = 1.00;
                                dataGridView1.Rows[row].Cells[colPZ].Value = getProd.PackSize;
                                dataGridView1.Rows[row].Cells[colCostPerUnit].Value = Library.ConvertDecimalToStringForm(getProd.CostOnly);
                                dataGridView1.Rows[row].Cells[colAllPrice].Value = Library.ConvertDecimalToStringForm(getProd.CostOnly);
                                dataGridView1.Rows[row].Cells[colVatType].Value = getProd.Products.ProductVatType.Name;
                                dataGridView1.Rows[row].Cells[colDescription].Value = "-";

                                dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colQty];
                                dataGridView1.BeginEdit(true);
                            }
                            else
                            {
                                MessageBox.Show("ไม่พบ");
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
                                dataGridView1.Rows[row].Cells[colAllPrice].Value = Library.ConvertDecimalToStringForm(qty * costOnly);

                                dataGridView1.CurrentCell = dataGridView1.Rows[row + 1].Cells[colCode];
                                dataGridView1.BeginEdit(true);

                            }
                            else
                            {
                                decimal qty = decimal.Parse(dataGridView1.Rows[row].Cells[colQty].Value.ToString());
                                decimal costOnly = decimal.Parse(dataGridView1.Rows[row].Cells[colCostPerUnit].Value.ToString());
                                dataGridView1.Rows[row].Cells[colAllPrice].Value = Library.ConvertDecimalToStringForm(qty * costOnly);
                            }

                        }

                        break;
                }

                CalSummary();
            }
            catch (Exception)
            {

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            this.row = e.RowIndex;
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            this.row = e.RowIndex;
        }
    }
}
